using System.Reflection;
using UpdateFieldCodeGenerator.Structures;

namespace UpdateFieldCodeGenerator.Formats
{
    public abstract class UpdateFieldHandlerBase : IUpdateFieldHandler
    {
        protected TextWriter _header;
        protected TextWriter _source;
        protected Type _structureType;
        protected bool _create;
        protected bool _writeUpdateMasks;
        protected bool _isRoot;
        protected int _indent = 1;
        protected readonly IDictionary<string, List<int>> _fieldBitIndex = new Dictionary<string, List<int>>();
        protected List<int> _previousFieldCounters;
        protected int _blockGroupBit;
        protected int _blockGroupSize;
        protected int _bitCounter;
        protected int _nonArrayBitCounter;
        protected List<(string Name, bool IsSize, Func<List<FlowControlBlock>, List<FlowControlBlock>> Write)> _fieldWrites;
        protected UpdateFieldFlag _dynamicChangesMaskFlags;

        protected UpdateFieldHandlerBase(TextWriter source, TextWriter header)
        {
            _header = header;
            _source = source;
        }

        protected string GetIndent()
        {
            return "".PadLeft(_indent * 4);
        }

        protected void WriteControlBlocks(TextWriter output, IReadOnlyList<FlowControlBlock> flowControlBlocks, IReadOnlyList<FlowControlBlock> previousControlFlow)
        {
            var blocksMatching = true;
            for (var i = 0; i < flowControlBlocks.Count; ++i)
            {
                var currentBlock = flowControlBlocks[i];
                if (blocksMatching)
                {
                    var previousFieldBlock = previousControlFlow != null && i < previousControlFlow.Count ? previousControlFlow[i] : null;
                    if (previousFieldBlock?.Statement != currentBlock.Statement)
                    {
                        blocksMatching = false;
                        // write closing brackets
                        if (previousFieldBlock != null)
                            for (var j = previousControlFlow.Count; j > i; --j)
                                output.WriteLine($"{"".PadLeft((j + _indent - 1) * 4)}}}");
                    }
                }

                if (!blocksMatching)
                {
                    var pad = "".PadLeft((i + _indent) * 4);
                    output.WriteLine($"{pad}{currentBlock.Statement}");
                    output.WriteLine($"{pad}{{");
                }
            }

            if (blocksMatching && previousControlFlow != null && previousControlFlow.Count > flowControlBlocks.Count)
                for (var i = previousControlFlow.Count; i > flowControlBlocks.Count; --i)
                    output.WriteLine($"{"".PadLeft((i + _indent - 1) * 4)}}}");

            _indent += flowControlBlocks.Count;
        }

        public void FinishControlBlocks(TextWriter output, IReadOnlyList<FlowControlBlock> previousControlFlow)
        {
            if (previousControlFlow != null)
                for (var i = previousControlFlow.Count; i > 0; --i)
                    output.WriteLine($"{"".PadLeft((i + _indent - 1) * 4)}}}");
        }

        public virtual void BeforeStructures()
        {
        }

        public virtual void AfterStructures()
        {
        }

        public virtual void OnStructureBegin(Type structureType, ObjectType objectType, bool create, bool writeUpdateMasks)
        {
            _structureType = structureType;
            _create = create;
            _writeUpdateMasks = writeUpdateMasks;
            _isRoot = false;
            try
            {
                Program.GetObjectType(structureType);
                _isRoot = true;
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            var hasChangesMask = structureType.GetCustomAttribute<HasChangesMaskAttribute>();

            _fieldBitIndex.Clear();
            _blockGroupBit = 0;
            _blockGroupSize = hasChangesMask?.BlockGroupSize ?? 32;
            _bitCounter = HasNonArrayFields(structureType) && /*CountFields(structureType, _ => true) > 1 &&*/ _blockGroupSize > 0 ? 0 : -1;
            _nonArrayBitCounter = 0;
            _fieldWrites = new List<(string Name, bool IsSize, Func<List<FlowControlBlock>, List<FlowControlBlock>> Write)>();
            _dynamicChangesMaskFlags = hasChangesMask?.DynamicChangesMaskFlags ?? UpdateFieldFlag.None;
        }

        public abstract void OnStructureEnd(bool forceMaskMask);

        public abstract IReadOnlyList<FlowControlBlock> OnField(string name, UpdateField updateField, IReadOnlyList<FlowControlBlock> previousBlock);
        public abstract IReadOnlyList<FlowControlBlock> OnDynamicFieldSizeCreate(string name, UpdateField updateField, IReadOnlyList<FlowControlBlock> previousControlFlow);
        public abstract IReadOnlyList<FlowControlBlock> OnDynamicFieldSizeUpdate(string name, UpdateField updateField, IReadOnlyList<FlowControlBlock> previousControlFlow);
        public abstract IReadOnlyList<FlowControlBlock> OnOptionalFieldInitCreate(string name, UpdateField updateField, IReadOnlyList<FlowControlBlock> previousControlFlow);
        public abstract IReadOnlyList<FlowControlBlock> OnOptionalFieldInitUpdate(string name, UpdateField updateField, IReadOnlyList<FlowControlBlock> previousControlFlow);

        public void FinishControlBlocks(string tag)
        {
            _fieldWrites.Add((RenameField(tag), false, (pcf) =>
            {
                FinishControlBlocks(_source, pcf);
                return new List<FlowControlBlock>();
            }));
        }

        public abstract void FinishBitPack(string tag);

        protected static bool HasNonArrayFields(Type type)
        {
            return CountFields(type, field => !field.Type.IsArray) > 0;
        }

        protected static int CountFields(Type type, Func<UpdateField, bool> pred)
        {
            return type.GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(field => typeof(UpdateField).IsAssignableFrom(field.FieldType))
                .Select(field => field.GetValue(null) as UpdateField)
                .Where(pred)
                .Count();
        }

        protected virtual void GenerateBitIndexConditions(UpdateField updateField, string name, List<FlowControlBlock> flowControl, IReadOnlyList<FlowControlBlock> previousControlFlow, int arrayLoopBlockIndex)
        {
            var newField = false;
            var nameForIndex = updateField.UpdateBitGroup != null
                ? RenameField(updateField.UpdateBitGroup)
                : updateField.SizeForField != null ? RenameField(updateField.SizeForField.Name) : name;
            if (!_fieldBitIndex.TryGetValue(nameForIndex, out var bitIndex))
            {
                bitIndex = new List<int>();
                if (flowControl.Count == 0 || !FlowControlBlock.AreChainsAlmostEqual(previousControlFlow, flowControl)
                    || updateField.CustomFlag.HasFlag(CustomUpdateFieldFlag.ForceNewBlockBit))
                {
                    if (!updateField.Type.IsArray)
                    {
                        ++_nonArrayBitCounter;
                        if (_nonArrayBitCounter == _blockGroupSize)
                        {
                            _blockGroupBit = ++_bitCounter;
                            _nonArrayBitCounter = 1;
                        }
                    }

                    bitIndex.Add(++_bitCounter);

                    if (!updateField.Type.IsArray && _blockGroupSize > 0)
                        bitIndex.Add(_blockGroupBit);
                }
                else
                {
                    if (_previousFieldCounters == null || _previousFieldCounters.Count == 1)
                        throw new Exception("Expected previous field to have been an array");

                    bitIndex.Add(_previousFieldCounters[0]);
                }

                _fieldBitIndex[nameForIndex] = bitIndex;
                newField = true;
            }

            if (updateField.Type.IsArray)
            {
                flowControl.Insert(0, new FlowControlBlock { Statement = $"if (changesMask[{bitIndex[0]}])" });
                var bitsToGenerate = updateField.Size;
                var conditionIncrement = " + i";
                if (typeof(DynamicUpdateField).IsAssignableFrom(updateField.Type.GetElementType()))
                {
                    bitsToGenerate = 1;
                    conditionIncrement = string.Empty;
                }
                if (updateField.CustomFlag.HasFlag(CustomUpdateFieldFlag.NoArrayElementBits))
                    bitsToGenerate = 0;

                if (newField)
                {
                    bitIndex.AddRange(Enumerable.Range(_bitCounter + 1, bitsToGenerate));
                    _bitCounter += bitsToGenerate;
                }

                if (bitsToGenerate > 0)
                    flowControl.Insert(arrayLoopBlockIndex + 1, new FlowControlBlock { Statement = $"if (changesMask[{bitIndex[1]}{conditionIncrement}])" });
            }
            else
            {
                if (_blockGroupSize > 0)
                {
                    flowControl.Insert(0, new FlowControlBlock { Statement = $"if (changesMask[{bitIndex[1]}])" });
                    flowControl.Insert(1, new FlowControlBlock { Statement = $"if (changesMask[{bitIndex[0]}])" });
                }
                else
                    flowControl.Insert(0, new FlowControlBlock { Statement = $"if (changesMask[{bitIndex[0]}])" });
            }

            _previousFieldCounters = bitIndex;
        }

        protected abstract string RenameType(Type type);

        protected abstract string RenameField(string name);

        protected void PostProcessFieldWrites()
        {
            void moveFieldRelativeToField(string fieldToMove, bool fieldIsSize, string where, bool whereIsSize, bool isBefore)
            {
                fieldToMove = RenameField(fieldToMove);
                where = RenameField(where);
                var movedFieldIndex = _fieldWrites.FindIndex(fieldWrite => fieldWrite.Name == fieldToMove && fieldWrite.IsSize == fieldIsSize);
                if (movedFieldIndex == -1)
                    throw new ArgumentOutOfRangeException(nameof(fieldToMove), fieldToMove, "Field not found");

                var whereFieldIndex = _fieldWrites.FindIndex(fieldWrite => fieldWrite.Name == where && fieldWrite.IsSize == whereIsSize);
                if (whereFieldIndex == -1)
                    throw new ArgumentOutOfRangeException(nameof(where), where, "Field not found");

                // move to just-before-last field
                var movedField = _fieldWrites[movedFieldIndex];
                _fieldWrites.RemoveAt(movedFieldIndex);
                int offset = isBefore ? 0 : 1;
                _fieldWrites.Insert(whereFieldIndex < movedFieldIndex ? whereFieldIndex + offset : whereFieldIndex - 1 + offset, movedField);
            }

            void moveFieldBeforeField(string fieldToMove, bool fieldIsSize, string where, bool whereIsSize)
            {
                moveFieldRelativeToField(fieldToMove, fieldIsSize, where, whereIsSize, true);
            }

            void moveFieldAfterField(string fieldToMove, bool fieldIsSize, string where, bool whereIsSize)
            {
                moveFieldRelativeToField(fieldToMove, fieldIsSize, where, whereIsSize, false);
            }

            void moveFieldToEnd(string fieldToMove)
            {
                moveFieldBeforeField(fieldToMove, false, "OnStructureEnd", false);
            }

            void removeField(string fieldToRemove)
            {
                var removedFields = _fieldWrites.RemoveAll(fieldWrite => fieldWrite.Name == fieldToRemove);
                if (removedFields <= 0)
                    throw new ArgumentOutOfRangeException(nameof(fieldToRemove), fieldToRemove, "Field not found");
            }

            if (_structureType == typeof(JamMirrorUnitAssistActionData_C))
            {
                moveFieldAfterField("m_virtualRealmAddress", false, "m_type", false);

                if (this is TrinityCoreHandler)
                {
                    FinishBitPack("name_length");
                    moveFieldAfterField("name_length", false, "m_playerName{0}size()", false);
                }
            }
            else if (_structureType == typeof(JamMirrorQuestLog_C))
            {
                if (_create)
                    moveFieldBeforeField("m_objectiveProgress", false, "m_endTime", false);
            }
            else if (_structureType == typeof(JamMirrorPetCreatureName_C))
            {
                if (this is TrinityCoreHandler)
                {
                    FinishBitPack("name_length");
                    moveFieldAfterField("name_length", false, "m_name{0}size()", false);
                }
            }
            else if (_structureType == typeof(CGPlayerData))
            {
                moveFieldBeforeField("name", false, "declinedNames", false);
                if (_create)
                {
                    moveFieldBeforeField("name{0}size()", false, "hasQuestSession", false);
                }
                else
                {
                    if (this is TrinityCoreHandler)
                        moveFieldBeforeField("declinedNames.has_value()", false, "WriteUpdate_FinishControlBlocks_Optionals", false);
                    else
                        moveFieldAfterField("declinedNames.has_value()", false, "WriteUpdate_FinishBitPack_Optionals", false);

                    moveFieldBeforeField("name{0}size()", false, "declinedNames.has_value()", false);
                    return; // don't reorder ResetBitReader in WPP after name size
                }
            }
            else if (_structureType == typeof(JamMirrorDeclinedNames_C))
            {
                FinishControlBlocks("SplitBits");
                moveFieldBeforeField("SplitBits", false, "m_name", false);
                if (!_create)
                    moveFieldBeforeField("WriteUpdate_FinishBitPack_after_DynamicField_sizes", false, "m_name", false);
            }
            else if (_structureType == typeof(CGActivePlayerData))
            {
                if (!_create)
                {
                    moveFieldAfterField("researchSites", true, "pvpInfo", true);
                    moveFieldAfterField("researchSiteProgress", true, "researchSites", true);
                    moveFieldAfterField("research", true, "researchSiteProgress", true);
                }

                moveFieldAfterField("researchSites", false, "research", true);
                moveFieldAfterField("researchSiteProgress", false, "researchSites", false);
                moveFieldAfterField("research", false, "researchSiteProgress", false);

                if (_create)
                {
                    moveFieldAfterField("characterBankTabSettings", true, "petStable.has_value()", false);
                    moveFieldAfterField("accountBankTabSettings", true, "characterBankTabSettings", true);

                    moveFieldAfterField("characterBankTabSettings", false, "petStable", false);
                    moveFieldAfterField("accountBankTabSettings", false, "characterBankTabSettings", false);
                }
                else
                {
                    // dynamic fields with sizes as bits
                    FinishControlBlocks("blocks_after_accountBankTabSettings");
                    FinishBitPack("bits_after_accountBankTabSettings");
                    moveFieldBeforeField("blocks_after_accountBankTabSettings", false, "farsightObject", false);
                    moveFieldBeforeField("bits_after_accountBankTabSettings", false, "farsightObject", false);
                    moveFieldAfterField("characterBankTabSettings", true, "bits_after_accountBankTabSettings", false);
                    moveFieldAfterField("accountBankTabSettings", true, "characterBankTabSettings", true);
                    moveFieldAfterField("characterBankTabSettings", false, "accountBankTabSettings", true);
                    moveFieldAfterField("accountBankTabSettings", false, "characterBankTabSettings", false);

                    moveFieldAfterField("questSession.has_value()", false, "WriteUpdate_FinishControlBlocks_Optionals", false);
                    moveFieldAfterField("petStable.has_value()", false, "questSession.has_value()", false);
                    moveFieldAfterField("walkInData.has_value()", false, "petStable.has_value()", false);
                    moveFieldAfterField("challengeModeData.has_value()", false, "walkInData.has_value()", false);

                    FinishControlBlocks("blocks_after_research");
                    FinishBitPack("bits_after_research");
                    moveFieldAfterField("blocks_after_research", false, "research", false);
                    moveFieldAfterField("bits_after_research", false, "blocks_after_research", false);
                }
            }
            else if (_structureType == typeof(JamMirrorTraitConfig_C))
            {
                moveFieldToEnd("m_name");
                moveFieldBeforeField("m_name{0}size()", false, "m_name", false);

                FinishBitPack("name_length");
                moveFieldRelativeToField("name_length", false, "m_name{0}size()", false, this is WowPacketParserHandler);

                FinishControlBlocks("name_length_block");
                moveFieldBeforeField("name_length_block", false, "name_length", false);
            }
            else if (_structureType == typeof(JamMirrorCraftingOrder_C))
            {
                if (_create)
                {
                    moveFieldBeforeField("m_recraftItemInfo.has_value()", false, "m_enchantments", true);
                    moveFieldBeforeField("m_recraftItemInfo", false, "m_enchantments", false);
                }
            }
            else if (_structureType == typeof(JamMirrorCraftingOrderData_C))
            {
                moveFieldBeforeField("m_customerNotes{0}size()", false, "m_customer.has_value()", false);
                moveFieldBeforeField("m_customerNotes", false, "m_customer", false);

                if (!_create && this is WowPacketParserHandler)
                {
                    moveFieldBeforeField("WriteUpdate_FinishControlBlocks_Optionals", false, "m_customerNotes{0}size()", false);
                    moveFieldBeforeField("WriteUpdate_FinishBitPack_Optionals", false, "m_customerNotes{0}size()", false);
                }

                return; // don't reorder ResetBitReader in WPP after customerNotes size
            }
            else if (_structureType == typeof(JamMirrorCraftingOrderItem_C))
            {
                if (!_create)
                    moveFieldAfterField("m_dataSlotIndex.has_value()", false, "m_flags", false);
            }
            else if (_structureType == typeof(JamMirrorStablePetInfo_C))
            {
                moveFieldBeforeField("m_petFlags", false, "m_name{0}size()", false);
                moveFieldBeforeField("m_specialization", false, "m_name{0}size()", false);

                FinishBitPack("name_length");
                moveFieldRelativeToField("name_length", false, "m_name{0}size()", false, this is WowPacketParserHandler);

                FinishControlBlocks("name_length_block");
                moveFieldBeforeField("name_length_block", false, "name_length", false);
            }
            else if (_structureType == typeof(JamMirrorBankTabSettings_C))
            {
                moveFieldBeforeField("m_depositFlags", false, "m_name", false);
                if (!_create)
                {
                    FinishControlBlocks("FinishControlBlocks_after_sizes");
                    moveFieldBeforeField("FinishControlBlocks_after_sizes", false, "m_depositFlags", false);
                    moveFieldBeforeField("WriteUpdate_FinishBitPack_after_DynamicField_sizes", false, "m_depositFlags", false);
                }
            }
            else if (_structureType == typeof(JamMirrorTransmogOutfitDataInfo_C))
            {
                moveFieldBeforeField("icon", false, "name{0}size()", false);
                if (_create)
                {
                    moveFieldToEnd("name");
                    if (this is WowPacketParserHandler)
                    {
                        moveFieldBeforeField("WriteCreate_FinishControlBlocks", false, "m_name{0}size()", false);
                        moveFieldBeforeField("WriteCreate_FinishBitPack", false, "m_name{0}size()", false);
                    }
                }
            }
            else if (_structureType == typeof(JamMirrorGameObjectAssistActionData_C))
            {
                moveFieldBeforeField("m_monsterName{0}size()", false, "m_virtualRealmAddress", false);
                moveFieldBeforeField("m_playerName{0}size()", false, "m_monsterName{0}size()", false);

                moveFieldToEnd("m_playerName");
                moveFieldToEnd("m_monsterName");
            }
            else if (_structureType == typeof(CGAreaTriggerData))
            {
                if (!_create)
                {
                    if (this is TrinityCoreHandler)
                    {
                        moveFieldBeforeField("m_targetRollPitchYaw.has_value()", false, "WriteUpdate_FinishControlBlocks_Optionals", false);
                        moveFieldBeforeField("m_forcedPositionAndRotation.has_value()", false, "WriteUpdate_FinishControlBlocks_Optionals", false);
                    }
                    else
                    {
                        moveFieldAfterField("m_targetRollPitchYaw.has_value()", false, "WriteUpdate_FinishBitPack_Optionals", false);
                        moveFieldAfterField("m_forcedPositionAndRotation.has_value()", false, "m_targetRollPitchYaw.has_value()", false);
                    }
                }
            }
            else if (_structureType == typeof(JamMirrorVisualAnim_C))
            {
                if (_create)
                {
                    moveFieldBeforeField("m_animKitID", false, "m_animationDataID", false);
                    moveFieldBeforeField("m_serverTime", false, "m_animationDataID", false);
                }
                else
                {
                    moveFieldAfterField("m_animationDataID.has_value()", false, "m_isDecay", false);
                    return; // don't reorder ResetBitReader in WPP after customerNotes size
                }
            }
            else if (_structureType == typeof(JamMirrorAreaTriggerSplineCalculator_C))
            {
                if (_create)
                    moveFieldAfterField("m_linear", false, "m_points", true);
            }
            else if (_structureType == typeof(CGConversationData))
            {
                if (_create)
                {
                    moveFieldBeforeField("m_dontPlayBroadcastTextSounds", false, "m_actors", true);
                    moveFieldBeforeField("m_field_33", false, "m_actors", true);
                }
            }
            else if (_structureType == typeof(CGMeshObjectData))
            {
                moveFieldBeforeField("m_fileDataID", false, "m_geobox", false);
                if (!_create)
                {
                    removeField("WriteUpdate_FinishControlBlocks_after_DynamicField_sizes");
                    removeField("WriteUpdate_FinishBitPack_after_DynamicField_sizes");
                }
            }
            else if (_structureType == typeof(JamMirrorDecorStoragePersistedData_C))
            {
                moveFieldAfterField("m_sourceValue", false, "m_dyeSlots", false);
                if (!_create)
                    moveFieldRelativeToField("m_dyeSlots.has_value()", false, "WriteUpdate_FinishControlBlocks_Optionals", false, this is TrinityCoreHandler);

                moveFieldAfterField("m_sourceValue{0}size()", false, "m_dyeSlots.has_value()", false);
            }
            else if (_structureType == typeof(JamMirrorDecorPetInfo_C))
            {
                moveFieldBeforeField("m_petBehavior", false, "m_petName{0}size()", false);

                FinishBitPack("name_length");
                moveFieldRelativeToField("name_length", false, "m_petName{0}size()", false, this is WowPacketParserHandler);

                FinishControlBlocks("name_length_block");
                moveFieldBeforeField("name_length_block", false, "name_length", false);
            }
            else if (_structureType == typeof(CGHousingDecorData))
            {
                if (!_create)
                {
                    moveFieldAfterField("m_persistedData.has_value()", false, "WriteUpdate_FinishControlBlocks_Optionals", false);
                    moveFieldAfterField("m_petInfo.has_value()", false, "m_persistedData.has_value()", false);
                }
            }
            else if (_structureType == typeof(CGNeighborhoodMirrorData))
            {
                if (_create)
                    moveFieldBeforeField("m_name{0}size()", false, "m_ownerGUID", false);
                else
                {
                    FinishBitPack("FinishBitPack_beforeNameLength");
                    FinishControlBlocks("FinishControlBlocks_beforeNameLength");
                    moveFieldBeforeField("FinishControlBlocks_beforeNameLength", false, "m_name{0}size()", false);
                    moveFieldBeforeField("FinishBitPack_beforeNameLength", false, "m_name{0}size()", false);
                }
            }
            else if (_structureType == typeof(JamMirrorNeighborhoodCharter_C))
            {
                FinishBitPack("name_length");
                moveFieldRelativeToField("name_length", false, "m_name{0}size()", false, this is WowPacketParserHandler);

                FinishControlBlocks("name_length_block");
                moveFieldBeforeField("name_length_block", false, "name_length", false);
            }
            else if (_structureType == typeof(JamMirrorNeighborhoodOwnershipTransfer_C))
            {
                FinishBitPack("FinishBitPack_afterLength");
                moveFieldAfterField("FinishBitPack_afterLength", false, "m_neighborhoodName{0}size()", false);
            }

            if (!_create && this is WowPacketParserHandler)
            {
                var firstOptional = _fieldWrites.FindIndex(field => field.Name.EndsWith(".has_value()"));
                if (firstOptional >= 0)
                {
                    var where = _fieldWrites[firstOptional].Name;
                    moveFieldBeforeField("WriteUpdate_FinishControlBlocks_Optionals", false, where, false);
                    moveFieldBeforeField("WriteUpdate_FinishBitPack_Optionals", false, where, false);
                }
            }
        }

        public void Dispose()
        {
            if (_source != null)
            {
                _source.Dispose();
                _source = null;
            }
            if (_header != null)
            {
                _header.Dispose();
                _header = null;
            }
        }
    }
}

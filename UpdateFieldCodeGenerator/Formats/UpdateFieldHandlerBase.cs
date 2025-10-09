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
        protected List<string> _dynamicChangesMaskTypes;

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
            _fieldBitIndex.Clear();
            _blockGroupBit = 0;
            _blockGroupSize = structureType.GetCustomAttribute<HasChangesMaskAttribute>()?.BlockGroupSize ?? 32;
            _bitCounter = HasNonArrayFields(structureType) && /*CountFields(structureType, _ => true) > 1 &&*/ _blockGroupSize > 0 ? 0 : -1;
            _nonArrayBitCounter = 0;
            _fieldWrites = new List<(string Name, bool IsSize, Func<List<FlowControlBlock>, List<FlowControlBlock>> Write)>();
            _dynamicChangesMaskTypes = new List<string>();
        }

        public abstract void OnStructureEnd(bool needsFlush, bool forceMaskMask);

        public abstract IReadOnlyList<FlowControlBlock> OnField(string name, UpdateField updateField, IReadOnlyList<FlowControlBlock> previousBlock);
        public abstract IReadOnlyList<FlowControlBlock> OnDynamicFieldSizeCreate(string name, UpdateField updateField, IReadOnlyList<FlowControlBlock> previousControlFlow);
        public abstract IReadOnlyList<FlowControlBlock> OnDynamicFieldSizeUpdate(string name, UpdateField updateField, IReadOnlyList<FlowControlBlock> previousControlFlow);
        public abstract IReadOnlyList<FlowControlBlock> OnOptionalFieldInitCreate(string name, UpdateField updateField, IReadOnlyList<FlowControlBlock> previousControlFlow);
        public abstract IReadOnlyList<FlowControlBlock> OnOptionalFieldInitUpdate(string name, UpdateField updateField, IReadOnlyList<FlowControlBlock> previousControlFlow);

        public abstract void FinishControlBlocks(IReadOnlyList<FlowControlBlock> previousControlFlow, string tag);

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

        protected abstract string RenameType(Type type);

        protected abstract string RenameField(string name);

        protected void PostProcessFieldWrites()
        {
            void moveFieldBeforeField(string fieldToMove, bool fieldIsSize, string where, bool whereIsSize)
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
                _fieldWrites.Insert(whereFieldIndex < movedFieldIndex ? whereFieldIndex : whereFieldIndex - 1, movedField);
            }

            void moveFieldToEnd(string fieldToMove)
            {
                moveFieldBeforeField(fieldToMove, false, "OnStructureEnd", false);
            }

            if (_structureType == typeof(CGItemData))
            {
                if (!_create)
                    moveFieldBeforeField("m_modifiers", false, "m_spellCharges", false);
            }
            else if (_structureType == typeof(JamMirrorUnitAssistActionData_C))
            {
                if (_create)
                    moveFieldToEnd("m_playerName");
                else
                    moveFieldBeforeField("m_virtualRealmAddress", false, "m_playerName{0}size()", false);

                FinishBitPack("name_length");
                moveFieldBeforeField("name_length", false, "m_playerName", false);
            }
            else if (_structureType == typeof(CGUnitData))
            {
                if (_create)
                {
                    moveFieldToEnd("assistActionData.has_value()");
                    moveFieldToEnd("assistActionData");
                }
                else
                {
                    FinishBitPack("FinishBitPack_Optionals");
                    if (this is WowPacketParserHandler)
                    {
                        FinishControlBlocks(null, "FinishControlBlocks_Optionals");
                        moveFieldBeforeField("FinishControlBlocks_Optionals", false, "assistActionData.has_value()", false);
                        moveFieldBeforeField("FinishBitPack_Optionals", false, "assistActionData.has_value()", false);
                    }
                    else
                        moveFieldBeforeField("FinishBitPack_Optionals", false, "assistActionData", false);
                }
            }
            else if (_structureType == typeof(CGPlayerData))
            {
                if (_create)
                {
                    moveFieldToEnd("declinedNames");
                    moveFieldBeforeField("name{0}size()", false, "hasQuestSession", false);
                    moveFieldBeforeField("declinedNames.has_value()", false, "declinedNames", false);
                    moveFieldBeforeField("dungeonScore", false, "declinedNames", false);
                    moveFieldBeforeField("name", false, "declinedNames", false);
                    moveFieldBeforeField("leaverInfo", false, "declinedNames", false);
                    moveFieldBeforeField("visibleEquipableSpells", false, "declinedNames", false);
                    moveFieldBeforeField("petNames", false, "declinedNames", false);
                }
                else
                {
                    FinishBitPack("FinishBitPack_afterDeclinedNamesBit");

                    moveFieldBeforeField("name{0}size()", false, "partyType", false);
                    moveFieldBeforeField("declinedNames.has_value()", false, "partyType", false);
                    moveFieldBeforeField("FinishBitPack_afterDeclinedNamesBit", false, "partyType", false);
                    moveFieldBeforeField("dungeonScore", false, "partyType", false);
                    moveFieldBeforeField("name", false, "partyType", false);
                    moveFieldBeforeField("leaverInfo", false, "partyType", false);
                    moveFieldBeforeField("declinedNames", false, "partyType", false);
                    moveFieldToEnd("visibleEquipableSpells");
                }
            }
            else if (_structureType == typeof(JamMirrorDeclinedNames_C))
            {
                FinishControlBlocks(null, "SplitBits");
                moveFieldBeforeField("SplitBits", false, "m_name", false);
                if (!_create)
                    moveFieldBeforeField("WriteUpdate_FinishBitPack_after_DynamicField_sizes", false, "m_name", false);
            }
            else if (_structureType == typeof(CGActivePlayerData))
            {
                moveFieldBeforeField("researchSites", true, "dailyQuestsCompleted", true);
                moveFieldBeforeField("researchSiteProgress", true, "dailyQuestsCompleted", true);
                moveFieldBeforeField("research", true, "dailyQuestsCompleted", true);
                moveFieldBeforeField("researchSites", false, "dailyQuestsCompleted", true);
                moveFieldBeforeField("researchSiteProgress", false, "dailyQuestsCompleted", true);
                moveFieldBeforeField("research", false, "dailyQuestsCompleted", true);

                if (_create)
                {
                    moveFieldToEnd("challengeModeData");
                    moveFieldBeforeField("delveData", false, "challengeModeData", false);
                    moveFieldBeforeField("walkInData", false, "delveData", false);
                    moveFieldBeforeField("accountBankTabSettings", false, "walkInData", false);
                    moveFieldBeforeField("characterBankTabSettings", false, "accountBankTabSettings", false);
                    moveFieldBeforeField("petStable", false, "characterBankTabSettings", false);

                    moveFieldBeforeField("dungeonScore", false, "pvpInfo", false);
                }
                else
                {
                    FinishControlBlocks(null, "blocks_after_accountBankTabSettings");
                    FinishBitPack("bits_after_accountBankTabSettings");
                    moveFieldBeforeField("blocks_after_accountBankTabSettings", false, "pvpInfo", false);
                    moveFieldBeforeField("bits_after_accountBankTabSettings", false, "pvpInfo", false);

                    moveFieldBeforeField("characterBankTabSettings", true, "blocks_after_accountBankTabSettings", false);
                    moveFieldBeforeField("accountBankTabSettings", true, "blocks_after_accountBankTabSettings", false);

                    FinishControlBlocks(null, "blocks_before_accountBankTabSettings");
                    FinishBitPack("bits_before_accountBankTabSettings");
                    moveFieldBeforeField("blocks_before_accountBankTabSettings", false, "characterBankTabSettings", true);
                    moveFieldBeforeField("bits_before_accountBankTabSettings", false, "characterBankTabSettings", true);

                    moveFieldBeforeField("challengeModeData", false, "invSlots", false);
                    moveFieldBeforeField("delveData", false, "challengeModeData", false);
                    moveFieldBeforeField("walkInData", false, "delveData", false);
                    moveFieldBeforeField("petStable", false, "walkInData", false);
                    moveFieldBeforeField("dungeonScore", false, "petStable", false);

                    FinishControlBlocks(null, string.Empty);
                    FinishBitPack("FinishBitPack_afterResearch");

                    var finishBitPack = _fieldWrites.GetRange(_fieldWrites.Count - 2, 2);
                    _fieldWrites.RemoveRange(_fieldWrites.Count - 2, 2);

                    var researchIndex = _fieldWrites.FindIndex(fieldWrite => fieldWrite.Name == RenameField("research") && !fieldWrite.IsSize);
                    _fieldWrites.InsertRange(researchIndex + 1, finishBitPack);
                }

                moveFieldBeforeField("field_1410", false, "dungeonScore", false);
                moveFieldBeforeField("frozenPerksVendorItem", false, "field_1410", false);
                moveFieldBeforeField("questSession", false, "frozenPerksVendorItem", false);
                moveFieldBeforeField("researchHistory", false, "questSession", false);
                moveFieldBeforeField("challengeModeData.has_value()", false, "researchHistory", false);
                moveFieldBeforeField("delveData.has_value()", false, "challengeModeData.has_value()", false);
                moveFieldBeforeField("walkInData.has_value()", false, "delveData.has_value()", false);

                if (_create)
                {
                    moveFieldBeforeField("characterBankTabSettings", true, "walkInData.has_value()", false);
                    moveFieldBeforeField("accountBankTabSettings", true, "walkInData.has_value()", false);
                    moveFieldBeforeField("petStable.has_value()", false, "characterBankTabSettings", true);
                }
                else
                {
                    moveFieldBeforeField("petStable.has_value()", false, "walkInData.has_value()", false);
                }

                moveFieldBeforeField("questSession.has_value()", false, "petStable.has_value()", false);

                FinishControlBlocks(null, "FinishControlBlocks_Optionals");
                FinishBitPack("FinishBitPack_Optionals");

                if (!_create && this is WowPacketParserHandler)
                {
                    moveFieldBeforeField("FinishControlBlocks_Optionals", false, "questSession.has_value()", false);
                    moveFieldBeforeField("FinishBitPack_Optionals", false, "questSession.has_value()", false);
                }
                else
                {
                    moveFieldBeforeField("FinishControlBlocks_Optionals", false, "researchHistory", false);
                    moveFieldBeforeField("FinishBitPack_Optionals", false, "researchHistory", false);
                }
            }
            else if (_structureType == typeof(JamMirrorPlayerDataElement_C))
            {
                if (_create)
                    moveFieldBeforeField("m_type", false, "m_floatValue", false);
            }
            else if (_structureType == typeof(JamMirrorTraitConfig_C))
            {
                moveFieldToEnd("m_name");
                moveFieldBeforeField("m_name{0}size()", false, "m_name", false);
                if (_create)
                    moveFieldBeforeField("m_subTrees", false, "m_name", false);

            }
            else if (_structureType == typeof(JamMirrorTraitSubTreeCache_C))
            {
                if (!_create)
                    moveFieldBeforeField("m_traitSubTreeID", false, "m_entries{0}size()", false);

            }
            else if (_structureType == typeof(JamMirrorCraftingOrder_C))
            {
                if (_create)
                {
                    moveFieldBeforeField("m_data", false, "m_recraftItemInfo", false);
                    moveFieldBeforeField("m_recraftItemInfo", false, "m_enchantments", false);
                    moveFieldBeforeField("m_recraftItemInfo.has_value()", false, "m_enchantments", true);
                }

                FinishBitPack("FinishBitPack_afterOptionalBit");
                moveFieldBeforeField("FinishBitPack_afterOptionalBit", false, "m_recraftItemInfo", false);
            }
            else if (_structureType == typeof(JamMirrorCraftingOrderData_C))
            {
                if (_create)
                {
                    moveFieldToEnd("m_reagents");
                    moveFieldToEnd("m_customerNotes");
                    moveFieldToEnd("m_customer");
                    moveFieldToEnd("m_npcCustomer");
                    moveFieldToEnd("m_outputItem");
                    moveFieldToEnd("m_outputItemData");

                    FinishBitPack("FinishBitPack_afterOptionalBit");
                    moveFieldBeforeField("FinishBitPack_afterOptionalBit", false, "m_reagents", false);
                }
            }
            else if (_structureType == typeof(JamMirrorCraftingOrderItem_C))
            {
                if (_create)
                    moveFieldBeforeField("m_dataSlotIndex.has_value()", false, "m_dataSlotIndex", false);

                FinishBitPack("FinishBitPack_afterOptionalBit");
                moveFieldBeforeField("FinishBitPack_afterOptionalBit", false, "m_dataSlotIndex", false);
            }
            else if (_structureType == typeof(JamMirrorStablePetInfo_C))
            {
                if (!_create)
                {
                    moveFieldBeforeField("m_petFlags", false, "m_name{0}size()", false);
                    moveFieldBeforeField("m_specialization", false, "m_name{0}size()", false);
                }
            }
            else if (_structureType == typeof(JamMirrorBankTabSettings_C))
            {
                moveFieldBeforeField("m_depositFlags", false, "m_name", false);
                if (!_create)
                {
                    FinishControlBlocks(null, "FinishControlBlocks_after_sizes");
                    moveFieldBeforeField("FinishControlBlocks_after_sizes", false, "m_depositFlags", false);
                    moveFieldBeforeField("WriteUpdate_FinishBitPack_after_DynamicField_sizes", false, "m_depositFlags", false);
                }
            }
            else if (_structureType == typeof(JamMirrorWalkInData_C))
            {
                if (!_create)
                    moveFieldBeforeField("Field_18", false, "m_type", false);
            }
            else if (_structureType == typeof(JamMirrorDelveData_C))
            {
                if (!_create)
                {
                    moveFieldBeforeField("m_owners{0}size()", false, "m_started", false);
                    moveFieldBeforeField("m_owners", false, "m_started", false);
                }
            }
            else if (_structureType == typeof(CGGameObjectData))
            {
                if (_create)
                    moveFieldToEnd("m_assistActionData");

                FinishBitPack("gameobject_optionals");
                moveFieldBeforeField("gameobject_optionals", false, "m_assistActionData", false);
            }
            else if (_structureType == typeof(JamMirrorGameObjectAssistActionData_C))
            {
                moveFieldBeforeField("m_monsterName{0}size()", false, "m_virtualRealmAddress", false);
                moveFieldBeforeField("m_playerName{0}size()", false, "m_monsterName{0}size()", false);

                moveFieldToEnd("m_playerName");
                moveFieldToEnd("m_monsterName");
            }
            else if (_structureType == typeof(JamMirrorAreaTriggerSplineCalculator_C))
            {
                if (_create)
                    moveFieldBeforeField("m_catmullrom", false, "m_points", false);
            }
            else if (_structureType == typeof(CGAreaTriggerData))
            {
                if (_create)
                {
                    FinishBitPack("FinishBitPack_beforeOptionalBits");

                    moveFieldBeforeField("m_overrideScaleCurve", false, "m_caster", false);
                    moveFieldBeforeField("m_targetRollPitchYaw.has_value()", false, "m_overrideMoveCurveX", false);
                    moveFieldBeforeField("m_forcedPositionAndRotation.has_value()", false, "m_overrideMoveCurveX", false);
                    moveFieldBeforeField("FinishBitPack_beforeOptionalBits", false, "m_targetRollPitchYaw.has_value()", false);
                    moveFieldBeforeField("m_targetRollPitchYaw", false, "m_overrideMoveCurveY", false);
                    moveFieldBeforeField("m_forcedPositionAndRotation", false, "m_overrideMoveCurveY", false);
                }
                else
                {
                    moveFieldToEnd("m_extraScaleCurve");
                    moveFieldToEnd("m_targetRollPitchYaw.has_value()");
                    moveFieldToEnd("m_forcedPositionAndRotation.has_value()");
                    moveFieldToEnd("m_overrideMoveCurveX");
                    moveFieldToEnd("m_targetRollPitchYaw");
                    moveFieldToEnd("m_forcedPositionAndRotation");
                    moveFieldToEnd("m_overrideMoveCurveY");
                    moveFieldToEnd("m_overrideMoveCurveZ");
                    moveFieldToEnd("m_visualAnim");
                    moveFieldToEnd("m_spline");
                    moveFieldToEnd("m_orbit");

                    FinishBitPack("FinishBitPack_beforeOptionalBits");
                    FinishControlBlocks(null, "FinishControlBlocks_beforeOptionalBits");
                    moveFieldBeforeField("FinishControlBlocks_beforeOptionalBits", false, "m_targetRollPitchYaw.has_value()", false);
                    moveFieldBeforeField("FinishBitPack_beforeOptionalBits", false, "m_targetRollPitchYaw.has_value()", false);

                    FinishBitPack("FinishBitPack_afterOptionalBits");
                    FinishControlBlocks(null, "FinishControlBlocks_afterOptionalBits");
                    moveFieldBeforeField("FinishControlBlocks_afterOptionalBits", false, "m_overrideMoveCurveX", false);
                    moveFieldBeforeField("FinishBitPack_afterOptionalBits", false, "m_overrideMoveCurveX", false);
                }
            }
            else if (_structureType == typeof(CGConversationData))
            {
                if (_create)
                {
                    var dontPlayBroadcastTextSoundsIndex = _fieldWrites.FindIndex(fw => fw.Name == RenameField("m_dontPlayBroadcastTextSounds"));
                    var actorsSizeIndex = _fieldWrites.FindIndex(fw => fw.Name == RenameField("m_actors") && fw.IsSize);
                    if (actorsSizeIndex != -1)
                    {
                        // move to just-before-end (end is a write for closing all brackets)
                        var dontPlayBroadcastTextSounds = _fieldWrites[dontPlayBroadcastTextSoundsIndex];
                        _fieldWrites.RemoveAt(dontPlayBroadcastTextSoundsIndex);
                        _fieldWrites.Insert(actorsSizeIndex, dontPlayBroadcastTextSounds);
                    }
                }
            }
        }

        protected void RegisterDynamicChangesMaskFieldType(Type fieldType)
        {
            if (_dynamicChangesMaskTypes.Contains(fieldType.Name))
                return;

            _dynamicChangesMaskTypes.Add(fieldType.Name);
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

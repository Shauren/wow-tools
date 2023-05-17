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
            _bitCounter = HasNonArrayFields(structureType) && CountFields(structureType, field => true) > 1 && _blockGroupSize > 0 ? 0 : -1;
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

        public abstract void FinishControlBlocks(IReadOnlyList<FlowControlBlock> previousControlFlow);

        public abstract void FinishBitPack();

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

        protected virtual string RenameType(Type type)
        {
            return type.Name;
        }

        protected abstract string RenameField(string name);

        protected void PostProcessFieldWrites()
        {
            Action<string, string, bool> moveFieldBeforeField = (fieldToMove, where, whereIsSize) =>
            {
                fieldToMove = RenameField(fieldToMove);
                where = RenameField(where);
                var movedFieldIndex = _fieldWrites.FindIndex(fieldWrite => fieldWrite.Name == fieldToMove && !fieldWrite.IsSize);
                var whereFieldIndex = _fieldWrites.FindIndex(fieldWrite => fieldWrite.Name == where && fieldWrite.IsSize == whereIsSize);
                if (movedFieldIndex != -1 && whereFieldIndex != -1)
                {
                    // move to just-before-last field
                    var movedField = _fieldWrites[movedFieldIndex];
                    _fieldWrites.RemoveAt(movedFieldIndex);
                    _fieldWrites.Insert(whereFieldIndex < movedFieldIndex ? whereFieldIndex : whereFieldIndex - 1, movedField);
                }
            };

            Action<string> moveFieldToEnd = (fieldToMove) =>
            {
                fieldToMove = RenameField(fieldToMove);
                var movedFieldIndex = _fieldWrites.FindIndex(fieldWrite => fieldWrite.Name == fieldToMove && !fieldWrite.IsSize);
                if (movedFieldIndex != -1)
                {
                    // move to just-before-last field
                    var movedField = _fieldWrites[movedFieldIndex];
                    _fieldWrites.RemoveAt(movedFieldIndex);
                    _fieldWrites.Insert(_fieldWrites.Count - 1, movedField);
                }
            };

            if (_structureType == typeof(CGItemData))
            {
                if (!_create)
                    moveFieldBeforeField("m_modifiers", "m_spellCharges", false);
            }
            else if (_structureType == typeof(CGPlayerData))
            {
                if (_create)
                    moveFieldToEnd("dungeonScore");
            }
            else if (_structureType == typeof(CGActivePlayerData))
            {
                if (!_create)
                {
                    var spellFlatModByLabelIndex = _fieldWrites.FindIndex(fieldWrite => fieldWrite.Name == RenameField("spellFlatModByLabel") && fieldWrite.IsSize);
                    if (spellFlatModByLabelIndex != -1)
                    {
                        // move after research
                        var researchSizeIndex = _fieldWrites.FindIndex(fw => fw.Name == RenameField("research") && fw.IsSize);
                        if (researchSizeIndex != -1)
                        {
                            var researchSize = _fieldWrites[researchSizeIndex];
                            _fieldWrites.RemoveAt(researchSizeIndex);
                            _fieldWrites.Insert(spellFlatModByLabelIndex + 1, researchSize);
                        }
                    }
                }

                if (_create)
                {
                    moveFieldBeforeField("questSession.has_value()", "pvpInfo", false);
                    moveFieldBeforeField("questSession", "pvpInfo", false);
                }
                else
                {
                    var questSessionBitIndex = _fieldWrites.FindIndex(fw => fw.Name == RenameField("questSession.has_value()"));
                    if (questSessionBitIndex != -1)
                    {
                        var newQuestSessionPos = _fieldWrites.FindIndex(fw => !fw.IsSize && fw.Name == RenameField("invSlots"));
                        if (newQuestSessionPos != -1)
                        {
                            var movedCount = 3;

                            var movedItems = _fieldWrites.GetRange(questSessionBitIndex, movedCount);
                            _fieldWrites.RemoveRange(questSessionBitIndex, movedCount);

                            FinishControlBlocks(null);

                            movedItems.Insert(0, _fieldWrites[_fieldWrites.Count - 1]);

                            _fieldWrites.RemoveRange(_fieldWrites.Count - 1, 1);

                            _fieldWrites.InsertRange(newQuestSessionPos - movedCount, movedItems);
                        }
                    }
                }

                var researchDataIndex = _fieldWrites.FindIndex(fieldWrite => fieldWrite.Name == RenameField("research") && !fieldWrite.IsSize);
                if (researchDataIndex != -1)
                {
                    var researchSizeIndex = _fieldWrites.FindIndex(fieldWrite => fieldWrite.Name == RenameField("research") && fieldWrite.IsSize);
                    if (researchSizeIndex != -1)
                    {
                        var researchData = _fieldWrites[researchDataIndex];
                        _fieldWrites.RemoveAt(researchDataIndex);
                        _fieldWrites.Insert(researchSizeIndex + 1, researchData);

                        if (!_create)
                        {
                            FinishControlBlocks(null);
                            FinishBitPack();

                            var finishBitPack = _fieldWrites.GetRange(_fieldWrites.Count - 2, 2);
                            _fieldWrites.RemoveRange(_fieldWrites.Count - 2, 2);
                            _fieldWrites.InsertRange(researchSizeIndex + 2, finishBitPack);
                        }
                    }
                }

                moveFieldBeforeField("frozenPerksVendorItem", "questSession", false);
                if (_create)
                    moveFieldBeforeField("dungeonScore", "pvpInfo", false);
                else
                    moveFieldBeforeField("dungeonScore", "invSlots", false);

                moveFieldBeforeField("field_1410", "dungeonScore", false);
            }
            else if (_structureType == typeof(JamMirrorTraitConfig_C))
            {
                moveFieldToEnd("m_name");
                moveFieldBeforeField("m_name{0}size()", "m_name", false);
            }
            else if (_structureType == typeof(JamMirrorCraftingOrder_C))
            {
                if (_create)
                {
                    moveFieldBeforeField("m_data", "m_recraftItemInfo", false);
                    moveFieldBeforeField("m_recraftItemInfo", "m_enchantments", false);
                    moveFieldBeforeField("m_recraftItemInfo.has_value()", "m_enchantments", true);
                }
            }
            else if (_structureType == typeof(JamMirrorCraftingOrderData_C))
            {
                if (_create)
                {
                    moveFieldToEnd("m_outputItem");
                    moveFieldToEnd("m_outputItemData");
                    moveFieldBeforeField("m_reagents", "m_customerNotes", false);
                    moveFieldBeforeField("m_outputItem.has_value()", "m_reagents", false);
                    moveFieldBeforeField("m_outputItemData.has_value()", "m_reagents", false);
                }
            }
            else if (_structureType == typeof(JamMirrorCraftingOrderItem_C))
            {
                if (_create)
                    moveFieldBeforeField("m_dataSlotIndex.has_value()", "m_dataSlotIndex", false);
            }
            else if (_structureType == typeof(CGAreaTriggerData))
            {
                if (_create)
                {
                    var overrideScaleCurveIndex = _fieldWrites.FindIndex(fieldWrite =>
                    {
                        return fieldWrite.Name == RenameField("m_overrideScaleCurve") && !fieldWrite.IsSize;
                    });
                    if (overrideScaleCurveIndex != -1)
                    {
                        // move to start
                        var overrideScaleCurve = _fieldWrites[overrideScaleCurveIndex];
                        _fieldWrites.RemoveAt(overrideScaleCurveIndex);
                        _fieldWrites.Insert(0, overrideScaleCurve);
                    }
                    moveFieldBeforeField("m_field_260", "m_field_C38", false);
                    moveFieldBeforeField("m_field_261", "m_field_C38", false);
                }
                else
                {
                    moveFieldBeforeField("m_extraScaleCurve", "m_visualAnim", false);
                    moveFieldBeforeField("m_field_C38", "m_visualAnim", false);
                    moveFieldBeforeField("m_field_C54", "m_visualAnim", false);
                    moveFieldBeforeField("m_field_C70", "m_visualAnim", false);
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
            if (fieldType.GetCustomAttribute<HasDynamicChangesMaskAttribute>() == null)
                return;

            if (_dynamicChangesMaskTypes.Contains(fieldType.Name))
                return;

            _dynamicChangesMaskTypes.Add(fieldType.Name);
        }

        protected void WriteAutogeneratedDisclaimer(TextWriter output)
        {
            output.WriteLine("// This file is automatically generated, DO NOT EDIT");
            output.WriteLine();
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

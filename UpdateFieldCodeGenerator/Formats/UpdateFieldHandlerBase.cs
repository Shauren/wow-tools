using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        protected int _bitCounter;
        protected int _blockGroupBit;
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
            _bitCounter = HasNonArrayFields(structureType) ? 0 : -1;
            _blockGroupBit = 0;
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
            return type.GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(field => typeof(UpdateField).IsAssignableFrom(field.FieldType))
                .Select(field => field.GetValue(null) as UpdateField)
                .Where(field => !field.Type.IsArray)
                .Count() > 0;
        }

        protected virtual string RenameType(Type type)
        {
            return type.Name;
        }

        protected virtual string RenameField(string name)
        {
            return name;
        }

        protected void PostProcessFieldWrites()
        {
            if (_structureType == typeof(CGActivePlayerData))
            {
                if (!_create)
                {
                    var name = RenameField("pvpInfo");
                    var pvpInfoIndex = _fieldWrites.FindIndex(fieldWrite =>
                    {
                        return fieldWrite.Name == name;
                    });
                    if (pvpInfoIndex != -1)
                    {
                        // move to just-before-end (end is a write for closing all brackets)
                        var pvpInfo = _fieldWrites[pvpInfoIndex];
                        _fieldWrites.RemoveAt(pvpInfoIndex);
                        _fieldWrites.Insert(_fieldWrites.Count - 1, pvpInfo);
                    }
                }
            }
            else if (_structureType == typeof(CGAreaTriggerData))
            {
                var name = RenameField("m_extraScaleCurve");
                var extraScaleCurveIndex = _fieldWrites.FindIndex(fieldWrite =>
                {
                    return fieldWrite.Name == name && !fieldWrite.IsSize;
                });
                if (extraScaleCurveIndex != -1)
                {
                    // move to just-before-end (end is a write for closing all brackets)
                    var extraScaleCurve = _fieldWrites[extraScaleCurveIndex];
                    _fieldWrites.RemoveAt(extraScaleCurveIndex);
                    _fieldWrites.Insert(_fieldWrites.Count - 1, extraScaleCurve);
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

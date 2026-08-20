namespace UpdateFieldCodeGenerator.Formats
{
    public class UpdateFieldHandlers : IDisposable
    {
        private readonly IList<IUpdateFieldHandler> _handlers;
        private readonly Dictionary<IUpdateFieldHandler, IReadOnlyList<FlowControlBlock>> _previousControlFlowDict;

        public UpdateFieldHandlers()
        {
            _handlers = new List<IUpdateFieldHandler>()
            {
                new TrinityCoreHandler(),
                new WowPacketParserHandler()
            };
            _previousControlFlowDict = new Dictionary<IUpdateFieldHandler, IReadOnlyList<FlowControlBlock>>();
            foreach (var handler in _handlers)
                _previousControlFlowDict[handler] = null;
        }

        public IUpdateFieldHandler TrinityCore => _handlers[0];
        public IUpdateFieldHandler WowPacketParser => _handlers[1];

        public void BeforeStructures()
        {
            foreach (var handler in _handlers)
                handler.BeforeStructures();
        }

        public void AfterStructures()
        {
            foreach (var handler in _handlers)
                handler.AfterStructures();
        }

        public void OnStructureBegin(Type structureType, ObjectType objectType, bool create, bool writeUpdateMasks)
        {
            foreach (var handler in _handlers)
                handler.OnStructureBegin(structureType, objectType, create, writeUpdateMasks);
        }

        public void OnStructureEnd(bool forceMaskMask)
        {
            FinishControlBlocks("OnStructureEnd");
            foreach (var handler in _handlers)
                handler.OnStructureEnd(forceMaskMask);
        }

        public void OnField(string name, UpdateField updateField)
        {
            foreach (var handler in _handlers)
                _previousControlFlowDict[handler] = handler.OnField(name, updateField, _previousControlFlowDict[handler]);
        }

        public void OnDynamicFieldSizeCreate(string name, UpdateField updateField)
        {
            foreach (var handler in _handlers)
                _previousControlFlowDict[handler] = handler.OnDynamicFieldSizeCreate(name, updateField, _previousControlFlowDict[handler]);
        }

        public void OnDynamicFieldSizeUpdate(string name, UpdateField updateField)
        {
            foreach (var handler in _handlers)
                _previousControlFlowDict[handler] = handler.OnDynamicFieldSizeUpdate(name, updateField, _previousControlFlowDict[handler]);
        }

        public void OnDynamicArrayFieldSizeUpdate(string name, UpdateField updateField)
        {
            foreach (var handler in _handlers)
                handler.OnDynamicFieldSizeUpdate(name, updateField, null);
        }

        public void OnOptionalFieldInitCreate(string name, UpdateField updateField)
        {
            foreach (var handler in _handlers)
                _previousControlFlowDict[handler] = handler.OnOptionalFieldInitCreate(name, updateField, _previousControlFlowDict[handler]);
        }

        public void OnOptionalFieldInitUpdate(string name, UpdateField updateField)
        {
            foreach (var handler in _handlers)
                _previousControlFlowDict[handler] = handler.OnOptionalFieldInitUpdate(name, updateField, _previousControlFlowDict[handler]);
        }

        public void FinishControlBlocks(string tag)
        {
            foreach (var handler in _handlers)
                FinishControlBlocksIn(handler, tag);
        }

        public void FinishControlBlocksIn(IUpdateFieldHandler handler, string tag)
        {
            handler.FinishControlBlocks(_previousControlFlowDict[handler], tag);
            _previousControlFlowDict[handler] = null;
        }

        public void FinishBitPack(string tag)
        {
            foreach (var handler in _handlers)
                FinishBitPackIn(handler, tag);
        }

        public void FinishBitPackIn(IUpdateFieldHandler handler, string tag)
        {
            handler.FinishBitPack(tag);
        }

        public void Dispose()
        {
            foreach (var handler in _handlers)
                handler.Dispose();

            _handlers.Clear();
        }
    }
}

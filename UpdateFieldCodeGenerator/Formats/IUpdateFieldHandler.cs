namespace UpdateFieldCodeGenerator.Formats
{
    public interface IUpdateFieldHandler : IDisposable
    {
        void BeforeStructures();
        void AfterStructures();

        void OnStructureBegin(Type structureType, ObjectType objectType, bool create, bool writeUpdateMasks);
        void OnStructureEnd(bool needsFlush, bool forceMaskMask);

        IReadOnlyList<FlowControlBlock> OnField(string name, UpdateField updateField, IReadOnlyList<FlowControlBlock> previousControlFlow);
        IReadOnlyList<FlowControlBlock> OnDynamicFieldSizeCreate(string name, UpdateField updateField, IReadOnlyList<FlowControlBlock> previousControlFlow);
        IReadOnlyList<FlowControlBlock> OnDynamicFieldSizeUpdate(string name, UpdateField updateField, IReadOnlyList<FlowControlBlock> previousControlFlow);
        IReadOnlyList<FlowControlBlock> OnOptionalFieldInitCreate(string name, UpdateField updateField, IReadOnlyList<FlowControlBlock> previousControlFlow);
        IReadOnlyList<FlowControlBlock> OnOptionalFieldInitUpdate(string name, UpdateField updateField, IReadOnlyList<FlowControlBlock> previousControlFlow);

        void FinishControlBlocks(IReadOnlyList<FlowControlBlock> previousControlFlow);

        void FinishBitPack();
    }
}

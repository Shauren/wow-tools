using System;
using System.Collections.Generic;

namespace UpdateFieldCodeGenerator.Formats
{
    public interface IUpdateFieldHandler : IDisposable
    {
        void BeforeStructures();
        void AfterStructures();

        void OnStructureBegin(Type structureType, ObjectType objectType, bool create, bool writeUpdateMasks);
        void OnStructureEnd(bool needsFlush, bool forceMaskMask);

        IReadOnlyList<IStatement> OnField(string name, UpdateField updateField, IReadOnlyList<IStatement> previousControlFlow);
        IReadOnlyList<IStatement> OnDynamicFieldSizeCreate(string name, UpdateField updateField, IReadOnlyList<IStatement> previousControlFlow);
        IReadOnlyList<IStatement> OnDynamicFieldSizeUpdate(string name, UpdateField updateField, IReadOnlyList<IStatement> previousControlFlow);
        IReadOnlyList<IStatement> OnOptionalFieldInitCreate(string name, UpdateField updateField, IReadOnlyList<IStatement> previousControlFlow);
        IReadOnlyList<IStatement> OnOptionalFieldInitUpdate(string name, UpdateField updateField, IReadOnlyList<IStatement> previousControlFlow);

        void FinishControlBlocks(IReadOnlyList<IStatement> previousControlFlow);

        void FinishBitPack();
    }
}

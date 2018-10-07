
#include "StructureUpdateFieldDumper.h"

void StructureUpdateFieldDumper::Dump()
{
    Structure TSDynamicMirrorDataArray_vtable;
    TSDynamicMirrorDataArray_vtable.SetName("TSDynamicMirrorDataArray_vtable");
    TSDynamicMirrorDataArray_vtable.AddMember(Structure::Member(0, "_DWORD (__thiscall* GrowToFitRaw)(TSDynamicMirrorDataArray*,unsigned int,int)", "", ""));
    TSDynamicMirrorDataArray_vtable.AddMember(Structure::Member(1, "_DWORD (__thiscall* CountRaw)(TSDynamicMirrorDataArray*)", "", ""));
    TSDynamicMirrorDataArray_vtable.AddMember(Structure::Member(2, "_DWORD (__thiscall* Count)(TSDynamicMirrorDataArray*)", "", ""));
    TSDynamicMirrorDataArray_vtable.AddMember(Structure::Member(3, "_DWORD (__thiscall* SetCountRaw)(TSDynamicMirrorDataArray*,unsigned int)", "", ""));
    TSDynamicMirrorDataArray_vtable.AddMember(Structure::Member(4, "_DWORD (__thiscall* func_10)(TSDynamicMirrorDataArray*,unsigned int,unsigned int const*)", "", ""));
    TSDynamicMirrorDataArray_vtable.AddMember(Structure::Member(5, "_DWORD (__thiscall* SetRaw)(TSDynamicMirrorDataArray*,unsigned int,unsigned int)", "", ""));
    TSDynamicMirrorDataArray_vtable.AddMember(Structure::Member(6, "_DWORD (__thiscall* AddRaw)(TSDynamicMirrorDataArray*,unsigned int,unsigned int const*)", "", ""));
    TSDynamicMirrorDataArray_vtable.AddMember(Structure::Member(7, "_DWORD (__thiscall* GetRaw)(TSDynamicMirrorDataArray*,unsigned int)", "", ""));
    TSDynamicMirrorDataArray_vtable.AddMember(Structure::Member(8, "_DWORD* (__thiscall* GetRawPtr)(TSDynamicMirrorDataArray*,unsigned int)", "", ""));
    TSDynamicMirrorDataArray_vtable.AddMember(Structure::Member(9, "_DWORD (__thiscall* ConvertRawIndexToRealIndex)(TSDynamicMirrorDataArray*,unsigned int)", "", ""));
    TSDynamicMirrorDataArray_vtable.AddMember(Structure::Member(10, "_DWORD (__thiscall* Bytes)(TSDynamicMirrorDataArray*)", "", ""));
    TSDynamicMirrorDataArray_vtable.AddMember(Structure::Member(11, "_DWORD* (__thiscall* Ptr)(TSDynamicMirrorDataArray*)", "", ""));

    Structure TSDynamicMirrorDataArray;
    TSDynamicMirrorDataArray.SetName("TSDynamicMirrorDataArray");
    TSDynamicMirrorDataArray.AddMember(Structure::Member(0, "TSDynamicMirrorDataArray_vtable*", "vtable", ""));
    TSDynamicMirrorDataArray.AddMember(Structure::Member(1, "_DWORD", "Capacity", ""));
    TSDynamicMirrorDataArray.AddMember(Structure::Member(2, "_DWORD", "Count", ""));
    TSDynamicMirrorDataArray.AddMember(Structure::Member(3, "_DWORD", "field_C", ""));
    TSDynamicMirrorDataArray.AddMember(Structure::Member(4, "_DWORD*", "Data", ""));

    BuildUpdateFields(ObjectFields, "CGObjectData", GetInputData()->ObjectFields, "", "");
    BuildDynamicUpdateFields(ObjectDynamicFields, "CGObjectDynamicData", std::vector<DynamicUpdateField>(), "", "");

    BuildUpdateFields(ItemFields, "CGItemData", GetInputData()->ItemFields, "", "");
    BuildDynamicUpdateFields(ItemDynamicFields, "CGItemDynamicData", GetInputData()->ItemDynamicFields, "", "");

    BuildUpdateFields(ContainerFields, "CGContainerData", GetInputData()->ContainerFields, "", "");
    BuildDynamicUpdateFields(ContainerDynamicFields, "CGContainerDynamicData", std::vector<DynamicUpdateField>(), "", "");

    BuildUpdateFields(AzeriteEmpoweredItemFields, "CGAzeriteEmpoweredItemData", GetInputData()->AzeriteEmpoweredItemFields, "", "");
    BuildDynamicUpdateFields(AzeriteEmpoweredItemDynamicFields, "CGAzeriteEmpoweredItemDynamicData", std::vector<DynamicUpdateField>(), "", "");

    BuildUpdateFields(AzeriteItemFields, "CGAzeriteItemData", GetInputData()->AzeriteItemFields, "", "");
    BuildDynamicUpdateFields(AzeriteItemDynamicFields, "CGAzeriteItemDynamicData", std::vector<DynamicUpdateField>(), "", "");

    BuildUpdateFields(UnitFields, "CGUnitData", GetInputData()->UnitFields, "", "");
    BuildDynamicUpdateFields(UnitDynamicFields, "CGUnitDynamicData", GetInputData()->UnitDynamicFields, "", "");

    BuildUpdateFields(PlayerFields, "CGPlayerData", GetInputData()->PlayerFields, "", "");
    BuildDynamicUpdateFields(PlayerDynamicFields, "CGPlayerDynamicData", GetInputData()->PlayerDynamicFields, "", "");

    BuildUpdateFields(ActivePlayerFields, "GActivePlayerData", GetInputData()->ActivePlayerFields, "", "");
    BuildDynamicUpdateFields(ActivePlayerDynamicFields, "CGActivePlayerDynamicData", GetInputData()->ActivePlayerDynamicFields, "", "");

    BuildUpdateFields(GameObjectFields, "CGGameObjectData", GetInputData()->GameObjectFields, "", "");
    BuildDynamicUpdateFields(GameObjectDynamicFields, "CGGameObjectDynamicData", GetInputData()->GameObjectDynamicFields, "", "");

    BuildUpdateFields(DynamicObjectFields, "CGDynamicObjectData", GetInputData()->DynamicObjectFields, "", "");
    BuildDynamicUpdateFields(DynamicObjectDynamicFields, "CGDynamicObjectDynamicData", std::vector<DynamicUpdateField>(), "", "");

    BuildUpdateFields(CorpseFields, "CGCorpseData", GetInputData()->CorpseFields, "", "");
    BuildDynamicUpdateFields(CorpseDynamicFields, "CGCorpseDynamicData", std::vector<DynamicUpdateField>(), "", "");

    BuildUpdateFields(AreaTriggerFields, "CGAreaTriggerData", GetInputData()->AreaTriggerFields, "", "");
    BuildDynamicUpdateFields(AreaTriggerDynamicFields, "CGAreaTriggerDynamicData", std::vector<DynamicUpdateField>(), "", "");

    BuildUpdateFields(SceneObjectFields, "CGSceneObjectData", GetInputData()->SceneObjectFields, "", "");
    BuildDynamicUpdateFields(SceneObjectDynamicFields, "CGSceneObjectDynamicData", std::vector<DynamicUpdateField>(), "", "");

    BuildUpdateFields(ConversationFields, "CGConversationData", GetInputData()->ConversationFields, "", "");
    BuildDynamicUpdateFields(ConversationDynamicFields, "CGConversationDynamicData", GetInputData()->ConversationDynamicFields, "", "");

    std::ofstream updateFieldsDump("FieldStructure.h");
    updateFieldsDump << "#pragma pack(push, 1)" << std::endl;
    updateFieldsDump << "struct TSDynamicMirrorDataArray;" << std::endl;
    updateFieldsDump << SourceOutput<Structure>(std::make_unique<CppStruct>(false), TSDynamicMirrorDataArray_vtable, 0) << std::endl;
    updateFieldsDump << SourceOutput<Structure>(std::make_unique<CppStruct>(false), TSDynamicMirrorDataArray, 0) << std::endl;
    DumpEnums(updateFieldsDump);
    updateFieldsDump << "#pragma pack(pop)" << std::endl;
    updateFieldsDump.close();
}

void StructureUpdateFieldDumper::DumpEnum(std::ofstream& file, Outputs const& enumData)
{
    if (enumData.S.GetMembers().empty())
        return;

    file << SourceOutput<Structure>(std::make_unique<CppStruct>(false), enumData.S, 0) << std::endl;
}

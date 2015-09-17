
#include "StructureUpdateFieldDumper.h"

void StructureUpdateFieldDumper::Dump()
{
    BuildUpdateFields(ObjectFields, "CGObjectData", GetInputData()->ObjectFields, "", "");
    BuildUpdateFields(ItemFields, "CGItemData", GetInputData()->ItemFields, "", "");
    BuildUpdateFields(ContainerFields, "CGContainerData", GetInputData()->ContainerFields, "", "");
    BuildUpdateFields(UnitFields, "CGUnitData", GetInputData()->UnitFields, "", "");
    BuildUpdateFields(PlayerFields, "CGPlayerData", GetInputData()->PlayerFields, "", "");
    BuildUpdateFields(GameObjectFields, "CGGameObjectData", GetInputData()->GameObjectFields, "", "");
    BuildUpdateFields(DynamicObjectFields, "CGDynamicObjectData", GetInputData()->DynamicObjectFields, "", "");
    BuildUpdateFields(CorpseFields, "CGCorpseData", GetInputData()->CorpseFields, "", "");
    BuildUpdateFields(AreaTriggerFields, "CGAreaTriggerData", GetInputData()->AreaTriggerFields, "", "");
    BuildUpdateFields(SceneObjectFields, "CGSceneObjectData", GetInputData()->SceneObjectFields, "", "");
    BuildUpdateFields(ConversationFields, "CGConversationData", GetInputData()->ConversationFields, "", "");

    std::ofstream updateFieldsDump("FieldStructure.h");
    updateFieldsDump << "#pragma pack(push, 1)" << std::endl;
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

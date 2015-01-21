
#include "CsUpdateFieldDumper.h"

void CsUpdateFieldDumper::Dump()
{
    BuildUpdateFieldEnum(ObjectFields, "ObjectField", GetInputData()->ObjectFields, "OBJECT_END", "");
    BuildDynamicUpdateFieldEnum(ObjectDynamicFields, "ObjectDynamicField", std::vector<DynamicUpdateField>(), "OBJECT_DYNAMIC_END", "");

    BuildUpdateFieldEnum(ItemFields, "ItemField", GetInputData()->ItemFields, "ITEM_END", "ObjectField.OBJECT_END");
    BuildDynamicUpdateFieldEnum(ItemDynamicFields, "ItemDynamicField", GetInputData()->ItemDynamicFields, "ITEM_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    BuildUpdateFieldEnum(ContainerFields, "ContainerField", GetInputData()->ContainerFields, "CONTAINER_END", "ItemField.ITEM_END");
    BuildDynamicUpdateFieldEnum(ContainerDynamicFields, "ContainerDynamicField", std::vector<DynamicUpdateField>(), "CONTAINER_DYNAMIC_END", "ItemDynamicField.ITEM_DYNAMIC_END");

    BuildUpdateFieldEnum(UnitFields, "UnitField", GetInputData()->UnitFields, "UNIT_END", "ObjectField.OBJECT_END");
    BuildDynamicUpdateFieldEnum(UnitDynamicFields, "UnitDynamicField", GetInputData()->UnitDynamicFields, "UNIT_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    BuildUpdateFieldEnum(PlayerFields, "PlayerField", GetInputData()->PlayerFields, "PLAYER_END", "UnitField.UNIT_END");
    BuildDynamicUpdateFieldEnum(PlayerDynamicFields, "PlayerDynamicField", GetInputData()->PlayerDynamicFields, "PLAYER_DYNAMIC_END", "UnitDynamicField.UNIT_DYNAMIC_END");

    BuildUpdateFieldEnum(GameObjectFields, "GameObjectField", GetInputData()->GameObjectFields, "GAMEOBJECT_END", "ObjectField.OBJECT_END");
    BuildDynamicUpdateFieldEnum(GameObjectDynamicFields, "GameObjectDynamicField", std::vector<DynamicUpdateField>(), "GAMEOBJECT_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    BuildUpdateFieldEnum(DynamicObjectFields, "DynamicObjectField", GetInputData()->DynamicObjectFields, "DYNAMICOBJECT_END", "ObjectField.OBJECT_END");
    BuildDynamicUpdateFieldEnum(DynamicObjectDynamicFields, "DynamicObjectDynamicField", std::vector<DynamicUpdateField>(), "DYNAMICOBJECT_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    BuildUpdateFieldEnum(CorpseFields, "CorpseField", GetInputData()->CorpseFields, "CORPSE_END", "ObjectField.OBJECT_END");
    BuildDynamicUpdateFieldEnum(CorpseDynamicFields, "CorpseDynamicField", std::vector<DynamicUpdateField>(), "CORPSE_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    BuildUpdateFieldEnum(AreaTriggerFields, "AreaTriggerField", GetInputData()->AreaTriggerFields, "AREATRIGGER_END", "ObjectField.OBJECT_END");
    BuildDynamicUpdateFieldEnum(AreaTriggerDynamicFields, "AreaTriggerDynamicField", std::vector<DynamicUpdateField>(), "AREATRIGGER_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    BuildUpdateFieldEnum(SceneObjectFields, "SceneObjectField", GetInputData()->SceneObjectFields, "SCENEOBJECT_END", "ObjectField.OBJECT_END");
    BuildDynamicUpdateFieldEnum(SceneObjectDynamicFields, "SceneObjectDynamicField", std::vector<DynamicUpdateField>(), "SCENEOBJECT_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    BuildUpdateFieldEnum(ConversationFields, "ConversationField", GetInputData()->ConversationFields, "CONVERSATION_END", "ObjectField.OBJECT_END");
    BuildDynamicUpdateFieldEnum(ConversationDynamicFields, "ConversationDynamicField", GetInputData()->ConversationDynamicFields, "CONVERSATION_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    std::ofstream updateFieldsDump("UpdateFields.cs");

    updateFieldsDump << "WowPacketParserModule.V" << FormatVersion("_") << ".Enums" << std::endl;
    updateFieldsDump << "{" << std::endl;
    updateFieldsDump << Tab << "// ReSharper disable InconsistentNaming" << std::endl;
    updateFieldsDump << Tab << "// " << FormatVersion(".") << std::endl;

    DumpEnums(updateFieldsDump);

    updateFieldsDump << Tab << "// ReSharper restore InconsistentNaming" << std::endl;
    updateFieldsDump << "}" << std::endl;

    updateFieldsDump.close();
}

void CsUpdateFieldDumper::DumpEnum(std::ofstream& file, Enum const& enumData)
{
    if (enumData.GetMembers().empty())
        return;

    file << SourceOutput<Enum>(std::make_unique<CsEnum>(), enumData, 4);
}

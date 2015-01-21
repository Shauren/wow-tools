
#include "CsUpdateFieldDumper.h"

void CsUpdateFieldDumper::Dump()
{
    BuildUpdateFieldEnum(ObjectFields, "ObjectField", GetInputData()->ObjectFields, OBJECT_COUNT, "OBJECT_END", "");
    BuildDynamicUpdateFieldEnum(ObjectDynamicFields, "ObjectDynamicField", nullptr, OBJECT_DYNAMIC_COUNT, "OBJECT_DYNAMIC_END", "");

    BuildUpdateFieldEnum(ItemFields, "ItemField", GetInputData()->ItemFields, ITEM_COUNT, "ITEM_END", "ObjectField.OBJECT_END");
    BuildDynamicUpdateFieldEnum(ItemDynamicFields, "ItemDynamicField", GetInputData()->ItemDynamicFields, ITEM_DYNAMIC_COUNT, "ITEM_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    BuildUpdateFieldEnum(ContainerFields, "ContainerField", GetInputData()->ContainerFields, CONTAINER_COUNT, "CONTAINER_END", "ItemField.ITEM_END");
    BuildDynamicUpdateFieldEnum(ContainerDynamicFields, "ContainerDynamicField", nullptr, CONTAINER_DYNAMIC_COUNT, "CONTAINER_DYNAMIC_END", "ItemDynamicField.ITEM_DYNAMIC_END");

    BuildUpdateFieldEnum(UnitFields, "UnitField", GetInputData()->UnitFields, UNIT_COUNT, "UNIT_END", "ObjectField.OBJECT_END");
    BuildDynamicUpdateFieldEnum(UnitDynamicFields, "UnitDynamicField", GetInputData()->UnitDynamicFields, UNIT_DYNAMIC_COUNT, "UNIT_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    BuildUpdateFieldEnum(PlayerFields, "PlayerField", GetInputData()->PlayerFields, PLAYER_COUNT, "PLAYER_END", "UnitField.UNIT_END");
    BuildDynamicUpdateFieldEnum(PlayerDynamicFields, "PlayerDynamicField", GetInputData()->PlayerDynamicFields, PLAYER_DYNAMIC_COUNT, "PLAYER_DYNAMIC_END", "UnitDynamicField.UNIT_DYNAMIC_END");

    BuildUpdateFieldEnum(GameObjectFields, "GameObjectField", GetInputData()->GameObjectFields, GAMEOBJECT_COUNT, "GAMEOBJECT_END", "ObjectField.OBJECT_END");
    BuildDynamicUpdateFieldEnum(GameObjectDynamicFields, "GameObjectDynamicField", nullptr, GAMEOBJECT_DYNAMIC_COUNT, "GAMEOBJECT_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    BuildUpdateFieldEnum(DynamicObjectFields, "DynamicObjectField", GetInputData()->DynamicObjectFields, DYNAMICOBJECT_COUNT, "DYNAMICOBJECT_END", "ObjectField.OBJECT_END");
    BuildDynamicUpdateFieldEnum(DynamicObjectDynamicFields, "DynamicObjectDynamicField", nullptr, DYNAMICOBJECT_DYNAMIC_COUNT, "DYNAMICOBJECT_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    BuildUpdateFieldEnum(CorpseFields, "CorpseField", GetInputData()->CorpseFields, CORPSE_COUNT, "CORPSE_END", "ObjectField.OBJECT_END");
    BuildDynamicUpdateFieldEnum(CorpseDynamicFields, "CorpseDynamicField", nullptr, CORPSE_DYNAMIC_COUNT, "CORPSE_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    BuildUpdateFieldEnum(AreaTriggerFields, "AreaTriggerField", GetInputData()->AreaTriggerFields, AREATRIGGER_COUNT, "AREATRIGGER_END", "ObjectField.OBJECT_END");
    BuildDynamicUpdateFieldEnum(AreaTriggerDynamicFields, "AreaTriggerDynamicField", nullptr, AREATRIGGER_DYNAMIC_COUNT, "AREATRIGGER_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    BuildUpdateFieldEnum(SceneObjectFields, "SceneObjectField", GetInputData()->SceneObjectFields, SCENEOBJECT_COUNT, "SCENEOBJECT_END", "ObjectField.OBJECT_END");
    BuildDynamicUpdateFieldEnum(SceneObjectDynamicFields, "SceneObjectDynamicField", nullptr, SCENEOBJECT_DYNAMIC_COUNT, "SCENEOBJECT_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    BuildUpdateFieldEnum(ConversationFields, "ConversationField", GetInputData()->ContainerFields, CONVERSATION_COUNT, "CONVERSATION_END", "ObjectField.OBJECT_END");
    BuildDynamicUpdateFieldEnum(ConversationDynamicFields, "ConversationDynamicField", GetInputData()->ConversationDynamicFields, CONVERSATION_DYNAMIC_COUNT, "CONVERSATION_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

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

    file << StructureOutput<Enum>(std::make_unique<CsEnum>(), enumData, 4);
}

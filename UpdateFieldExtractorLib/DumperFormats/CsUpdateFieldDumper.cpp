
#include "CsUpdateFieldDumper.h"

void CsUpdateFieldDumper::Dump()
{
    BuildUpdateFields(ObjectFields, "ObjectField", GetInputData()->ObjectFields, "OBJECT_END", "");
    BuildDynamicUpdateFields(ObjectDynamicFields, "ObjectDynamicField", std::vector<DynamicUpdateField>(), "OBJECT_DYNAMIC_END", "");

    BuildUpdateFields(ItemFields, "ItemField", GetInputData()->ItemFields, "ITEM_END", "ObjectField.OBJECT_END");
    BuildDynamicUpdateFields(ItemDynamicFields, "ItemDynamicField", GetInputData()->ItemDynamicFields, "ITEM_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    BuildUpdateFields(ContainerFields, "ContainerField", GetInputData()->ContainerFields, "CONTAINER_END", "ItemField.ITEM_END");
    BuildDynamicUpdateFields(ContainerDynamicFields, "ContainerDynamicField", std::vector<DynamicUpdateField>(), "CONTAINER_DYNAMIC_END", "ItemDynamicField.ITEM_DYNAMIC_END");

    BuildUpdateFields(AzeriteEmpoweredItemFields, "AzeriteEmpoweredItemField", GetInputData()->AzeriteEmpoweredItemFields, "AZERITE_EMPOWERED_ITEM_END", "ItemField.ITEM_END");
    BuildDynamicUpdateFields(AzeriteEmpoweredItemDynamicFields, "AzeriteEmpoweredItemDynamicField", std::vector<DynamicUpdateField>(), "AZERITE_EMPOWERED_ITEM_DYNAMIC_END", "ItemDynamicField.ITEM_DYNAMIC_END");

    BuildUpdateFields(AzeriteItemFields, "AzeriteItemField", GetInputData()->AzeriteItemFields, "AZERITE_ITEM_END", "ItemField.ITEM_END");
    BuildDynamicUpdateFields(AzeriteItemDynamicFields, "AzeriteItemDynamicField", std::vector<DynamicUpdateField>(), "AZERITE_ITEM_DYNAMIC_END", "ItemDynamicField.ITEM_DYNAMIC_END");

    BuildUpdateFields(UnitFields, "UnitField", GetInputData()->UnitFields, "UNIT_END", "ObjectField.OBJECT_END");
    BuildDynamicUpdateFields(UnitDynamicFields, "UnitDynamicField", GetInputData()->UnitDynamicFields, "UNIT_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    BuildUpdateFields(PlayerFields, "PlayerField", GetInputData()->PlayerFields, "PLAYER_END", "UnitField.UNIT_END");
    BuildDynamicUpdateFields(PlayerDynamicFields, "PlayerDynamicField", GetInputData()->PlayerDynamicFields, "PLAYER_DYNAMIC_END", "UnitDynamicField.UNIT_DYNAMIC_END");

    BuildUpdateFields(ActivePlayerFields, "ActivePlayerField", GetInputData()->ActivePlayerFields, "ACTIVE_PLAYER_END", "PlayerField.PLAYER_END");
    BuildDynamicUpdateFields(ActivePlayerDynamicFields, "ActivePlayerDynamicField", GetInputData()->ActivePlayerDynamicFields, "ACTIVE_PLAYER_DYNAMIC_END", "PlayerDynamicField.PLAYER_DYNAMIC_END");

    BuildUpdateFields(GameObjectFields, "GameObjectField", GetInputData()->GameObjectFields, "GAMEOBJECT_END", "ObjectField.OBJECT_END");
    BuildDynamicUpdateFields(GameObjectDynamicFields, "GameObjectDynamicField", GetInputData()->GameObjectDynamicFields, "GAMEOBJECT_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    BuildUpdateFields(DynamicObjectFields, "DynamicObjectField", GetInputData()->DynamicObjectFields, "DYNAMICOBJECT_END", "ObjectField.OBJECT_END");
    BuildDynamicUpdateFields(DynamicObjectDynamicFields, "DynamicObjectDynamicField", std::vector<DynamicUpdateField>(), "DYNAMICOBJECT_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    BuildUpdateFields(CorpseFields, "CorpseField", GetInputData()->CorpseFields, "CORPSE_END", "ObjectField.OBJECT_END");
    BuildDynamicUpdateFields(CorpseDynamicFields, "CorpseDynamicField", std::vector<DynamicUpdateField>(), "CORPSE_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    BuildUpdateFields(AreaTriggerFields, "AreaTriggerField", GetInputData()->AreaTriggerFields, "AREATRIGGER_END", "ObjectField.OBJECT_END");
    BuildDynamicUpdateFields(AreaTriggerDynamicFields, "AreaTriggerDynamicField", std::vector<DynamicUpdateField>(), "AREATRIGGER_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    BuildUpdateFields(SceneObjectFields, "SceneObjectField", GetInputData()->SceneObjectFields, "SCENEOBJECT_END", "ObjectField.OBJECT_END");
    BuildDynamicUpdateFields(SceneObjectDynamicFields, "SceneObjectDynamicField", std::vector<DynamicUpdateField>(), "SCENEOBJECT_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    BuildUpdateFields(ConversationFields, "ConversationField", GetInputData()->ConversationFields, "CONVERSATION_END", "ObjectField.OBJECT_END");
    BuildDynamicUpdateFields(ConversationDynamicFields, "ConversationDynamicField", GetInputData()->ConversationDynamicFields, "CONVERSATION_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    std::ofstream updateFieldsDump("UpdateFields.cs");

    updateFieldsDump << "namespace WowPacketParserModule.V" << FormatVersion("_") << ".Enums" << std::endl;
    updateFieldsDump << "{" << std::endl;
    updateFieldsDump << Tab << "// ReSharper disable InconsistentNaming" << std::endl;
    updateFieldsDump << Tab << "// " << FormatVersion(".") << std::endl;

    DumpEnums(updateFieldsDump);

    updateFieldsDump << Tab << "// ReSharper restore InconsistentNaming" << std::endl;
    updateFieldsDump << "}" << std::endl;

    updateFieldsDump.close();
}

void CsUpdateFieldDumper::DumpEnum(std::ofstream& file, Outputs const& enumData)
{
    if (enumData.E.GetMembers().empty())
        return;

    file << SourceOutput<Enum>(std::make_unique<CsEnum>(), enumData.E, 4) << std::endl;
}

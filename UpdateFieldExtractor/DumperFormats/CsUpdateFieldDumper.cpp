
#include "UpdateFieldNameMap.h"
#include "CsUpdateFieldDumper.h"


void CsUpdateFieldDumper::Dump()
{
    std::ofstream updateFieldsDump("UpdateFields.cs");

    updateFieldsDump << "WowPacketParserModule.V" << FormatVersion("_") << ".Enums" << std::endl;
    updateFieldsDump << "{" << std::endl;
    updateFieldsDump << Tab << "// ReSharper disable InconsistentNaming" << std::endl;
    updateFieldsDump << Tab << "// " << FormatVersion(".") << std::endl;

    DumpUpdateFields(updateFieldsDump, "ObjectField", GetInputData()->ObjectFields, OBJECT_COUNT, "OBJECT_END", "");
    DumpDynamicFields(updateFieldsDump, "ObjectDynamicField", nullptr, OBJECT_DYNAMIC_COUNT, "OBJECT_DYNAMIC_END", "");

    DumpUpdateFields(updateFieldsDump, "ItemField", GetInputData()->ItemFields, ITEM_COUNT, "ITEM_END", "ObjectField.OBJECT_END");
    DumpDynamicFields(updateFieldsDump, "ItemDynamicField", GetInputData()->ItemDynamicFields, ITEM_DYNAMIC_COUNT, "ITEM_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    DumpUpdateFields(updateFieldsDump, "ContainerField", GetInputData()->ContainerFields, CONTAINER_COUNT, "CONTAINER_END", "ItemField.ITEM_END");
    DumpDynamicFields(updateFieldsDump, "ContainerDynamicField", nullptr, CONTAINER_DYNAMIC_COUNT, "CONTAINER_DYNAMIC_END", "ItemDynamicField.ITEM_DYNAMIC_END");

    DumpUpdateFields(updateFieldsDump, "UnitField", GetInputData()->UnitFields, UNIT_COUNT, "UNIT_END", "ObjectField.OBJECT_END");
    DumpDynamicFields(updateFieldsDump, "UnitDynamicField", GetInputData()->UnitDynamicFields, UNIT_DYNAMIC_COUNT, "UNIT_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    DumpUpdateFields(updateFieldsDump, "PlayerField", GetInputData()->PlayerFields, PLAYER_COUNT, "PLAYER_END", "UnitField.UNIT_END");
    DumpDynamicFields(updateFieldsDump, "PlayerDynamicField", GetInputData()->PlayerDynamicFields, PLAYER_DYNAMIC_COUNT, "PLAYER_DYNAMIC_END", "UnitDynamicField.UNIT_DYNAMIC_END");

    DumpUpdateFields(updateFieldsDump, "GameObjectField", GetInputData()->GameObjectFields, GAMEOBJECT_COUNT, "GAMEOBJECT_END", "ObjectField.OBJECT_END");
    DumpDynamicFields(updateFieldsDump, "GameObjectDynamicField", nullptr, GAMEOBJECT_DYNAMIC_COUNT, "GAMEOBJECT_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    DumpUpdateFields(updateFieldsDump, "DynamicObjectField", GetInputData()->DynamicObjectFields, DYNAMICOBJECT_COUNT, "DYNAMICOBJECT_END", "ObjectField.OBJECT_END");
    DumpDynamicFields(updateFieldsDump, "DynamicObjectDynamicField", nullptr, DYNAMICOBJECT_DYNAMIC_COUNT, "DYNAMICOBJECT_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    DumpUpdateFields(updateFieldsDump, "CorpseField", GetInputData()->CorpseFields, CORPSE_COUNT, "CORPSE_END", "ObjectField.OBJECT_END");
    DumpDynamicFields(updateFieldsDump, "CorpseDynamicField", nullptr, CORPSE_DYNAMIC_COUNT, "CORPSE_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    DumpUpdateFields(updateFieldsDump, "AreaTriggerField", GetInputData()->AreaTriggerFields, AREATRIGGER_COUNT, "AREATRIGGER_END", "ObjectField.OBJECT_END");
    DumpDynamicFields(updateFieldsDump, "AreaTriggerDynamicField", nullptr, AREATRIGGER_DYNAMIC_COUNT, "AREATRIGGER_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    DumpUpdateFields(updateFieldsDump, "SceneObjectField", GetInputData()->SceneObjectFields, SCENEOBJECT_COUNT, "SCENEOBJECT_END", "ObjectField.OBJECT_END");
    DumpDynamicFields(updateFieldsDump, "SceneObjectDynamicField", nullptr, SCENEOBJECT_DYNAMIC_COUNT, "SCENEOBJECT_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    DumpUpdateFields(updateFieldsDump, "ConversationField", GetInputData()->ContainerFields, CONVERSATION_COUNT, "CONVERSATION_END", "ObjectField.OBJECT_END");
    DumpDynamicFields(updateFieldsDump, "ConversationDynamicField", GetInputData()->ConversationDynamicFields, CONVERSATION_DYNAMIC_COUNT, "CONVERSATION_DYNAMIC_END", "ObjectDynamicField.OBJECT_DYNAMIC_END");

    updateFieldsDump << Tab << "// ReSharper restore InconsistentNaming" << std::endl;
    updateFieldsDump << "}" << std::endl;

    updateFieldsDump.close();
}

void CsUpdateFieldDumper::DumpUpdateFields(std::ofstream& file, std::string const& name, UpdateField* data, UpdateFieldSizes count, std::string const& end, std::string const& fieldBase)
{
    file << Tab << "public enum " << name << std::endl;
    file << Tab << "{" << std::endl;

    std::int32_t i = 0;
    while (i < count)
    {
        UpdateField* field = &data[i];
        std::string name = GetInputData()->ReadProcessMemoryCString(data[i].NameAddress);
        if (name == "CGUnitData::npcFlags[UMNW0]")
        {
            name = "CGUnitData::npcFlags";
            field = &data[i + 1];
        }

        std::string oldName = GetOldName(name.c_str());
        if (!oldName.empty())
            name = oldName;

        std::string pad(PaddingSize - name.length(), ' ');
        file << Tab << Tab << name << pad << "= ";
        if (!fieldBase.empty())
            file << fieldBase << " + ";

        file << hex_number(i) << ',';
        file << " // Size: " << field->Size << ", Flags: " << GetUpdateFieldFlagName(field->Flags) << std::endl;
        i += field->Size;
    }

    file << Tab << Tab << end << std::string(PaddingSize - end.length(), ' ') << "= ";
    if (!fieldBase.empty())
        file << fieldBase << " + ";

    file << hex_number(i) << ',' << std::endl;

    file << Tab << "}" << std::endl << std::endl;
}

void CsUpdateFieldDumper::DumpDynamicFields(std::ofstream& file, std::string const& name, DynamicUpdateField* data, UpdateFieldSizes count, std::string const& end, std::string const& fieldBase)
{
    file << Tab << "public enum " << name << std::endl;
    file << Tab << "{" << std::endl;

    std::int32_t i = 0;
    while (i < count)
    {
        DynamicUpdateField* field = &data[i];
        std::string name = GetInputData()->ReadProcessMemoryCString(data[i].NameAddress);
        std::string oldName = GetOldName(name.c_str());
        if (!oldName.empty())
            name = oldName;

        std::string pad(PaddingSize - name.length(), ' ');
        file << Tab << Tab << name << pad << "= ";
        if (!fieldBase.empty())
            file << fieldBase << " + ";

        file << hex_number(i) << ',';
        file << " //  Flags: " << GetUpdateFieldFlagName(field->Flags) << std::endl;
        ++i;
    }

    file << Tab << Tab << end << std::string(PaddingSize - end.length(), ' ') << "= ";
    if (!fieldBase.empty())
        file << fieldBase << " + ";

    file << hex_number(i) << ',' << std::endl;
    file << Tab << "}" << std::endl << std::endl;
}

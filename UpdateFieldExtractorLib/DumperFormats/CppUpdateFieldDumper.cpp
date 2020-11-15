
#include "UpdateFieldNameMap.h"
#include "CppUpdateFieldDumper.h"

void CppUpdateFieldDumper::Dump()
{
    BuildUpdateFields(ObjectFields, "ObjectFields", GetInputData()->ObjectFields, "OBJECT_END", "");
    BuildDynamicUpdateFields(ObjectDynamicFields, "ObjectDynamicFields", std::vector<DynamicUpdateField>(), "OBJECT_DYNAMIC_END", "");

    BuildUpdateFields(ItemFields, "ItemFields", GetInputData()->ItemFields, "ITEM_END", "OBJECT_END");
    BuildDynamicUpdateFields(ItemDynamicFields, "ItemDynamicFields", GetInputData()->ItemDynamicFields, "ITEM_DYNAMIC_END", "OBJECT_DYNAMIC_END");

    BuildUpdateFields(ContainerFields, "ContainerFields", GetInputData()->ContainerFields, "CONTAINER_END", "ITEM_END");
    BuildDynamicUpdateFields(ContainerDynamicFields, "ContainerDynamicFields", std::vector<DynamicUpdateField>(), "CONTAINER_DYNAMIC_END", "ITEM_DYNAMIC_END");

    BuildUpdateFields(AzeriteEmpoweredItemFields, "AzeriteEmpoweredItemField", GetInputData()->AzeriteEmpoweredItemFields, "AZERITE_EMPOWERED_ITEM_END", "ITEM_END");
    BuildDynamicUpdateFields(AzeriteEmpoweredItemDynamicFields, "AzeriteEmpoweredItemDynamicField", std::vector<DynamicUpdateField>(), "AZERITE_EMPOWERED_ITEM_DYNAMIC_END", "ITEM_DYNAMIC_END");

    BuildUpdateFields(AzeriteItemFields, "AzeriteItemField", GetInputData()->AzeriteItemFields, "AZERITE_ITEM_END", "ITEM_END");
    BuildDynamicUpdateFields(AzeriteItemDynamicFields, "AzeriteItemDynamicField", std::vector<DynamicUpdateField>(), "AZERITE_ITEM_DYNAMIC_END", "ITEM_DYNAMIC_END");

    BuildUpdateFields(UnitFields, "UnitFields", GetInputData()->UnitFields, "UNIT_END", "OBJECT_END");
    BuildDynamicUpdateFields(UnitDynamicFields, "UnitDynamicFields", GetInputData()->UnitDynamicFields, "UNIT_DYNAMIC_END", "OBJECT_DYNAMIC_END");

    BuildUpdateFields(PlayerFields, "PlayerFields", GetInputData()->PlayerFields, "PLAYER_END", "UNIT_END");
    BuildDynamicUpdateFields(PlayerDynamicFields, "PlayerDynamicFields", GetInputData()->PlayerDynamicFields, "PLAYER_DYNAMIC_END", "UNIT_DYNAMIC_END");

    BuildUpdateFields(ActivePlayerFields, "ActivePlayerField", GetInputData()->ActivePlayerFields, "ACTIVE_PLAYER_END", "PLAYER_END");
    BuildDynamicUpdateFields(ActivePlayerDynamicFields, "ActivePlayerDynamicField", GetInputData()->ActivePlayerDynamicFields, "ACTIVE_PLAYER_DYNAMIC_END", "PLAYER_DYNAMIC_END");

    BuildUpdateFields(GameObjectFields, "GameObjectFields", GetInputData()->GameObjectFields, "GAMEOBJECT_END", "OBJECT_END");
    BuildDynamicUpdateFields(GameObjectDynamicFields, "GameObjectDynamicFields", GetInputData()->GameObjectDynamicFields, "GAMEOBJECT_DYNAMIC_END", "OBJECT_DYNAMIC_END");

    BuildUpdateFields(DynamicObjectFields, "DynamicObjectFields", GetInputData()->DynamicObjectFields, "DYNAMICOBJECT_END", "OBJECT_END");
    BuildDynamicUpdateFields(DynamicObjectDynamicFields, "DynamicObjectDynamicFields", std::vector<DynamicUpdateField>(), "DYNAMICOBJECT_DYNAMIC_END", "OBJECT_DYNAMIC_END");

    BuildUpdateFields(CorpseFields, "CorpseFields", GetInputData()->CorpseFields, "CORPSE_END", "OBJECT_END");
    BuildDynamicUpdateFields(CorpseDynamicFields, "CorpseDynamicFields", std::vector<DynamicUpdateField>(), "CORPSE_DYNAMIC_END", "OBJECT_DYNAMIC_END");

    BuildUpdateFields(AreaTriggerFields, "AreaTriggerFields", GetInputData()->AreaTriggerFields, "AREATRIGGER_END", "OBJECT_END");
    BuildDynamicUpdateFields(AreaTriggerDynamicFields, "AreaTriggerDynamicFields", std::vector<DynamicUpdateField>(), "AREATRIGGER_DYNAMIC_END", "OBJECT_DYNAMIC_END");

    BuildUpdateFields(SceneObjectFields, "SceneObjectFields", GetInputData()->SceneObjectFields, "SCENEOBJECT_END", "OBJECT_END");
    BuildDynamicUpdateFields(SceneObjectDynamicFields, "SceneObjectDynamicFields", std::vector<DynamicUpdateField>(), "SCENEOBJECT_DYNAMIC_END", "OBJECT_DYNAMIC_END");

    BuildUpdateFields(ConversationFields, "ConversationFields", GetInputData()->ConversationFields, "CONVERSATION_END", "OBJECT_END");
    BuildDynamicUpdateFields(ConversationDynamicFields, "ConversationDynamicFields", GetInputData()->ConversationDynamicFields, "CONVERSATION_DYNAMIC_END", "OBJECT_DYNAMIC_END");

    time_t now = time(nullptr);
    tm date;
    localtime_s(&date, &now);
    date.tm_year += 1900;

    std::ofstream updateFieldsDump("UpdateFields.h");

    updateFieldsDump << "/*" << std::endl;
    updateFieldsDump << " * Copyright (C) 2008-" << date.tm_year << " TrinityCore <http://www.trinitycore.org/>" << std::endl;
    updateFieldsDump << " * Copyright (C) 2005-2009 MaNGOS <http://getmangos.com/>" << std::endl;
    updateFieldsDump << " *" << std::endl;
    updateFieldsDump << " * This program is free software; you can redistribute it and/or modify it" << std::endl;
    updateFieldsDump << " * under the terms of the GNU General Public License as published by the" << std::endl;
    updateFieldsDump << " * Free Software Foundation; either version 2 of the License, or (at your" << std::endl;
    updateFieldsDump << " * option) any later version." << std::endl;
    updateFieldsDump << " *" << std::endl;
    updateFieldsDump << " * This program is distributed in the hope that it will be useful, but WITHOUT" << std::endl;
    updateFieldsDump << " * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or" << std::endl;
    updateFieldsDump << " * FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for" << std::endl;
    updateFieldsDump << " * more details." << std::endl;
    updateFieldsDump << " *" << std::endl;
    updateFieldsDump << " * You should have received a copy of the GNU General Public License along" << std::endl;
    updateFieldsDump << " * with this program. If not, see <http://www.gnu.org/licenses/>." << std::endl;
    updateFieldsDump << " */" << std::endl;
    updateFieldsDump << std::endl;
    updateFieldsDump << "#ifndef _UPDATEFIELDS_H" << std::endl;
    updateFieldsDump << "#define _UPDATEFIELDS_H" << std::endl;
    updateFieldsDump << std::endl;
    updateFieldsDump << "// Auto generated for version " << FormatVersion(", ") << std::endl;
    updateFieldsDump << std::endl;

    DumpEnums(updateFieldsDump);

    updateFieldsDump << "#endif // _UPDATEFIELDS_H" << std::endl;
    updateFieldsDump.close();

    std::ofstream updateFieldFlags("UpdateFieldFlags.cpp");

    updateFieldFlags << "/*" << std::endl;
    updateFieldFlags << " * Copyright (C) 2008-" << date.tm_year << " TrinityCore <http://www.trinitycore.org/>" << std::endl;
    updateFieldFlags << " *" << std::endl;
    updateFieldFlags << " * This program is free software; you can redistribute it and/or modify it" << std::endl;
    updateFieldFlags << " * under the terms of the GNU General Public License as published by the" << std::endl;
    updateFieldFlags << " * Free Software Foundation; either version 2 of the License, or (at your" << std::endl;
    updateFieldFlags << " * option) any later version." << std::endl;
    updateFieldFlags << " *" << std::endl;
    updateFieldFlags << " * This program is distributed in the hope that it will be useful, but WITHOUT" << std::endl;
    updateFieldFlags << " * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or" << std::endl;
    updateFieldFlags << " * FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for" << std::endl;
    updateFieldFlags << " * more details." << std::endl;
    updateFieldFlags << " *" << std::endl;
    updateFieldFlags << " * You should have received a copy of the GNU General Public License along" << std::endl;
    updateFieldFlags << " * with this program. If not, see <http://www.gnu.org/licenses/>." << std::endl;
    updateFieldFlags << " */" << std::endl;
    updateFieldFlags << std::endl;
    updateFieldFlags << "#include \"UpdateFieldFlags.h\"" << std::endl;
    updateFieldFlags << std::endl;

    DumpFlags(updateFieldFlags, "ContainerUpdateFieldFlags[CONTAINER_END]",
        { &GetInputData()->ObjectFields, &GetInputData()->ItemFields, &GetInputData()->ContainerFields });

    DumpFlags(updateFieldFlags, "AzeriteEmpoweredItemUpdateFieldFlags[AZERITE_EMPOWERED_ITEM_END]",
        { &GetInputData()->ObjectFields, &GetInputData()->ItemFields, &GetInputData()->AzeriteEmpoweredItemFields });

    DumpFlags(updateFieldFlags, "AzeriteItemUpdateFieldFlags[AZERITE_ITEM_END]",
        { &GetInputData()->ObjectFields, &GetInputData()->ItemFields, &GetInputData()->AzeriteItemFields });

    DumpDynamicFlags(updateFieldFlags, "ItemDynamicUpdateFieldFlags[ITEM_DYNAMIC_END]",
        { &GetInputData()->ItemDynamicFields });

    DumpFlags(updateFieldFlags, "UnitUpdateFieldFlags[ACTIVE_PLAYER_END]",
        { &GetInputData()->ObjectFields, &GetInputData()->UnitFields, &GetInputData()->PlayerFields, &GetInputData()->ActivePlayerFields });

    DumpDynamicFlags(updateFieldFlags, "UnitDynamicUpdateFieldFlags[ACTIVE_PLAYER_DYNAMIC_END]",
        { &GetInputData()->UnitDynamicFields, &GetInputData()->PlayerDynamicFields, &GetInputData()->ActivePlayerDynamicFields });

    DumpFlags(updateFieldFlags, "GameObjectUpdateFieldFlags[GAMEOBJECT_END]",
        { &GetInputData()->ObjectFields, &GetInputData()->GameObjectFields });

    DumpDynamicFlags(updateFieldFlags, "GameObjectDynamicUpdateFieldFlags[GAMEOBJECT_DYNAMIC_END]",
        { &GetInputData()->GameObjectDynamicFields });

    DumpFlags(updateFieldFlags, "DynamicObjectUpdateFieldFlags[DYNAMICOBJECT_END]",
        { &GetInputData()->ObjectFields, &GetInputData()->DynamicObjectFields });

    DumpFlags(updateFieldFlags, "CorpseUpdateFieldFlags[CORPSE_END]",
        { &GetInputData()->ObjectFields, &GetInputData()->CorpseFields });

    DumpFlags(updateFieldFlags, "AreaTriggerUpdateFieldFlags[AREATRIGGER_END]",
        { &GetInputData()->ObjectFields, &GetInputData()->AreaTriggerFields });

    DumpFlags(updateFieldFlags, "SceneObjectUpdateFieldFlags[SCENEOBJECT_END]",
        { &GetInputData()->ObjectFields, &GetInputData()->SceneObjectFields });

    DumpFlags(updateFieldFlags, "ConversationUpdateFieldFlags[CONVERSATION_END]",
        { &GetInputData()->ObjectFields, &GetInputData()->ConversationFields });

    DumpDynamicFlags(updateFieldFlags, "ConversationDynamicUpdateFieldFlags[CONVERSATION_DYNAMIC_END]",
        { &GetInputData()->ConversationDynamicFields });

    updateFieldFlags.close();
}

void CppUpdateFieldDumper::DumpEnum(std::ofstream& file, Outputs const& enumData)
{
    if (enumData.E.GetMembers().empty())
        return;

    file << SourceOutput<Enum>(std::make_unique<CppEnum>(), enumData.E, 0) << std::endl;
}

void CppUpdateFieldDumper::DumpFlags(std::ofstream& file, std::string const& varName, std::vector<std::vector<UpdateField>*> const& fields)
{
    file << "uint32 " << varName << " =" << std::endl;
    file << "{" << std::endl;

    for (std::size_t i = 0; i < fields.size(); ++i)
    {
        std::vector<UpdateField> const& fieldDefs = *fields[i];
        std::uintptr_t j = 0;
        while (j < fieldDefs.size())
        {
            std::string flagName = GetUpdateFieldFlagFullName(fieldDefs[j].Flags);
            std::string pad(PaddingSize - flagName.length(), ' ');
            std::string fieldName = GetInputData()->GetString(fieldDefs[j].NameAddress);
            std::string oldName = GetOldName(fieldName.c_str());
            if (!oldName.empty())
                fieldName = oldName;

            file << Tab << flagName << pad << " // " << fieldName << std::endl;
            for (std::uint32_t k = 1; k < fieldDefs[j].Size; ++k)
                file << Tab << flagName << pad << " // " << fieldName << '+' << k << std::endl;

            j += fieldDefs[j].Size;
        }
    }

    file << "};" << std::endl << std::endl;
}

void CppUpdateFieldDumper::DumpDynamicFlags(std::ofstream& file, std::string const& varName, std::vector<std::vector<DynamicUpdateField>*> const& fields)
{
    file << "uint32 " << varName << " =" << std::endl;
    file << "{" << std::endl;

    for (std::size_t i = 0; i < fields.size(); ++i)
    {
        std::vector<DynamicUpdateField> const& fieldDefs = *fields[i];
        std::uint32_t j = 0;
        while (j < fieldDefs.size())
        {
            std::string flagName = GetUpdateFieldFlagFullName(fieldDefs[j].Flags);
            std::string pad(PaddingSize - flagName.length(), ' ');
            std::string fieldName = GetInputData()->GetString(fieldDefs[j].NameAddress);
            std::string oldName = GetOldName(fieldName.c_str());
            if (!oldName.empty())
                fieldName = oldName;

            file << Tab << flagName << pad << " // " << fieldName << std::endl;
            ++j;
        }
    }

    file << "};" << std::endl << std::endl;
}

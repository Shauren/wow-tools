
#include "UpdateFieldNameMap.h"
#include "CppUpdateFieldDumper.h"

void CppUpdateFieldDumper::Dump()
{
    std::ofstream updateFieldsDump("UpdateFields.h");

    updateFieldsDump << "/*" << std::endl;
    updateFieldsDump << " * Copyright (C) 2008-2014 TrinityCore <http://www.trinitycore.org/>" << std::endl;
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

    DumpUpdateFields(updateFieldsDump, "ObjectFields", GetInputData()->ObjectFields, OBJECT_COUNT, "OBJECT_END", "");
    DumpDynamicFields(updateFieldsDump, "ObjectDynamicFields", nullptr, OBJECT_DYNAMIC_COUNT, "OBJECT_DYNAMIC_END", "");

    DumpUpdateFields(updateFieldsDump, "ItemFields", GetInputData()->ItemFields, ITEM_COUNT, "ITEM_END", "OBJECT_END");
    DumpDynamicFields(updateFieldsDump, "ItemDynamicFields", GetInputData()->ItemDynamicFields, ITEM_DYNAMIC_COUNT, "ITEM_DYNAMIC_END", "OBJECT_DYNAMIC_END");

    DumpUpdateFields(updateFieldsDump, "ContainerFields", GetInputData()->ContainerFields, CONTAINER_COUNT, "CONTAINER_END", "ITEM_END");
    DumpDynamicFields(updateFieldsDump, "ContainerDynamicFields", nullptr, CONTAINER_DYNAMIC_COUNT, "CONTAINER_DYNAMIC_END", "ITEM_DYNAMIC_END");

    DumpUpdateFields(updateFieldsDump, "UnitFields", GetInputData()->UnitFields, UNIT_COUNT, "UNIT_END", "OBJECT_END");
    DumpDynamicFields(updateFieldsDump, "UnitDynamicFields", GetInputData()->UnitDynamicFields, UNIT_DYNAMIC_COUNT, "UNIT_DYNAMIC_END", "OBJECT_DYNAMIC_END");

    DumpUpdateFields(updateFieldsDump, "PlayerFields", GetInputData()->PlayerFields, PLAYER_COUNT, "PLAYER_END", "UNIT_END");
    DumpDynamicFields(updateFieldsDump, "PlayerDynamicFields", GetInputData()->PlayerDynamicFields, PLAYER_DYNAMIC_COUNT, "PLAYER_DYNAMIC_END", "UNIT_DYNAMIC_END");

    DumpUpdateFields(updateFieldsDump, "GameObjectFields", GetInputData()->GameObjectFields, GAMEOBJECT_COUNT, "GAMEOBJECT_END", "OBJECT_END");
    DumpDynamicFields(updateFieldsDump, "GameObjectDynamicFields", nullptr, GAMEOBJECT_DYNAMIC_COUNT, "GAMEOBJECT_DYNAMIC_END", "OBJECT_DYNAMIC_END");

    DumpUpdateFields(updateFieldsDump, "DynamicObjectFields", GetInputData()->DynamicObjectFields, DYNAMICOBJECT_COUNT, "DYNAMICOBJECT_END", "OBJECT_END");
    DumpDynamicFields(updateFieldsDump, "DynamicObjectDynamicFields", nullptr, GAMEOBJECT_DYNAMIC_COUNT, "DYNAMICOBJECT_DYNAMIC_END", "OBJECT_DYNAMIC_END");

    DumpUpdateFields(updateFieldsDump, "CorpseFields", GetInputData()->CorpseFields, CORPSE_COUNT, "CORPSE_END", "OBJECT_END");
    DumpDynamicFields(updateFieldsDump, "CorpseDynamicFields", nullptr, GAMEOBJECT_DYNAMIC_COUNT, "CORPSE_DYNAMIC_END", "OBJECT_DYNAMIC_END");

    DumpUpdateFields(updateFieldsDump, "AreaTriggerFields", GetInputData()->AreaTriggerFields, AREATRIGGER_COUNT, "AREATRIGGER_END", "OBJECT_END");
    DumpDynamicFields(updateFieldsDump, "AreaTriggerDynamicFields", nullptr, GAMEOBJECT_DYNAMIC_COUNT, "AREATRIGGER_DYNAMIC_END", "OBJECT_DYNAMIC_END");

    DumpUpdateFields(updateFieldsDump, "SceneObjectFields", GetInputData()->SceneObjectFields, SCENEOBJECT_COUNT, "SCENEOBJECT_END", "OBJECT_END");
    DumpDynamicFields(updateFieldsDump, "SceneObjectDynamicFields", nullptr, GAMEOBJECT_DYNAMIC_COUNT, "SCENEOBJECT_DYNAMIC_END", "OBJECT_DYNAMIC_END");

    DumpUpdateFields(updateFieldsDump, "ConversationField", GetInputData()->ConversationFields, CONVERSATION_COUNT, "CONVERSATION_END", "OBJECT_END");
    DumpDynamicFields(updateFieldsDump, "ConversationDynamicField", GetInputData()->ConversationDynamicFields, CONVERSATION_DYNAMIC_COUNT, "CONVERSATION_DYNAMIC_END", "OBJECT_DYNAMIC_END");

    updateFieldsDump << "#endif // _UPDATEFIELDS_H" << std::endl;

    updateFieldsDump.close();

    std::ofstream updateFieldFlags("UpdateFieldFlags.cpp");

    updateFieldFlags << "/*" << std::endl;
    updateFieldFlags << " * Copyright (C) 2008-2014 TrinityCore <http://www.trinitycore.org/>" << std::endl;
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

    DumpFlags(updateFieldFlags, "ItemUpdateFieldFlags[CONTAINER_END]",
        { GetInputData()->ObjectFields, GetInputData()->ItemFields, GetInputData()->ContainerFields },
        { OBJECT_COUNT, ITEM_COUNT, CONTAINER_COUNT });

    DumpDynamicFlags(updateFieldFlags, "ItemDynamicUpdateFieldFlags[CONTAINER_DYNAMIC_END]",
        { GetInputData()->ItemDynamicFields },
        { ITEM_DYNAMIC_COUNT });

    DumpFlags(updateFieldFlags, "UnitUpdateFieldFlags[PLAYER_END]",
        { GetInputData()->ObjectFields, GetInputData()->UnitFields, GetInputData()->PlayerFields },
        { OBJECT_COUNT, UNIT_COUNT, PLAYER_COUNT });

    DumpDynamicFlags(updateFieldFlags, "UnitDynamicUpdateFieldFlags[PLAYER_DYNAMIC_END]",
        { GetInputData()->UnitDynamicFields, GetInputData()->PlayerDynamicFields },
        { UNIT_DYNAMIC_COUNT, PLAYER_DYNAMIC_COUNT });

    DumpFlags(updateFieldFlags, "GameObjectUpdateFieldFlags[GAMEOBJECT_END]",
        { GetInputData()->ObjectFields, GetInputData()->GameObjectFields },
        { OBJECT_COUNT, GAMEOBJECT_COUNT });

    DumpFlags(updateFieldFlags, "DynamicObjectUpdateFieldFlags[DYNAMICOBJECT_END]",
        { GetInputData()->ObjectFields, GetInputData()->DynamicObjectFields },
        { OBJECT_COUNT, DYNAMICOBJECT_COUNT });

    DumpFlags(updateFieldFlags, "CorpseUpdateFieldFlags[CORPSE_END]",
        { GetInputData()->ObjectFields, GetInputData()->CorpseFields },
        { OBJECT_COUNT, CORPSE_COUNT });

    DumpFlags(updateFieldFlags, "AreaTriggerUpdateFieldFlags[AREATRIGGER_END]",
        { GetInputData()->ObjectFields, GetInputData()->AreaTriggerFields },
        { OBJECT_COUNT, AREATRIGGER_COUNT });

    DumpFlags(updateFieldFlags, "SceneObjectUpdateFieldFlags[SCENEOBJECT_END]",
        { GetInputData()->ObjectFields, GetInputData()->SceneObjectFields },
        { OBJECT_COUNT, SCENEOBJECT_COUNT });

    DumpFlags(updateFieldFlags, "ConversationUpdateFieldFlags[CONVERSATION_END]",
        { GetInputData()->ObjectFields, GetInputData()->ConversationFields },
        { OBJECT_COUNT, CONVERSATION_COUNT });

    DumpDynamicFlags(updateFieldFlags, "ConversationDynamicUpdateFieldFlags[CONVERSATION_DYNAMIC_END]",
        { GetInputData()->ConversationDynamicFields },
        { CONVERSATION_DYNAMIC_COUNT });

    updateFieldFlags.close();
}

void CppUpdateFieldDumper::DumpUpdateFields(std::ofstream& file, std::string const& name, UpdateField* data, UpdateFieldSizes count, std::string const& end, std::string const& fieldBase)
{
    file << "enum " << name << std::endl;
    file << "{" << std::endl;

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
        if (name == "PLAYER_FIELD_INV_SLOT_HEAD")
            file << Tab << "PLAYER_FIELD_END_NOT_SELF " << pad << "= UNIT_END + " << hex_number(i) << ',' << std::endl << std::endl;

        file << Tab << name << pad << "= ";
        if (!fieldBase.empty())
            file << fieldBase << " + ";

        file << hex_number(i) << ',';
        file << " // Size: " << field->Size << ", Flags: " << GetUpdateFieldFlagName(field->Flags) << std::endl;
        i += field->Size;
    }

    file << Tab << end << std::string(PaddingSize - end.length(), ' ') << "= ";
    if (!fieldBase.empty())
        file << fieldBase << " + ";

    file << hex_number(i) << ',' << std::endl;

    file << "};" << std::endl << std::endl;
}

void CppUpdateFieldDumper::DumpDynamicFields(std::ofstream& file, std::string const& name, DynamicUpdateField* data, UpdateFieldSizes count, std::string const& end, std::string const& fieldBase)
{
    file << "enum " << name << std::endl;
    file << "{" << std::endl;

    std::int32_t i = 0;
    while (i < count)
    {
        DynamicUpdateField* field = &data[i];
        std::string name = GetInputData()->ReadProcessMemoryCString(data[i].NameAddress);
        std::string oldName = GetOldName(name.c_str());
        if (!oldName.empty())
            name = oldName;

        std::string pad(PaddingSize - name.length(), ' ');
        file << Tab << name << pad << "= ";
        if (!fieldBase.empty())
            file << fieldBase << " + ";

        file << hex_number(i) << ',';
        file << " //  Flags: " << GetUpdateFieldFlagName(field->Flags) << std::endl;
        ++i;
    }

    file << Tab << end << std::string(PaddingSize - end.length(), ' ') << "= ";
    if (!fieldBase.empty())
        file << fieldBase << " + ";

    file << hex_number(i) << ',' << std::endl;
    file << "};" << std::endl << std::endl;
}

void CppUpdateFieldDumper::DumpFlags(std::ofstream& file, std::string const& varName, std::vector<UpdateField*> const& fields, std::vector<UpdateFieldSizes> const& counts)
{
    file << "uint32 " << varName << " =" << std::endl;
    file << "{" << std::endl;

    for (std::size_t i = 0; i < fields.size(); ++i)
    {
        UpdateField* fieldDefs = fields[i];
        UpdateFieldSizes count = counts[i];

        std::int32_t j = 0;
        while (j < count)
        {
            std::string flagName = GetUpdateFieldFlagFullName(fieldDefs[j].Flags);
            std::string pad(PaddingSize - flagName.length(), ' ');
            std::string fieldName = GetInputData()->ReadProcessMemoryCString(fieldDefs[j].NameAddress);
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

void CppUpdateFieldDumper::DumpDynamicFlags(std::ofstream& file, std::string const& varName, std::vector<DynamicUpdateField*> const& fields, std::vector<UpdateFieldSizes> const& counts)
{
    file << "uint32 " << varName << " =" << std::endl;
    file << "{" << std::endl;

    for (std::size_t i = 0; i < fields.size(); ++i)
    {
        DynamicUpdateField* fieldDefs = fields[i];
        UpdateFieldSizes count = counts[i];

        std::int32_t j = 0;
        while (j < count)
        {
            std::string flagName = GetUpdateFieldFlagFullName(fieldDefs[j].Flags);
            std::string pad(PaddingSize - flagName.length(), ' ');
            std::string fieldName = GetInputData()->ReadProcessMemoryCString(fieldDefs[j].NameAddress);
            std::string oldName = GetOldName(fieldName.c_str());
            if (!oldName.empty())
                fieldName = oldName;

            file << Tab << flagName << pad << " // " << fieldName << std::endl;
            ++j;
        }
    }

    file << "};" << std::endl << std::endl;
}

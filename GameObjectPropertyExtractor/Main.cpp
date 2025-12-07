
#include "ProcessTools/ProcessTools.h"
#include "Structure.h"
#include <cstdint>
#include <unordered_map>
#include <fstream>
#include <string>
#include <algorithm>
#include <sstream>
#include <regex>
#include <map>
#include <bitset>

enum class TypeType
{
    UNKNOWN,
    DB_REF,
    ENUM,
    INTEGER
};

union GameObjectPropertyTypeInfo
{
    struct
    {
        std::uintptr_t Data[3];
    } Raw;
    struct
    {
        std::int32_t InvalidRefValue;
        std::int32_t _;
        char const* Name;
    } DbRef;
    struct
    {
        std::int32_t ValuesCount;
        char const** Values;
        std::int32_t DefaultValue;
    } Enum;
    struct
    {
        std::int32_t MinValue;
        std::int32_t MaxValue;
        std::int32_t DefaultValue;
    } Int;
};

TypeType GuessType(std::shared_ptr<Process> wow, GameObjectPropertyTypeInfo const& type)
{
    if (wow->IsValidAddress(type.Enum.Values) && type.Enum.DefaultValue < type.Enum.ValuesCount && type.Enum.ValuesCount > 0)
        return TypeType::ENUM;

    if (wow->IsValidAddress(type.DbRef.Name))
        return TypeType::DB_REF;

    return TypeType::INTEGER;
}

char const* GetIntType(TypeType typeType, GameObjectPropertyTypeInfo const& type)
{
    switch (typeType)
    {
        case TypeType::DB_REF:
            if (type.DbRef.InvalidRefValue < 0)
                return "int32";
            return "uint32";
        case TypeType::ENUM:
            return "uint32";
        case TypeType::INTEGER:
            if (std::uint32_t(type.Int.MinValue) > std::uint32_t(type.Int.MaxValue))
                return "int32";
            return "uint32";
        default:
            break;
    }
    return "uint32";
}

#pragma pack(push, 8)

struct GameObjectProperty
{
    static constexpr std::uintptr_t PROPERTY_DATA = 0x40AAB20;
    static constexpr std::size_t MAX_PROPERTY_INDEX = 261;

    std::uint32_t Index;
    char const* Name;
    std::uint32_t TypeIndex;
    std::uint32_t Unk2;
};

struct GameObjectPropertyInfo
{
    static constexpr std::uintptr_t GO_TYPE_DATA = 0x47E6E10;
    static constexpr std::size_t MAX_GAMEOBJECT_TYPE = 65;

    char const* TypeName;
    std::uint32_t Count;
    std::uint32_t* List;
    GameObjectPropertyTypeInfo** TypeInfo;
};

#pragma pack(pop)

constexpr std::uint32_t MAX_GAMEOBJECT_DATA = 35;

char const* TCEnumName[GameObjectPropertyInfo::MAX_GAMEOBJECT_TYPE] =
{
    "GAMEOBJECT_TYPE_DOOR",
    "GAMEOBJECT_TYPE_BUTTON",
    "GAMEOBJECT_TYPE_QUESTGIVER",
    "GAMEOBJECT_TYPE_CHEST",
    "GAMEOBJECT_TYPE_BINDER",
    "GAMEOBJECT_TYPE_GENERIC",
    "GAMEOBJECT_TYPE_TRAP",
    "GAMEOBJECT_TYPE_CHAIR",
    "GAMEOBJECT_TYPE_SPELL_FOCUS",
    "GAMEOBJECT_TYPE_TEXT",
    "GAMEOBJECT_TYPE_GOOBER",
    "GAMEOBJECT_TYPE_TRANSPORT",
    "GAMEOBJECT_TYPE_AREADAMAGE",
    "GAMEOBJECT_TYPE_CAMERA",
    "GAMEOBJECT_TYPE_MAP_OBJECT",
    "GAMEOBJECT_TYPE_MAP_OBJ_TRANSPORT",
    "GAMEOBJECT_TYPE_DUEL_ARBITER",
    "GAMEOBJECT_TYPE_FISHINGNODE",
    "GAMEOBJECT_TYPE_RITUAL",
    "GAMEOBJECT_TYPE_MAILBOX",
    "GAMEOBJECT_TYPE_DO_NOT_USE",
    "GAMEOBJECT_TYPE_GUARDPOST",
    "GAMEOBJECT_TYPE_SPELLCASTER",
    "GAMEOBJECT_TYPE_MEETINGSTONE",
    "GAMEOBJECT_TYPE_FLAGSTAND",
    "GAMEOBJECT_TYPE_FISHINGHOLE",
    "GAMEOBJECT_TYPE_FLAGDROP",
    "GAMEOBJECT_TYPE_MINI_GAME",
    "GAMEOBJECT_TYPE_DO_NOT_USE_2",
    "GAMEOBJECT_TYPE_CONTROL_ZONE",
    "GAMEOBJECT_TYPE_AURA_GENERATOR",
    "GAMEOBJECT_TYPE_DUNGEON_DIFFICULTY",
    "GAMEOBJECT_TYPE_BARBER_CHAIR",
    "GAMEOBJECT_TYPE_DESTRUCTIBLE_BUILDING",
    "GAMEOBJECT_TYPE_GUILD_BANK",
    "GAMEOBJECT_TYPE_TRAPDOOR",
    "GAMEOBJECT_TYPE_NEW_FLAG",
    "GAMEOBJECT_TYPE_NEW_FLAG_DROP",
    "GAMEOBJECT_TYPE_GARRISON_BUILDING",
    "GAMEOBJECT_TYPE_GARRISON_PLOT",
    "GAMEOBJECT_TYPE_CLIENT_CREATURE",
    "GAMEOBJECT_TYPE_CLIENT_ITEM",
    "GAMEOBJECT_TYPE_CAPTURE_POINT",
    "GAMEOBJECT_TYPE_PHASEABLE_MO",
    "GAMEOBJECT_TYPE_GARRISON_MONUMENT",
    "GAMEOBJECT_TYPE_GARRISON_SHIPMENT",
    "GAMEOBJECT_TYPE_GARRISON_MONUMENT_PLAQUE",
    "GAMEOBJECT_TYPE_ITEM_FORGE",
    "GAMEOBJECT_TYPE_UI_LINK",
    "GAMEOBJECT_TYPE_KEYSTONE_RECEPTACLE",
    "GAMEOBJECT_TYPE_GATHERING_NODE",
    "GAMEOBJECT_TYPE_CHALLENGE_MODE_REWARD",
    "GAMEOBJECT_TYPE_MULTI",
    "GAMEOBJECT_TYPE_SIEGEABLE_MULTI",
    "GAMEOBJECT_TYPE_SIEGEABLE_MO",
    "GAMEOBJECT_TYPE_PVP_REWARD",
    "GAMEOBJECT_TYPE_PLAYER_CHOICE_CHEST",
    "GAMEOBJECT_TYPE_LEGENDARY_FORGE",
    "GAMEOBJECT_TYPE_GARR_TALENT_TREE",
    "GAMEOBJECT_TYPE_WEEKLY_REWARD_CHEST",
    "GAMEOBJECT_TYPE_CLIENT_MODEL",
    "GAMEOBJECT_TYPE_CRAFTING_TABLE",
    "GAMEOBJECT_TYPE_PERKS_PROGRAM_CHEST",
    "GAMEOBJECT_TYPE_FUTURE_PATCH",
    "GAMEOBJECT_TYPE_ASSIST_ACTION"
};

std::string FormatType(std::shared_ptr<Process> wow, std::uint32_t typeIndex, GameObjectPropertyTypeInfo const& type);
std::int32_t GetPropDefault(std::shared_ptr<Process> wow, GameObjectPropertyTypeInfo const& type);

std::string FixName(std::string name)
{
    static std::regex const commentRemoval(R"(\s*\([^\)]+\))");
    static std::regex const normalizer("[^[:alnum:]]");
    std::string noExtra = std::regex_replace(name, commentRemoval, "");
    return std::regex_replace(noExtra, normalizer, "");
}

int main(int argc, char* argv[])
{
    std::shared_ptr<Process> wow = ProcessTools::Open(_T("Wow.exe"), 63506, true);
    if (!wow)
        return 1;

    std::vector<GameObjectProperty> props = wow->ReadArray<GameObjectProperty>(GameObjectProperty::PROPERTY_DATA, GameObjectProperty::MAX_PROPERTY_INDEX);
    std::unordered_map<std::string, std::int32_t> propDefaults;
    std::unordered_map<std::string, std::string> propTypes;
    std::string propertyNames[GameObjectProperty::MAX_PROPERTY_INDEX];
    for (std::uint32_t i = 0; i < props.size(); ++i)
        propertyNames[i] = wow->Read<std::string>(props[i].Name);

    std::vector<GameObjectPropertyInfo> typeData = wow->ReadArray<GameObjectPropertyInfo>(GameObjectPropertyInfo::GO_TYPE_DATA, GameObjectPropertyInfo::MAX_GAMEOBJECT_TYPE);

    Structure templateUnion;
    templateUnion.SetName("GameObjectTemplateData");

    std::vector<Structure> typeStructures;
    typeStructures.resize(typeData.size());

    for (std::uint32_t i = 0; i < typeData.size(); ++i)
    {
        Structure& typeStructure = typeStructures[i];
        typeStructure.SetComment(std::to_string(i) + " " + TCEnumName[i]);
        typeStructure.SetValueCommentPadding(56);

        std::uint32_t propCount = std::min(MAX_GAMEOBJECT_DATA, typeData[i].Count);
        std::vector<std::uint32_t> propIndexes = wow->ReadArray<std::uint32_t>(typeData[i].List, propCount);
        for (std::size_t j = 0; j < propIndexes.size(); ++j)
        {
            std::string name = propertyNames[propIndexes[j]];
            GameObjectPropertyTypeInfo* type = wow->Read<GameObjectPropertyTypeInfo*>(typeData[i].TypeInfo + j);
            GameObjectPropertyTypeInfo typeValue = wow->Read<GameObjectPropertyTypeInfo>(type);
            TypeType typeType = GuessType(wow, typeValue);
            std::string fixedName = FixName(name);
            propDefaults[fixedName] = GetPropDefault(wow, typeValue);
            propTypes[fixedName] = GetIntType(typeType, typeValue);
            typeStructure.AddMember(Structure::Member(std::uint32_t(j), GetIntType(typeType, typeValue), fixedName,
                (std::ostringstream() << j << " " << name << ", "
                    << FormatType(wow, props[propIndexes[j]].TypeIndex, typeValue)).str()));
        }

        std::string typeName = FixName(wow->Read<std::string>(typeData[i].TypeName));
        templateUnion.AddMember(Structure::Member(i,
            (std::ostringstream() << SourceOutput<Structure>(std::make_unique<CppStruct>(true), typeStructure, 4)).str(), typeName, ""));

        // Set name after converting to string and adding to union
        typeStructure.SetName(typeName);
    }

    Structure raw;
    raw.AddMember(Structure::Member(0, "uint32", "data[MAX_GAMEOBJECT_DATA]", ""));

    templateUnion.AddMember(Structure::Member(GameObjectPropertyInfo::MAX_GAMEOBJECT_TYPE,
        (std::ostringstream() << SourceOutput<Structure>(std::make_unique<CppStruct>(true), raw, 4)).str(), "raw", ""));

    // Generate accessor functions
    std::map<std::string, std::bitset<GameObjectPropertyInfo::MAX_GAMEOBJECT_TYPE>> propUsedByTypes;

    for (std::size_t i = 0; i < typeStructures.size(); ++i)
        for (StructureMember const& member : typeStructures[i].GetMembers())
            propUsedByTypes[member.ValueName][i] = true;

    for (auto itr = propUsedByTypes.begin(); itr != propUsedByTypes.end(); ++itr)
    {
        if (itr->second.count() <= 1 && propDefaults[itr->first] == 0)
            continue;

        std::string typeName = propTypes[itr->first] + " Get" + (char)std::toupper(itr->first[0]) + itr->first.substr(1) + "() const";
        std::string valueName = "\n"
            "    {\n"
            "        switch (type)\n"
            "        {\n";

        for (std::size_t bit = 0; bit < GameObjectPropertyInfo::MAX_GAMEOBJECT_TYPE; ++bit)
            if (itr->second[bit])
                valueName = valueName + "            case " + TCEnumName[bit] + ": return " + typeStructures[bit].GetName() + '.' + itr->first + ";\n";

        valueName +=
            "            default: return " + std::to_string(propDefaults[itr->first]) + ";\n"
            "        }\n"
            "    }";

        templateUnion.AddMember(Structure::Member(-1, typeName, valueName, "", true));
    }

    std::ofstream structure("GameObjectTemplate.h");
    structure << SourceOutput<Structure>(std::make_unique<CppUnion>(false), templateUnion, 0);
    return 0;
}

std::string FormatType(std::shared_ptr<Process> wow, std::uint32_t typeIndex, GameObjectPropertyTypeInfo const& type)
{
    std::ostringstream stream;
    switch (GuessType(wow, type))
    {
        case TypeType::DB_REF:
            stream << "References: " << wow->Read<std::string>(type.DbRef.Name) << ", NoValue = " << type.DbRef.InvalidRefValue;
            break;
        case TypeType::ENUM:
        {
            void const* values;
            stream << "enum {";
            for (std::int32_t i = 0; i < type.Enum.ValuesCount; ++i)
            {
                values = wow->Read<void const*>(type.Enum.Values + i);
                stream << " " << wow->Read<std::string>(values) << ",";
            }
            stream << " };";
            if (type.Enum.ValuesCount)
            {
                values = wow->Read<void const*>(type.Enum.Values + type.Enum.DefaultValue);
                stream << " Default: " << wow->Read<std::string>(values);
            }
            break;
        }
        case TypeType::INTEGER:
            stream << "int, Min value: " << type.Int.MinValue << ", Max value: " << type.Int.MaxValue << ", Default value: " << type.Int.DefaultValue;
            break;
        default:
            stream << "Type id: " << typeIndex;
            break;
    }

    return stream.str();
}

std::int32_t GetPropDefault(std::shared_ptr<Process> wow, GameObjectPropertyTypeInfo const& type)
{
    switch (GuessType(wow, type))
    {
        case TypeType::DB_REF:
            return type.DbRef.InvalidRefValue;
        case TypeType::ENUM:
            return type.Enum.DefaultValue;
        case TypeType::INTEGER:
            return type.Int.DefaultValue;
        default:
            break;
    }

    return 0;
}

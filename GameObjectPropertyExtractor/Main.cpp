
#include "ProcessTools/ProcessTools.h"
#include "Structure.h"
#include <cstdint>
#include <unordered_map>
#include <fstream>
#include <string>
#include <algorithm>
#include <sstream>
#include <regex>

enum TypeType
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
        return ENUM;

    if (wow->IsValidAddress(type.DbRef.Name))
        return DB_REF;

    return INTEGER;
}

char const* GetIntType(TypeType typeType, GameObjectPropertyTypeInfo const& type)
{
    switch (typeType)
    {
        case DB_REF:
            if (type.DbRef.InvalidRefValue < 0)
                return "int32";
            return "uint32";
        case ENUM:
            return "uint32";
        case INTEGER:
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
    std::uint32_t Index;
    char const* Name;
    std::uint32_t TypeIndex;
    std::uint32_t Unk2;
};

struct GameObjectPropertyInfo
{
    char const* TypeName;
    std::uint32_t Count;
    std::uint32_t* List;
    GameObjectPropertyTypeInfo** TypeInfo;
};

#pragma pack(pop)

#define MAX_GAMEOBJECT_TYPE 56
#define MAX_PROPERTY_INDEX 225

#define GO_TYPE_DATA 0x2333560
#define PROPERTY_DATA 0x1DBD8B0
#define MAX_GAMEOBJECT_DATA 34

char const* TCEnumName[MAX_GAMEOBJECT_TYPE] =
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
    "GAMEOBJECT_TYPE_ARTIFACT_FORGE",
    "GAMEOBJECT_TYPE_UI_LINK",
    "GAMEOBJECT_TYPE_KEYSTONE_RECEPTACLE",
    "GAMEOBJECT_TYPE_GATHERING_NODE",
    "GAMEOBJECT_TYPE_CHALLENGE_MODE_REWARD",
    "GAMEOBJECT_TYPE_MULTI",
    "GAMEOBJECT_TYPE_SIEGEABLE_MULTI",
    "GAMEOBJECT_TYPE_SIEGEABLE_MO",
    "GAMEOBJECT_TYPE_PVP_REWARD"
};

std::string FormatType(std::shared_ptr<Process> wow, std::uint32_t typeIndex, GameObjectPropertyTypeInfo const& type);

std::string FixName(std::string name)
{
    static std::regex const commentRemoval("\\s*\\([^\\)]+\\)");
    static std::regex const normalizer("[^[:alnum:]]");
    std::string noExtra = std::regex_replace(name, commentRemoval, "");
    return std::regex_replace(noExtra, normalizer, "");
}

int main(int argc, char* argv[])
{
    std::shared_ptr<Process> wow = ProcessTools::Open(_T("Wow.exe"), 27980, true);
    if (!wow)
        return 1;

    std::vector<GameObjectProperty> props = wow->ReadArray<GameObjectProperty>(PROPERTY_DATA, MAX_PROPERTY_INDEX);
    std::string propertyNames[MAX_PROPERTY_INDEX];
    for (std::uint32_t i = 0; i < props.size(); ++i)
        propertyNames[i] = wow->Read<std::string>(props[i].Name);

    std::vector<GameObjectPropertyInfo> typeData = wow->ReadArray<GameObjectPropertyInfo>(GO_TYPE_DATA, MAX_GAMEOBJECT_TYPE);

    Structure templateUnion;
    templateUnion.SetName("GameObjectTemplateData");

    for (std::uint32_t i = 0; i < typeData.size(); ++i)
    {
        Structure typeStructure;
        typeStructure.SetComment(std::to_string(i) + " " + TCEnumName[i]);
        typeStructure.SetValueCommentPadding(56);

        std::uint32_t propCount = std::min<std::uint32_t>(MAX_GAMEOBJECT_DATA, typeData[i].Count);
        std::vector<std::uint32_t> propIndexes = wow->ReadArray<std::uint32_t>(typeData[i].List, propCount);
        for (std::size_t j = 0; j < propIndexes.size(); ++j)
        {
            std::string name = propertyNames[propIndexes[j]];
            GameObjectPropertyTypeInfo* type = wow->Read<GameObjectPropertyTypeInfo*>(typeData[i].TypeInfo + j);
            GameObjectPropertyTypeInfo typeValue = wow->Read<GameObjectPropertyTypeInfo>(type);
            TypeType typeType = GuessType(wow, typeValue);
            typeStructure.AddMember(Structure::Member(j, GetIntType(typeType, typeValue), FixName(name),
                (std::ostringstream() << j << " " << name << ", "
                    << FormatType(wow, props[propIndexes[j]].TypeIndex, typeValue)).str()));
        }

        templateUnion.AddMember(Structure::Member(i,
            (std::ostringstream() << SourceOutput<Structure>(std::make_unique<CppStruct>(true), typeStructure, 4)).str(), FixName(wow->Read<std::string>(typeData[i].TypeName)), ""));
    }

    Structure raw;
    raw.AddMember(Structure::Member(0, "uint32", "data[MAX_GAMEOBJECT_DATA]", ""));

    templateUnion.AddMember(Structure::Member(MAX_GAMEOBJECT_TYPE,
        (std::ostringstream() << SourceOutput<Structure>(std::make_unique<CppStruct>(true), raw, 4)).str(), "raw", ""));

    std::ofstream structure("GameObjectTemplate.h");
    structure << SourceOutput<Structure>(std::make_unique<CppUnion>(false), templateUnion, 0);
    return 0;
}

std::string FormatType(std::shared_ptr<Process> wow, std::uint32_t typeIndex, GameObjectPropertyTypeInfo const& type)
{
    std::ostringstream stream;
    switch (GuessType(wow, type))
    {
        case DB_REF:
            stream << "References: " << wow->Read<std::string>(type.DbRef.Name) << ", NoValue = " << type.DbRef.InvalidRefValue;
            break;
        case ENUM:
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
        case INTEGER:
            stream << "int, Min value: " << type.Int.MinValue << ", Max value: " << type.Int.MaxValue << ", Default value: " << type.Int.DefaultValue;
            break;
        default:
            stream << "Type id: " << typeIndex;
            break;
    }

    return stream.str();
}

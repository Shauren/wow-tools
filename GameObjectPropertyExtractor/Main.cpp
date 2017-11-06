
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
    UNKNONW,
    DB_REF,
    ENUM,
    INTEGER
};

union GameObjectPropertyTypeInfo
{
    struct
    {
        std::int32_t Data[3];
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

TypeType PropTypes[58];

#define MAX_GAMEOBJECT_TYPE 52
#define MAX_PROPERTY_INDEX 217

#define GO_TYPE_DATA 0x141ED38
#define PROPERTY_DATA 0x11D82C0
#define MAX_GAMEOBJECT_DATA 33

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
    "GAMEOBJECT_TYPE_CHALLENGE_MODE_REWARD"
};

void InitTypes();
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
    std::shared_ptr<Process> wow = ProcessTools::Open(_T("WowT.exe"), 25383, true);
    if (!wow)
        return 1;

    std::vector<GameObjectProperty> props = wow->ReadArray<GameObjectProperty>(PROPERTY_DATA - 0x400000, MAX_PROPERTY_INDEX);
    std::string propertyNames[MAX_PROPERTY_INDEX];
    for (std::uint32_t i = 0; i < props.size(); ++i)
        propertyNames[i] = wow->Read<std::string>(props[i].Name);

    std::vector<GameObjectPropertyInfo> typeData = wow->ReadArray<GameObjectPropertyInfo>(GO_TYPE_DATA - 0x400000, MAX_GAMEOBJECT_TYPE);

    InitTypes();

    Structure templateUnion;
    templateUnion.SetName("GameObjectTemplateData");

    for (std::uint32_t i = 0; i < typeData.size(); ++i)
    {
        Structure typeStructure;
        typeStructure.SetComment(std::to_string(i) + " " + TCEnumName[i]);
        typeStructure.SetValueCommentPadding(40);

        std::uint32_t propCount = std::min<std::uint32_t>(MAX_GAMEOBJECT_DATA, typeData[i].Count);
        std::vector<std::uint32_t> propIndexes = wow->ReadArray<std::uint32_t>(typeData[i].List, propCount);
        for (std::size_t j = 0; j < propIndexes.size(); ++j)
        {
            std::string name = propertyNames[propIndexes[j]];
            GameObjectPropertyTypeInfo* type = wow->Read<GameObjectPropertyTypeInfo*>(typeData[i].TypeInfo + j);
            typeStructure.AddMember(Structure::Member(j, "uint32", FixName(name),
                static_cast<std::ostringstream&>(std::ostringstream() << j << " " << name << ", "
                    << FormatType(wow, props[propIndexes[j]].TypeIndex, wow->Read<GameObjectPropertyTypeInfo>(type))).str()));
        }

        templateUnion.AddMember(Structure::Member(i,
            static_cast<std::ostringstream&>(std::ostringstream() << SourceOutput<Structure>(std::make_unique<CppStruct>(true), typeStructure, 4)).str(), FixName(wow->Read<std::string>(typeData[i].TypeName)), ""));
    }

    Structure raw;
    raw.AddMember(Structure::Member(0, "uint32", "data[MAX_GAMEOBJECT_DATA]", ""));

    templateUnion.AddMember(Structure::Member(MAX_GAMEOBJECT_TYPE,
        static_cast<std::ostringstream&>(std::ostringstream() << SourceOutput<Structure>(std::make_unique<CppStruct>(true), raw, 4)).str(), "raw", ""));

    std::ofstream structure("GameObjectTemplate.h");
    structure << SourceOutput<Structure>(std::make_unique<CppUnion>(false), templateUnion, 0);
    return 0;
}

void InitTypes()
{
    PropTypes[4] = INTEGER;
    PropTypes[6] = ENUM;
    PropTypes[7] = ENUM;
    PropTypes[8] = ENUM;
    PropTypes[9] = ENUM;
    PropTypes[10] = ENUM;
    PropTypes[12] = INTEGER;
    PropTypes[13] = INTEGER;
    PropTypes[14] = INTEGER;
    PropTypes[15] = DB_REF;
    PropTypes[16] = DB_REF;
    PropTypes[17] = DB_REF;
    PropTypes[18] = DB_REF;
    PropTypes[19] = DB_REF;
    PropTypes[20] = DB_REF;
    PropTypes[21] = DB_REF;
    PropTypes[22] = DB_REF;
    PropTypes[23] = DB_REF;
    PropTypes[24] = DB_REF;
    PropTypes[25] = DB_REF;
    PropTypes[26] = DB_REF;
    PropTypes[27] = DB_REF;
    PropTypes[28] = DB_REF;
    PropTypes[29] = DB_REF;
    PropTypes[30] = DB_REF;
    PropTypes[31] = DB_REF;
    PropTypes[32] = DB_REF;
    PropTypes[33] = DB_REF;
    PropTypes[34] = DB_REF;
    PropTypes[35] = DB_REF;
    PropTypes[36] = DB_REF;
    PropTypes[37] = DB_REF;
    PropTypes[38] = DB_REF;
    PropTypes[39] = DB_REF;
    PropTypes[40] = DB_REF;
    PropTypes[41] = DB_REF;
    PropTypes[42] = DB_REF;
    PropTypes[43] = DB_REF;
    PropTypes[44] = DB_REF;
    PropTypes[45] = DB_REF;
    PropTypes[46] = DB_REF;
    PropTypes[47] = DB_REF;
    PropTypes[48] = DB_REF;
    PropTypes[49] = DB_REF;
    PropTypes[50] = DB_REF;
    PropTypes[51] = DB_REF;
    PropTypes[52] = DB_REF;
    PropTypes[53] = DB_REF;
    PropTypes[54] = DB_REF;
    PropTypes[55] = DB_REF;
    PropTypes[56] = DB_REF;
    PropTypes[57] = DB_REF;
}

std::string FormatType(std::shared_ptr<Process> wow, std::uint32_t typeIndex, GameObjectPropertyTypeInfo const& type)
{
    if (typeIndex >= std::extent<decltype(PropTypes)>::value)
        return "ERROR TOO LARGE TYPEINDEX " + std::to_string(typeIndex);

    std::ostringstream stream;
    switch (PropTypes[typeIndex])
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

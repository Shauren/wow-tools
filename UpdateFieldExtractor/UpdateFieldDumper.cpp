
#include "UpdateFieldDumper.h"
#include "UpdateFieldNameMap.h"
#include <sstream>
#include <cstdio>

enum UpdatefieldFlags
{
    UF_FLAG_NONE                = 0x000,
    UF_FLAG_PUBLIC              = 0x001,
    UF_FLAG_PRIVATE             = 0x002,
    UF_FLAG_OWNER               = 0x004,
    UF_FLAG_ITEM_OWNER          = 0x008,
    UF_FLAG_SPECIAL_INFO        = 0x010,
    UF_FLAG_PARTY_MEMBER        = 0x020,
    UF_FLAG_UNIT_ALL            = 0x040,
    UF_FLAG_DYNAMIC             = 0x080,
    UF_FLAG_0x100               = 0x100,
    UF_FLAG_URGENT              = 0x200,
    UF_FLAG_URGENT_SELF_ONLY    = 0x400
};

namespace Offsets
{
    DWORD_PTR const ObjectFields = 0xD221D0;
    DWORD_PTR const ItemFields = 0xD21DF8;
    DWORD_PTR const ItemDynamicFields = 0xD21A10;
    DWORD_PTR const ContainerFields = 0xD21300;
    DWORD_PTR const UnitFields = 0xD1FF60;
    DWORD_PTR const UnitDynamicFields = 0xD1F5C4;
    DWORD_PTR const PlayerFields = 0xD181C8;
    DWORD_PTR const PlayerDynamicFields = 0xD10428;
    DWORD_PTR const GameObjectFields = 0xD102E8;
    DWORD_PTR const DynamicObjectFields = 0xD100E8;
    DWORD_PTR const CorpseFields = 0xD0FE68;
    DWORD_PTR const AreaTriggerFields = 0xD0FBE8;
    DWORD_PTR const SceneObjectFields = 0xD0FAA0;
    DWORD_PTR const ConversationFields = 0xD0F9AC;
    DWORD_PTR const ConversationDynamicFields = 0xD0F900;

    DWORD_PTR BaseAddress;
}

Data::Data(HANDLE wow) : _process(wow)
{
#define READ_UPDATE_FIELDS(fields) \
    if (!ReadProcessMemory(wow, reinterpret_cast<LPCVOID>(Offsets::BaseAddress + Offsets::fields), this->fields, sizeof(this->fields), NULL)) \
        printf("Failed to read %s\n", #fields); \

    READ_UPDATE_FIELDS(ObjectFields);
    READ_UPDATE_FIELDS(ItemFields);
    READ_UPDATE_FIELDS(ItemDynamicFields);
    READ_UPDATE_FIELDS(ContainerFields);
    READ_UPDATE_FIELDS(UnitFields);
    READ_UPDATE_FIELDS(UnitDynamicFields);
    READ_UPDATE_FIELDS(PlayerFields);
    READ_UPDATE_FIELDS(PlayerDynamicFields);
    READ_UPDATE_FIELDS(GameObjectFields);
    READ_UPDATE_FIELDS(DynamicObjectFields);
    READ_UPDATE_FIELDS(CorpseFields);
    READ_UPDATE_FIELDS(AreaTriggerFields);
    READ_UPDATE_FIELDS(SceneObjectFields);
    READ_UPDATE_FIELDS(ConversationFields);
    READ_UPDATE_FIELDS(ConversationDynamicFields);

#undef READ_UPDATE_FIELDS
}

std::string Data::ReadProcessMemoryCString(DWORD_PTR address)
{
    std::unordered_map<DWORD_PTR, std::string>::const_iterator itr = _stringCache.find(address);
    if (itr != _stringCache.end())
        return itr->second;

    char buffer;
    std::string str;
    DWORD_PTR orig_address = address;
    while (ReadProcessMemory(_process, reinterpret_cast<LPCVOID>(address++), &buffer, 1, NULL) && buffer != '\0')
        str.append(1, buffer);

    return _stringCache[orig_address] = str;
}

void UpdateFieldDumper::BuildUpdateFieldEnum(Enum& enumData, std::string const& name, UpdateField* data, UpdateFieldSizes count, std::string const& end, std::string const& fieldBase)
{
    enumData.SetName(name);

    std::uint32_t i = 0;
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

        enumData.AddMember(Enum::Member(i, FormatValue(i, fieldBase), name,
            static_cast<std::ostringstream&>(std::ostringstream() << "Size: " << field->Size << ", Flags: " << GetUpdateFieldFlagName(field->Flags)).str()));

        i += field->Size;
    }

    enumData.AddMember(Enum::Member(i, FormatValue(i, fieldBase), end, ""));
}

void UpdateFieldDumper::BuildDynamicUpdateFieldEnum(Enum& enumData, std::string const& name, DynamicUpdateField* data, UpdateFieldSizes count, std::string const& end, std::string const& fieldBase)
{
    enumData.SetName(name);

    std::uint32_t i = 0;
    while (i < count)
    {
        DynamicUpdateField* field = &data[i];
        std::string name = GetInputData()->ReadProcessMemoryCString(data[i].NameAddress);
        std::string oldName = GetOldName(name.c_str());
        if (!oldName.empty())
            name = oldName;

        enumData.AddMember(Enum::Member(i, FormatValue(i, fieldBase), name,
            static_cast<std::ostringstream&>(std::ostringstream() << "Flags: " << GetUpdateFieldFlagName(field->Flags)).str()));
        ++i;
    }

    enumData.AddMember(Enum::Member(i, FormatValue(i, fieldBase), end, ""));
}

std::string const UpdateFieldDumper::Tab = std::string(4, ' ');

std::string UpdateFieldDumper::FormatVersion(std::string const& partSeparator) const
{
    std::ostringstream str;
    str << _versionInfo.FileMajorPart << partSeparator << _versionInfo.FileMinorPart << partSeparator
        << _versionInfo.FileBuildPart << partSeparator << _versionInfo.FilePrivatePart;

    return str.str();
}

std::string UpdateFieldDumper::FormatValue(std::uint32_t val, std::string const& valueBase)
{
    std::ostringstream str;
    if (!valueBase.empty())
        str << valueBase << " + ";

    str << hex_number(val);
    return str.str();
}

void UpdateFieldDumper::DumpEnums(std::ofstream& updateFieldsDump)
{
    DumpEnum(updateFieldsDump, ObjectFields);
    DumpEnum(updateFieldsDump, ObjectDynamicFields);
    DumpEnum(updateFieldsDump, ItemFields);
    DumpEnum(updateFieldsDump, ItemDynamicFields);
    DumpEnum(updateFieldsDump, ContainerFields);
    DumpEnum(updateFieldsDump, ContainerDynamicFields);
    DumpEnum(updateFieldsDump, UnitFields);
    DumpEnum(updateFieldsDump, UnitDynamicFields);
    DumpEnum(updateFieldsDump, PlayerFields);
    DumpEnum(updateFieldsDump, PlayerDynamicFields);
    DumpEnum(updateFieldsDump, GameObjectFields);
    DumpEnum(updateFieldsDump, GameObjectDynamicFields);
    DumpEnum(updateFieldsDump, DynamicObjectFields);
    DumpEnum(updateFieldsDump, DynamicObjectDynamicFields);
    DumpEnum(updateFieldsDump, CorpseFields);
    DumpEnum(updateFieldsDump, CorpseDynamicFields);
    DumpEnum(updateFieldsDump, AreaTriggerFields);
    DumpEnum(updateFieldsDump, AreaTriggerDynamicFields);
    DumpEnum(updateFieldsDump, SceneObjectFields);
    DumpEnum(updateFieldsDump, SceneObjectDynamicFields);
    DumpEnum(updateFieldsDump, ConversationFields);
    DumpEnum(updateFieldsDump, ConversationDynamicFields);
}

std::string UpdateFieldDumper::GetUpdateFieldFlagName(std::uint16_t flag)
{
    if (!flag)
        return "NONE";

    std::string name;
    AppendIf(flag, UF_FLAG_PUBLIC, name, "PUBLIC", ", ");
    AppendIf(flag, UF_FLAG_PRIVATE, name, "PRIVATE", ", ");
    AppendIf(flag, UF_FLAG_OWNER, name, "OWNER", ", ");
    AppendIf(flag, UF_FLAG_ITEM_OWNER, name, "ITEM_OWNER", ", ");
    AppendIf(flag, UF_FLAG_SPECIAL_INFO, name, "SPECIAL_INFO", ", ");
    AppendIf(flag, UF_FLAG_PARTY_MEMBER, name, "PARTY_MEMBER", ", ");
    AppendIf(flag, UF_FLAG_UNIT_ALL, name, "UNIT_ALL", ", ");
    AppendIf(flag, UF_FLAG_DYNAMIC, name, "DYNAMIC", ", ");
    AppendIf(flag, UF_FLAG_0x100, name, "0x100", ", ");
    AppendIf(flag, UF_FLAG_URGENT, name, "URGENT", ", ");
    AppendIf(flag, UF_FLAG_URGENT_SELF_ONLY, name, "URGENT_SELF_ONLY", ", ");
    return name;
}

std::string UpdateFieldDumper::GetUpdateFieldFlagFullName(std::uint16_t flag)
{
    if (!flag)
        return "UF_FLAG_NONE,";

    std::string name;
    AppendIf(flag, UF_FLAG_PUBLIC, name, "UF_FLAG_PUBLIC", " | ");
    AppendIf(flag, UF_FLAG_PRIVATE, name, "UF_FLAG_PRIVATE", " | ");
    AppendIf(flag, UF_FLAG_OWNER, name, "UF_FLAG_OWNER", " | ");
    AppendIf(flag, UF_FLAG_ITEM_OWNER, name, "UF_FLAG_ITEM_OWNER", " | ");
    AppendIf(flag, UF_FLAG_SPECIAL_INFO, name, "UF_FLAG_SPECIAL_INFO", " | ");
    AppendIf(flag, UF_FLAG_PARTY_MEMBER, name, "UF_FLAG_PARTY_MEMBER", " | ");
    AppendIf(flag, UF_FLAG_UNIT_ALL, name, "UF_FLAG_UNIT_ALL", " | ");
    AppendIf(flag, UF_FLAG_DYNAMIC, name, "UF_FLAG_DYNAMIC", " | ");
    AppendIf(flag, UF_FLAG_0x100, name, "UF_FLAG_0x100", " | ");
    AppendIf(flag, UF_FLAG_URGENT, name, "UF_FLAG_URGENT", " | ");
    AppendIf(flag, UF_FLAG_URGENT_SELF_ONLY, name, "UF_FLAG_URGENT_SELF_ONLY", " | ");
    name.append(1, ',');
    return name;
}

void UpdateFieldDumper::AppendIf(std::uint16_t flag, std::uint16_t flagToCheck, std::string& str, std::string const& flagName, std::string const& separator)
{
    if (!(flag & flagToCheck))
        return;

    if (!str.empty())
        str.append(separator);

    str.append(flagName);
}

UpdateFieldDumper::UpdateFieldDumper(HANDLE source, Data* input, FileVersionInfo const& version, std::uint32_t enumPadding) : _source(source), _input(input), _versionInfo(version)
{
    ObjectFields.SetPaddingAfterValueName(enumPadding);
    ObjectDynamicFields.SetPaddingAfterValueName(enumPadding);
    ItemFields.SetPaddingAfterValueName(enumPadding);
    ItemDynamicFields.SetPaddingAfterValueName(enumPadding);
    ContainerFields.SetPaddingAfterValueName(enumPadding);
    ContainerDynamicFields.SetPaddingAfterValueName(enumPadding);
    UnitFields.SetPaddingAfterValueName(enumPadding);
    UnitDynamicFields.SetPaddingAfterValueName(enumPadding);
    PlayerFields.SetPaddingAfterValueName(enumPadding);
    PlayerDynamicFields.SetPaddingAfterValueName(enumPadding);
    GameObjectFields.SetPaddingAfterValueName(enumPadding);
    GameObjectDynamicFields.SetPaddingAfterValueName(enumPadding);
    DynamicObjectFields.SetPaddingAfterValueName(enumPadding);
    DynamicObjectDynamicFields.SetPaddingAfterValueName(enumPadding);
    CorpseFields.SetPaddingAfterValueName(enumPadding);
    CorpseDynamicFields.SetPaddingAfterValueName(enumPadding);
    AreaTriggerFields.SetPaddingAfterValueName(enumPadding);
    AreaTriggerDynamicFields.SetPaddingAfterValueName(enumPadding);
    SceneObjectFields.SetPaddingAfterValueName(enumPadding);
    SceneObjectDynamicFields.SetPaddingAfterValueName(enumPadding);
    ConversationFields.SetPaddingAfterValueName(enumPadding);
    ConversationDynamicFields.SetPaddingAfterValueName(enumPadding);
}

std::ostream& operator<<(std::ostream& stream, hex_number const& hex)
{
    stream << "0x";

    std::ios_base::fmtflags oldflags = stream.flags();
    char fill = stream.fill('0');
    auto width = stream.width(3);
    stream.unsetf(std::ios_base::showbase);
    stream.setf(std::ios_base::uppercase);
    stream.setf(std::ios_base::hex, std::ios_base::basefield);

    stream << hex.Value;

    stream.flags(oldflags);
    stream.fill(fill);
    stream.width(width);

    return stream;
}

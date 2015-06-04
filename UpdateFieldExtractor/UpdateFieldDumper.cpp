
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
    std::uintptr_t const ObjectFields = 0xDBC570;
    std::uintptr_t const ItemFields = 0xDBC198;
    std::uintptr_t const ItemDynamicFields = 0xDBBDB0;
    std::uintptr_t const ContainerFields = 0xDBB698;
    std::uintptr_t const UnitFields = 0xDBA2C0;
    std::uintptr_t const UnitDynamicFields = 0xDB98F0;
    std::uintptr_t const PlayerFields = 0xDB07A8;
    std::uintptr_t const PlayerDynamicFields = 0xDA6C78;
    std::uintptr_t const GameObjectFields = 0xDA6B30;
    std::uintptr_t const DynamicObjectFields = 0xDA6930;
    std::uintptr_t const CorpseFields = 0xDA66B0;
    std::uintptr_t const AreaTriggerFields = 0xDA6430;
    std::uintptr_t const SceneObjectFields = 0xDA62E8;
    std::uintptr_t const ConversationFields = 0xDA61F4;
    std::uintptr_t const ConversationDynamicFields = 0xDA6148;
}

Data::Data(std::shared_ptr<Process> wow) : _process(wow)
{
    ObjectFields = _process->ReadArray<UpdateField>(Offsets::ObjectFields, OBJECT_COUNT);
    ItemFields = _process->ReadArray<UpdateField>(Offsets::ItemFields, ITEM_COUNT);
    ItemDynamicFields = _process->ReadArray<DynamicUpdateField>(Offsets::ItemDynamicFields, ITEM_DYNAMIC_COUNT);
    ContainerFields = _process->ReadArray<UpdateField>(Offsets::ContainerFields, CONTAINER_COUNT);
    UnitFields = _process->ReadArray<UpdateField>(Offsets::UnitFields, UNIT_COUNT);
    UnitDynamicFields = _process->ReadArray<DynamicUpdateField>(Offsets::UnitDynamicFields, UNIT_DYNAMIC_COUNT);
    PlayerFields = _process->ReadArray<UpdateField>(Offsets::PlayerFields, PLAYER_COUNT);
    PlayerDynamicFields = _process->ReadArray<DynamicUpdateField>(Offsets::PlayerDynamicFields, PLAYER_DYNAMIC_COUNT);
    GameObjectFields = _process->ReadArray<UpdateField>(Offsets::GameObjectFields, GAMEOBJECT_COUNT);
    DynamicObjectFields = _process->ReadArray<UpdateField>(Offsets::DynamicObjectFields, DYNAMICOBJECT_COUNT);
    CorpseFields = _process->ReadArray<UpdateField>(Offsets::CorpseFields, CORPSE_COUNT);
    AreaTriggerFields = _process->ReadArray<UpdateField>(Offsets::AreaTriggerFields, AREATRIGGER_COUNT);
    SceneObjectFields = _process->ReadArray<UpdateField>(Offsets::SceneObjectFields, SCENEOBJECT_COUNT);
    ConversationFields = _process->ReadArray<UpdateField>(Offsets::ConversationFields, CONVERSATION_COUNT);
    ConversationDynamicFields = _process->ReadArray<DynamicUpdateField>(Offsets::ConversationDynamicFields, CONVERSATION_DYNAMIC_COUNT);
}

void UpdateFieldDumper::BuildUpdateFields(Outputs& outputs, std::string const& name, std::vector<UpdateField> const& data, std::string const& end, std::string const& fieldBase)
{
    outputs.SetName(name);

    std::uint32_t i = 0;
    while (i < data.size())
    {
        UpdateField const* field = &data[i];
        std::string name = GetInputData()->GetString(data[i].NameAddress);
        if (name == "CGUnitData::npcFlags[UMNW0]")
        {
            name = "CGUnitData::npcFlags";
            field = &data[i + 1];
        }

        std::string oldName = GetOldName(name.c_str());
        if (!oldName.empty())
            name = oldName;

        outputs.E.AddMember(Enum::Member(i, FormatValue(i, fieldBase), name,
            static_cast<std::ostringstream&>(std::ostringstream() << "Size: " << field->Size << ", Flags: " << GetUpdateFieldFlagName(field->Flags)).str()));

        if (field->Size > 1)
            name += "[" + std::to_string(field->Size) + "]";

        outputs.S.AddMember(Structure::Member(i, "_DWORD", name, ""));

        i += field->Size;
    }

    outputs.E.AddMember(Enum::Member(i, FormatValue(i, fieldBase), end, ""));
}

void UpdateFieldDumper::BuildDynamicUpdateFields(Outputs& outputs, std::string const& name, std::vector<DynamicUpdateField> const& data, std::string const& end, std::string const& fieldBase)
{
    outputs.SetName(name);

    std::uint32_t i = 0;
    while (i < data.size())
    {
        DynamicUpdateField const* field = &data[i];
        std::string name = GetInputData()->GetString(data[i].NameAddress);
        std::string oldName = GetOldName(name.c_str());
        if (!oldName.empty())
            name = oldName;

        outputs.E.AddMember(Enum::Member(i, FormatValue(i, fieldBase), name,
            static_cast<std::ostringstream&>(std::ostringstream() << "Flags: " << GetUpdateFieldFlagName(field->Flags)).str()));
        ++i;
    }

    outputs.E.AddMember(Enum::Member(i, FormatValue(i, fieldBase), end, ""));
}

std::string const UpdateFieldDumper::Tab = std::string(4, ' ');

std::string UpdateFieldDumper::FormatVersion(std::string const& partSeparator) const
{
    FileVersionInfo const& version = GetVersionInfo();
    std::ostringstream str;
    str << version.FileMajorPart << partSeparator << version.FileMinorPart << partSeparator
        << version.FileBuildPart << partSeparator << version.FilePrivatePart;

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

UpdateFieldDumper::UpdateFieldDumper(std::shared_ptr<Data> input, std::uint32_t enumPadding) : _input(input)
{
    ObjectFields.E.SetPaddingAfterValueName(enumPadding);
    ObjectDynamicFields.E.SetPaddingAfterValueName(enumPadding);
    ItemFields.E.SetPaddingAfterValueName(enumPadding);
    ItemDynamicFields.E.SetPaddingAfterValueName(enumPadding);
    ContainerFields.E.SetPaddingAfterValueName(enumPadding);
    ContainerDynamicFields.E.SetPaddingAfterValueName(enumPadding);
    UnitFields.E.SetPaddingAfterValueName(enumPadding);
    UnitDynamicFields.E.SetPaddingAfterValueName(enumPadding);
    PlayerFields.E.SetPaddingAfterValueName(enumPadding);
    PlayerDynamicFields.E.SetPaddingAfterValueName(enumPadding);
    GameObjectFields.E.SetPaddingAfterValueName(enumPadding);
    GameObjectDynamicFields.E.SetPaddingAfterValueName(enumPadding);
    DynamicObjectFields.E.SetPaddingAfterValueName(enumPadding);
    DynamicObjectDynamicFields.E.SetPaddingAfterValueName(enumPadding);
    CorpseFields.E.SetPaddingAfterValueName(enumPadding);
    CorpseDynamicFields.E.SetPaddingAfterValueName(enumPadding);
    AreaTriggerFields.E.SetPaddingAfterValueName(enumPadding);
    AreaTriggerDynamicFields.E.SetPaddingAfterValueName(enumPadding);
    SceneObjectFields.E.SetPaddingAfterValueName(enumPadding);
    SceneObjectDynamicFields.E.SetPaddingAfterValueName(enumPadding);
    ConversationFields.E.SetPaddingAfterValueName(enumPadding);
    ConversationDynamicFields.E.SetPaddingAfterValueName(enumPadding);
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

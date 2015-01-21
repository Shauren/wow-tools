
#ifndef UpdateFieldDumper_h__
#define UpdateFieldDumper_h__

#include <cstdint>
#include <string>
#include <unordered_map>
#include <fstream>
#include <sstream>
#include <iomanip>
#include <Windows.h>
#include "ProcessTools/ProcessTools.h"
#include "Enum.h"

#pragma pack(push,4)

enum UpdateFieldSizes : std::uint32_t
{
    OBJECT_COUNT                = 12,
    OBJECT_DYNAMIC_COUNT        = 0,
    ITEM_COUNT                  = 70,
    ITEM_DYNAMIC_COUNT          = 2,
    CONTAINER_COUNT             = 145,
    CONTAINER_DYNAMIC_COUNT     = 0,
    UNIT_COUNT                  = 191,
    UNIT_DYNAMIC_COUNT          = 2,
    PLAYER_COUNT                = 2473,
    PLAYER_DYNAMIC_COUNT        = 6,
    GAMEOBJECT_COUNT            = 21,
    GAMEOBJECT_DYNAMIC_COUNT    = 0,
    DYNAMICOBJECT_COUNT         = 8,
    DYNAMICOBJECT_DYNAMIC_COUNT = 0,
    CORPSE_COUNT                = 33,
    CORPSE_DYNAMIC_COUNT        = 0,
    AREATRIGGER_COUNT           = 8,
    AREATRIGGER_DYNAMIC_COUNT   = 0,
    SCENEOBJECT_COUNT           = 7,
    SCENEOBJECT_DYNAMIC_COUNT   = 0,
    CONVERSATION_COUNT          = 1,
    CONVERSATION_DYNAMIC_COUNT  = 2
};

struct UpdateField
{
    std::uint32_t NameAddress;
    std::uint32_t Size;
    std::uint16_t Flags;
    std::uint16_t __PADDING__;
};

struct DynamicUpdateField
{
    std::uint32_t NameAddress;
    std::uint16_t Flags;
    std::uint16_t __PADDING__;
};

class Data
{
public:
    Data(std::shared_ptr<Process> wow);
    std::string const& GetString(std::uintptr_t address) { return _process->Read<std::string>(address, false); }

    std::vector<UpdateField> ObjectFields;
    std::vector<UpdateField> ItemFields;
    std::vector<DynamicUpdateField> ItemDynamicFields;
    std::vector<UpdateField> ContainerFields;
    std::vector<UpdateField> UnitFields;
    std::vector<DynamicUpdateField> UnitDynamicFields;
    std::vector<UpdateField> PlayerFields;
    std::vector<DynamicUpdateField> PlayerDynamicFields;
    std::vector<UpdateField> GameObjectFields;
    std::vector<UpdateField> DynamicObjectFields;
    std::vector<UpdateField> CorpseFields;
    std::vector<UpdateField> AreaTriggerFields;
    std::vector<UpdateField> SceneObjectFields;
    std::vector<UpdateField> ConversationFields;
    std::vector<DynamicUpdateField> ConversationDynamicFields;

    std::shared_ptr<Process> GetProcess() { return _process; }

private:
    std::shared_ptr<Process> _process;
};

namespace Offsets
{
    extern DWORD_PTR BaseAddress;
}

#pragma pack(pop)

class UpdateFieldDumper
{
public:
    UpdateFieldDumper(std::shared_ptr<Data> input, std::uint32_t enumPadding);
    virtual ~UpdateFieldDumper() { }

    virtual void Dump() = 0;

    static std::string const Tab;

protected:
    void BuildUpdateFieldEnum(Enum& enumData, std::string const& name, std::vector<UpdateField> const& data, std::string const& end, std::string const& fieldBase);
    void BuildDynamicUpdateFieldEnum(Enum& enumData, std::string const& name, std::vector<DynamicUpdateField> const& data, std::string const& end, std::string const& fieldBase);
    std::string FormatVersion(std::string const& partSeparator) const;
    static std::string FormatValue(std::uint32_t val, std::string const& valueBase);

    void DumpEnums(std::ofstream& updateFieldsDump);
    virtual void DumpEnum(std::ofstream& file, Enum const& enumData) = 0;
    std::shared_ptr<Data> GetInputData() { return _input; }
    FileVersionInfo const& GetVersionInfo() const { return _input->GetProcess()->GetFileVersionInfo(); }
    static std::string GetUpdateFieldFlagName(std::uint16_t flag);
    static std::string GetUpdateFieldFlagFullName(std::uint16_t flag);

    Enum ObjectFields;
    Enum ObjectDynamicFields;
    Enum ItemFields;
    Enum ItemDynamicFields;
    Enum ContainerFields;
    Enum ContainerDynamicFields;
    Enum UnitFields;
    Enum UnitDynamicFields;
    Enum PlayerFields;
    Enum PlayerDynamicFields;
    Enum GameObjectFields;
    Enum GameObjectDynamicFields;
    Enum DynamicObjectFields;
    Enum DynamicObjectDynamicFields;
    Enum CorpseFields;
    Enum CorpseDynamicFields;
    Enum AreaTriggerFields;
    Enum AreaTriggerDynamicFields;
    Enum SceneObjectFields;
    Enum SceneObjectDynamicFields;
    Enum ConversationFields;
    Enum ConversationDynamicFields;

private:
    static void AppendIf(std::uint16_t flag, std::uint16_t flagToCheck, std::string& str, std::string const& flagName, std::string const& separator);

    std::shared_ptr<Data> _input;
};

struct hex_number
{
    explicit hex_number(std::uint32_t v) : Value(v) { }
    std::uint32_t Value;
};

std::ostream& operator<<(std::ostream& stream, hex_number const& hex);

#endif // UpdateFieldDumper_h__

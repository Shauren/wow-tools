
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
#include "Structure.h"

#pragma pack(push,8)

struct UpdateField
{
    std::uintptr_t NameAddress;
    std::uintptr_t Size;
    std::uint16_t Flags;
    std::uint16_t __PADDING__[3];
};

struct DynamicUpdateField
{
    std::uintptr_t NameAddress;
    std::uint16_t Flags;
    std::uint16_t __PADDING__[3];
};

#pragma pack(pop)

struct UpdateFieldOffsets;

class Data
{
public:
    Data(std::shared_ptr<Process> wow, UpdateFieldOffsets const* offsets);
    std::string const& GetString(std::uintptr_t address) { return _process->Read<std::string>(address, false); }

    std::vector<UpdateField> ObjectFields;
    std::vector<UpdateField> ItemFields;
    std::vector<DynamicUpdateField> ItemDynamicFields;
    std::vector<UpdateField> ContainerFields;
    std::vector<UpdateField> AzeriteEmpoweredItemFields;
    std::vector<UpdateField> AzeriteItemFields;
    std::vector<UpdateField> UnitFields;
    std::vector<DynamicUpdateField> UnitDynamicFields;
    std::vector<UpdateField> PlayerFields;
    std::vector<DynamicUpdateField> PlayerDynamicFields;
    std::vector<UpdateField> ActivePlayerFields;
    std::vector<DynamicUpdateField> ActivePlayerDynamicFields;
    std::vector<UpdateField> GameObjectFields;
    std::vector<DynamicUpdateField> GameObjectDynamicFields;
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

class UpdateFieldDumper
{
public:
    UpdateFieldDumper(std::shared_ptr<Data> input, std::uint32_t enumPadding);
    virtual ~UpdateFieldDumper() { }

    virtual void Dump() = 0;

    static std::string const Tab;

protected:
    struct Outputs
    {
        Enum E;
        Structure S;

        void SetName(std::string const& name) { E.SetName(name); S.SetName(name); }
    };

    void BuildUpdateFields(Outputs& enumData, std::string const& outputName, std::vector<UpdateField> const& data, std::string const& end, std::string const& fieldBase);
    void BuildDynamicUpdateFields(Outputs& enumData, std::string const& outputName, std::vector<DynamicUpdateField> const& data, std::string const& end, std::string const& fieldBase);
    std::string FormatVersion(std::string const& partSeparator) const;
    static std::string FormatValue(std::uint32_t val, std::string const& valueBase);

    void DumpEnums(std::ofstream& updateFieldsDump);
    virtual void DumpEnum(std::ofstream& file, Outputs const& enumData) = 0;
    std::shared_ptr<Data> GetInputData() { return _input; }
    FileVersionInfo const& GetVersionInfo() const { return _input->GetProcess()->GetFileVersionInfo(); }
    static std::string GetUpdateFieldFlagName(std::uint16_t flag);
    static std::string GetUpdateFieldFlagFullName(std::uint16_t flag);

    Outputs ObjectFields;
    Outputs ObjectDynamicFields;
    Outputs ItemFields;
    Outputs ItemDynamicFields;
    Outputs ContainerFields;
    Outputs ContainerDynamicFields;
    Outputs AzeriteEmpoweredItemFields;
    Outputs AzeriteEmpoweredItemDynamicFields;
    Outputs AzeriteItemFields;
    Outputs AzeriteItemDynamicFields;
    Outputs UnitFields;
    Outputs UnitDynamicFields;
    Outputs PlayerFields;
    Outputs PlayerDynamicFields;
    Outputs ActivePlayerFields;
    Outputs ActivePlayerDynamicFields;
    Outputs GameObjectFields;
    Outputs GameObjectDynamicFields;
    Outputs DynamicObjectFields;
    Outputs DynamicObjectDynamicFields;
    Outputs CorpseFields;
    Outputs CorpseDynamicFields;
    Outputs AreaTriggerFields;
    Outputs AreaTriggerDynamicFields;
    Outputs SceneObjectFields;
    Outputs SceneObjectDynamicFields;
    Outputs ConversationFields;
    Outputs ConversationDynamicFields;

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

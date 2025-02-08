
#include "ProcessTools/ProcessTools.h"
#include "Enum.h"
#include <fstream>

enum UIErrorOutput : std::uint32_t
{
    UI_ERROR_CHAT = 0,
    UI_ERROR_INFO_MESSAGE = 1,
    UI_ERROR_ERROR_MESSAGE = 2,
    UI_ERROR_CONSOLE = 3,
};

struct UIErrorInfo
{
    char const* ErrorName;
    UIErrorOutput OutputTarget;
    std::uint32_t Sound;
    std::uint32_t VocalErrorSoundId;
    std::uint32_t ChatMsgType;
};

void DumpEnum(Enum const& enumData, std::string const& fileNameBase)
{
    std::ofstream dump(fileNameBase + ".h");
    dump << SourceOutput<Enum>(std::make_unique<CppEnum>("uint32"), enumData, 0);
    dump.close();

    dump.open(fileNameBase + ".idc");
    dump << SourceOutput<Enum>(std::make_unique<IdcEnum>(), enumData, 0);
}

void DumpUIErrors(std::shared_ptr<Process> wow)
{
    static std::uintptr_t const UIErrorsOffset = 0x40CCE80;
    static std::size_t const UIErrorsSize = 1181;

    Enum uiErrors;
    uiErrors.SetName("class GameError");
    uiErrors.SetPaddingAfterValueName(64);
    std::vector<UIErrorInfo> errors = wow->ReadArray<UIErrorInfo>(UIErrorsOffset, UIErrorsSize);
    for (std::size_t i = 0; i < errors.size(); ++i)
    {
        std::string error = wow->Read<std::string>(errors[i].ErrorName);
        if (!error.empty())
            uiErrors.AddMember(Enum::Member(std::uint32_t(i), error, ""));
    }

    DumpEnum(uiErrors, "UIErrors");
}

struct WowCS_FragmentDefinition
{
    std::uint32_t FragmentID;
    char const* Name;
    std::uint32_t StorageType;
    std::uint8_t Field_14;
    std::uint32_t Field_18;
    std::uint32_t FragmentSize;
    std::uint16_t Field_20;
    void* Factory;
    void* Assignment;
    void* Destructor;
    void* Field_40;
    void* Field_48;
    std::uint32_t Field_50;

    bool IsInitialOnly() const { return (Field_18 & 2) != 0; }
    bool IsUpdatable() const { return StorageType != 4 && Field_14 >= 3 && !IsInitialOnly(); }
    bool IsOwnerOnly() const { return Field_14 >= 3 && (Field_18 & 1); }
    bool IsIndirect() const { return StorageType == 1; }
    bool IsTag() const { return StorageType == 4; }
};

void DumpWowCSData(std::shared_ptr<Process> wow)
{
    static std::uintptr_t const FragmentsOffset = 0x406A3F0;
    static std::size_t const FragmentsSize = 256;

    std::ofstream out("WowCSEntityDefinitions.h");

    Enum fragmentsEnum;
    fragmentsEnum.SetName("WowCSEntityFragments");
    fragmentsEnum.SetPaddingAfterValueName(28);
    std::vector<WowCS_FragmentDefinition> fragments = wow->ReadArray<WowCS_FragmentDefinition>(FragmentsOffset, FragmentsSize);
    for (std::size_t i = 0; i < fragments.size(); ++i)
    {
        WowCS_FragmentDefinition const& fragment = fragments[i];
        if (fragment.FragmentID != i)
            continue;

        std::string comment;
        if (fragment.IsInitialOnly())
            comment += " INITIAL_ONLY,";
        if (fragment.IsUpdatable())
            comment += " UPDATEABLE,";
        if (fragment.IsOwnerOnly())
            comment += " OWNER_ONLY,";
        if (fragment.IsIndirect())
            comment += " INDIRECT,";
        if (fragment.IsTag())
            comment += " TAG,";

        fragmentsEnum.AddMember(Enum::Member(i, wow->Read<std::string>(fragment.Name), comment));
    }

    out << SourceOutput<Enum>(std::make_unique<CppEnum>("uint32"), fragmentsEnum, 0);
}

int main()
{
    std::shared_ptr<Process> wow = ProcessTools::Open(_T("Wow_11.0.7.58046-T-64_orig.exe"), 58046, true);
    if (!wow)
        return 1;

    DumpUIErrors(wow);
    DumpWowCSData(wow);
    return 0;
}

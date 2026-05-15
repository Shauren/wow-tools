
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

void DumpUIErrors(Process& wow)
{
    static constexpr std::uintptr_t UIErrorsOffset = 0x3EC8270;
    static constexpr std::size_t UIErrorsSize = 1240;

    Enum uiErrors;
    uiErrors.SetName("class GameError");
    uiErrors.SetPaddingAfterValueName(64);
    std::vector<UIErrorInfo> errors = wow.ReadArray<UIErrorInfo>(UIErrorsOffset, UIErrorsSize);
    for (std::size_t i = 0; i < errors.size(); ++i)
    {
        std::string error = wow.Read<std::string>(errors[i].ErrorName);
        if (!error.empty())
            uiErrors.AddMember(Enum::Member(std::uint32_t(i), error, ""));
    }

    DumpEnum(uiErrors, "UIErrors");
}

struct WowCS_FragmentDefinition
{
    std::uint32_t FragmentID;
    char const* Name;
    std::uint32_t Flags;
    std::uint32_t FragmentSize;
    std::uint32_t Field_18;
    std::uint8_t StorageType;
    std::uint8_t SomethingRelevant;
    std::uint8_t Pad[146];

    bool IsInitialOnly() const { return (Flags & 2) != 0; }
    bool IsUpdatable() const { return StorageType != 4 && SomethingRelevant >= 3 && !IsInitialOnly(); }
    bool IsOwnerOnly() const { return SomethingRelevant >= 3 && (Flags & 1); }
    bool IsIndirect() const { return StorageType == 1; }
    bool IsTag() const { return StorageType == 4; }
};

void DumpWowCSData(Process& wow)
{
    static constexpr std::uintptr_t FragmentsOffset = 0x4095F70;
    static constexpr std::size_t FragmentsSize = 256;

    std::ofstream out("WowCSEntityDefinitions.h");

    Enum fragmentsEnum;
    fragmentsEnum.SetName("WowCSEntityFragments");
    fragmentsEnum.SetPaddingAfterValueName(32);
    std::vector<WowCS_FragmentDefinition> fragments = wow.ReadArray<WowCS_FragmentDefinition>(FragmentsOffset, FragmentsSize);
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

        fragmentsEnum.AddMember(Enum::Member(i, wow.Read<std::string>(fragment.Name), comment));
    }

    out << SourceOutput<Enum>(std::make_unique<CppEnum>("uint32"), fragmentsEnum, 0);
}

void DumpResponseCodes(Process& wow)
{
    static constexpr std::uintptr_t Offset = 0x3666DE0;
    static constexpr std::size_t Size = 113;

    Enum responseCodes;
    responseCodes.SetName("ResponseCodes");
    responseCodes.SetPaddingAfterValueName(55);
    std::vector<char const*> codes = wow.ReadArray<char const*>(Offset, Size);
    for (std::size_t i = 0; i < codes.size(); ++i)
    {
        std::string_view error = wow.Read<std::string>(codes[i]);
        if (!error.empty())
            responseCodes.AddMember(Enum::Member(std::uint32_t(i), error, ""));
    }

    DumpEnum(responseCodes, "ResponseCodes");
}

int main()
{
    std::shared_ptr<Process> wow = ProcessTools::Open(_T("WowT.exe"), 67088, true);
    if (!wow)
        return 1;

    DumpUIErrors(*wow);
    DumpWowCSData(*wow);
    DumpResponseCodes(*wow);
    return 0;
}

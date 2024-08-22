
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
    static std::uintptr_t const UIErrorsOffset = 0x419A200;
    static std::size_t const UIErrorsSize = 1169;

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

int main()
{
    std::shared_ptr<Process> wow = ProcessTools::Open(_T("Wow_11.0.2.56071-B-64_orig.exe"), 56071, true);
    if (!wow)
        return 1;

    DumpUIErrors(wow);
    return 0;
}

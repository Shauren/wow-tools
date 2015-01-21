
#include "ProcessTools/ProcessTools.h"
#include "Enum.h"
#include <fstream>

enum UIErrorOutput
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
    char const* Sound;
    std::uint32_t VocalErrorSoundId;
    std::uint32_t ChatMsgType;
};

int main()
{
    std::shared_ptr<Process> wow = ProcessTools::Open(_T("Wow.exe"), 19342, true);
    if (!wow)
        return 1;

    Enum uiErrors;
    uiErrors.SetName("GAME_ERROR_TYPE");
    std::vector<UIErrorInfo> errors = wow->ReadArray<UIErrorInfo>(0xFAD998 - 0x400000, 931);
    for (std::size_t i = 0; i < errors.size(); ++i)
        uiErrors.AddMember(Enum::Member(i, wow->Read<std::string>(errors[i].ErrorName), ""));

    std::ofstream uiErrorsDump("UIErrors.h");
    uiErrorsDump << SourceOutput<Enum>(std::make_unique<CppEnum>(), uiErrors, 0);
    uiErrorsDump.close();

    uiErrorsDump.open("UIErrors.idc");
    uiErrorsDump << SourceOutput<Enum>(std::make_unique<IdcEnum>(), uiErrors, 0);
}

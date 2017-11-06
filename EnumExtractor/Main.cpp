
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
    static std::uintptr_t const UIErrorsOffset = 0x10231E8;
    static std::size_t const UIErrorsSize = 987;

    Enum uiErrors;
    uiErrors.SetName("GAME_ERROR_TYPE");
    uiErrors.SetPaddingAfterValueName(56);
    std::vector<UIErrorInfo> errors = wow->ReadArray<UIErrorInfo>(UIErrorsOffset, UIErrorsSize);
    for (std::size_t i = 0; i < errors.size(); ++i)
    {
        std::string error = wow->Read<std::string>(errors[i].ErrorName);
        if (!error.empty())
            uiErrors.AddMember(Enum::Member(i, error, ""));
    }

    DumpEnum(uiErrors, "UIErrors");
}

void DumpFrameXML_Events(std::shared_ptr<Process> wow)
{
    static std::uintptr_t const FrameXML_EventsOffset = 0x12298C8;
    std::size_t const FrameXML_EventsSize = 1163;

    Enum frameXML;
    frameXML.SetName("FrameXML_Events");
    std::vector<char const*> events = wow->ReadArray<char const*>(FrameXML_EventsOffset, FrameXML_EventsSize);
    for (std::size_t i = 0; i < events.size(); ++i)
    {
        std::string evt = wow->Read<std::string>(events[i]);
        if (!evt.empty())
            frameXML.AddMember(Enum::Member(i, evt, ""));
    }

    DumpEnum(frameXML, "FrameXML_Events");
}

int main()
{
    std::shared_ptr<Process> wow = ProcessTools::Open(_T("WowT.exe"), 25383, true);
    if (!wow)
        return 1;

    DumpUIErrors(wow);
    DumpFrameXML_Events(wow);
}

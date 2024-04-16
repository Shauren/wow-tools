
#include <Windows.h>
#include <cstdio>
#include "Enum.h"
#include <fstream>
#include <map>

void DumpEnum(Enum const& enumData, std::string const& fileNameBase)
{
    std::ofstream dump(fileNameBase + ".h");
    dump << SourceOutput<Enum>(std::make_unique<CppEnum>(), enumData, 0);
    dump.close();

    dump.open(fileNameBase + ".idc");
    dump << SourceOutput<Enum>(std::make_unique<IdcEnum>(), enumData, 0);
    dump.close();
}

void DumpSpellFailures()
{
    using pGetErrorString = char* (__fastcall*)(int);
    pGetErrorString GetStringReason = (pGetErrorString)((DWORD_PTR)GetModuleHandle(nullptr) + 0x251AE90);

    Enum spellFailures;
    spellFailures.SetName("SpellCastResult");
    spellFailures.SetPaddingAfterValueName(60);
    int err = 0;
    std::string error = GetStringReason(err);
    while (true)
    {
        if (!error.empty())
            spellFailures.AddMember(Enum::Member(std::size_t(err), error, ""));

        if (error == "SPELL_FAILED_UNKNOWN")
            break;

        error = GetStringReason(++err);
    }

    DumpEnum(spellFailures, "SpellCastResult");
}

void DumpInventoryErrors()
{
    struct UIErrorInfo
    {
        char const* ErrorName;
        std::uint32_t OutputTarget;
        std::uint32_t Sound;
        std::uint32_t VocalErrorSoundId;
        std::uint32_t ChatMsgType;
    };

    UIErrorInfo* uis = (UIErrorInfo*)((DWORD_PTR)GetModuleHandle(nullptr) + 0x408DDD0);

    using GetGameErrorFn = int(__fastcall*)(int);
    GetGameErrorFn CGBag_C_GetGameError = (GetGameErrorFn)((DWORD_PTR)GetModuleHandle(nullptr) + 0x2777730);

    Enum spellFailures;
    spellFailures.SetName("InventoryResult");
    spellFailures.SetPaddingAfterValueName(55);
    int err = 0;
    int error = CGBag_C_GetGameError(err);
    std::multimap<std::string, int> duplicates;
    while (err <= 119)
    {
        std::string err_name = "EQUIP_";
        if (error < 1137)
            err_name += uis[error].ErrorName;
        else
            err_name += "NONE";

        duplicates.emplace(err_name, err);
        if (duplicates.count(err_name) > 1)
            err_name += "_" + std::to_string(duplicates.count(err_name));

        spellFailures.AddMember(Enum::Member(std::size_t(err), err_name, ""));
        error = CGBag_C_GetGameError(++err);
    }

    DumpEnum(spellFailures, "InventoryResult");
}

void DumpSwitchedEnums()
{
    DumpSpellFailures();
    DumpInventoryErrors();
}

BOOL WINAPI DllMain(HANDLE hDllHandle, DWORD dwReason, LPVOID lpreserved)
{
    DumpSwitchedEnums();

    // don't stay loaded, work was already done.
    // yes, this is horrible
    return FALSE;
}

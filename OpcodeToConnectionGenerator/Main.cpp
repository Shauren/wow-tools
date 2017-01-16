
#include <Windows.h>
#include <cstdio>
#include "ServerOpcodes.h"
#include "Enum.h"
#include <fstream>
#include <map>

typedef bool(__cdecl *JamCheckFn)(WORD);

struct JamGroup
{
    JamCheckFn BelongsToGroup;
    JamCheckFn RequiresInstanceConnection;
};

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
    typedef char*(__cdecl* pGetErrorString)(int);
    pGetErrorString GetStringReason = (pGetErrorString)((DWORD_PTR)GetModuleHandle(NULL) + 0x29EDDC);

    Enum spellFailures;
    spellFailures.SetName("SPELL_FAILED_REASON");
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
        int z;
        void* a;
        int b[2];
    };

    UIErrorInfo* uis = (UIErrorInfo*)((DWORD_PTR)GetModuleHandle(NULL) + 0xC85558);

    typedef int(__cdecl* GetGameErrorFn)(int);
    GetGameErrorFn CGBag_C_GetGameError = (GetGameErrorFn)((DWORD_PTR)GetModuleHandle(NULL) + 0x3A563E);

    Enum spellFailures;
    spellFailures.SetName("InventoryResult");
    spellFailures.SetPaddingAfterValueName(55);
    int err = 0;
    int error = CGBag_C_GetGameError(err);
    std::multimap<std::string, int> duplicates;
    while (err <= 100)
    {
        std::string err_name = std::string("EQUIP_");
        if (error < 984)
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
    //initopcodes();

    //DWORD_PTR BaseAddress = (DWORD_PTR)GetModuleHandle(NULL);
    //BaseAddress -= 0x400000; // because im lazy and use not rebased offsets

    //JamGroup grp[4];
    //grp[0].BelongsToGroup = ((JamCheckFn)(BaseAddress + 0x6030C7));
    //grp[0].RequiresInstanceConnection = ((JamCheckFn)(BaseAddress + 0x603077));
    //grp[1].BelongsToGroup = ((JamCheckFn)(BaseAddress + 0x6073AA));
    //grp[1].RequiresInstanceConnection = ((JamCheckFn)(BaseAddress + 0x60811C));
    //grp[2].BelongsToGroup = ((JamCheckFn)(BaseAddress + 0x62D089));
    //grp[2].RequiresInstanceConnection = ((JamCheckFn)(BaseAddress + 0x62D02D));
    //grp[3].BelongsToGroup = ((JamCheckFn)(BaseAddress + 0xCDC3CF));
    //grp[3].RequiresInstanceConnection = ((JamCheckFn)(BaseAddress + 0xCDC27E));

    //FILE* dump = nullptr;
    //fopen_s(&dump, "dump.txt", "w");
    //if (!dump)
    //    return FALSE;

    //for (WORD opc = 0; opc < 0x1FFF; ++opc)
    //{
    //    if (((opc - 1) & 0x17CE) == 1026 || ((opc - 1) & 0x17BC) == 1672) // auth
    //        continue;

    //    if (grp[0].BelongsToGroup(opc))
    //    {
    //        if (grp[0].RequiresInstanceConnection(opc))
    //        {
    //            if (OpcodeNames[opc])
    //                fprintf(dump, "        case %s: // ClientQuest\n", OpcodeNames[opc]);
    //            else
    //                fprintf(dump, "        case 0x%04X: // ClientQuest\n", opc);
    //        }
    //    }
    //    else if (grp[1].BelongsToGroup(opc))
    //    {
    //        if (grp[1].RequiresInstanceConnection(opc))
    //        {
    //            if (OpcodeNames[opc])
    //                fprintf(dump, "        case %s: // Client\n", OpcodeNames[opc]);
    //            else
    //                fprintf(dump, "        case 0x%04X: // Client\n", opc);
    //        }
    //    }
    //    else if (grp[2].BelongsToGroup(opc))
    //    {
    //        if (grp[2].RequiresInstanceConnection(opc))
    //        {
    //            if (OpcodeNames[opc])
    //                fprintf(dump, "        case %s: // ClientGuild\n", OpcodeNames[opc]);
    //            else
    //                fprintf(dump, "        case 0x%04X: // ClientGuild\n", opc);
    //        }
    //    }
    //    else if (grp[3].BelongsToGroup(opc))
    //    {
    //        if (grp[3].RequiresInstanceConnection(opc))
    //        {
    //            if (OpcodeNames[opc])
    //                fprintf(dump, "        case %s: // ClientSpell\n", OpcodeNames[opc]);
    //            else
    //                fprintf(dump, "        case 0x%04X: // ClientSpell\n", opc);
    //        }
    //    }
    //}

    //fclose(dump);

    DumpSwitchedEnums();

    // don't stay loaded, work was already done.
    // yes, this is horrible
    return FALSE;
}

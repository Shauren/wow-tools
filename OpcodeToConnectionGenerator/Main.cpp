
#include <Windows.h>
#include <cstdio>
#include "ServerOpcodes.h"
#include "Enum.h"
#include <fstream>

typedef bool(__cdecl *JamCheckFn)(WORD);

struct JamGroup
{
    JamCheckFn BelongsToGroup;
    JamCheckFn RequiresInstanceConnection;
};

/*
char __cdecl sub_5F4D86(int a1, int a2, int a3, void *a4, size_t a5)
{
  char result; // al@4

  if ( (((_WORD)a2 - 1) & 0x1508) != 5376
    || (unsigned __int8)sub_5F534B(a2) && NetClient::GetWowConnectionIndex(a3) != 1 )
  {
    result = 0;
  }
  else
  {
    sub_653B0B(a1, 23, a3, a4, a5);
    result = 1;
  }
  return result;
}

char __cdecl sub_5FF895(int a1, int a2, int a3, void *a4, size_t a5)
{
  __int16 v5; // ax@1
  int v6; // ecx@3
  int v7; // ecx@13
  int v8; // ecx@16
  int v9; // eax@18
  char result; // al@22

  v5 = a2 - 1;
  if ( (((_WORD)a2 - 1) & 0x1F54) != 4608
    && (v5 & 0x1DDC) != 208
    && (v6 = v5 & 0xDDC, v6 != 256)
    && v6 != 2304
    && (v5 & 0x5D4) != 384
    && (v5 & 0x5D6) != 272
    && (v5 & 0x76C) != 832
    && (v5 & 0x5F4) != 480
    && (v5 & 0x15D4) != 4
    && (v5 & 0x15C4) != 260
    && (v5 & 0x15BE) != 5256
    && (v5 & 0x174E) != 1288
    && (v7 = v5 & 0x74E, v7 != 1292)
    && v7 != 1294
    && (v5 & 0x74A) != 1800
    && (v8 = v5 & 0xFCE, v8 != 1352)
    && v8 != 1480
    && (v9 = v5 & 0xDCE, v9 != 1354)
    && v9 != 1482
    || (unsigned __int8)sub_5FA8D3(a2) && NetClient::GetWowConnectionIndex(a3) != 1 )
  {
    result = 0;
  }
  else
  {
    sub_653B0B(a1, 23, a3, a4, a5);
    result = 1;
  }
  return result;
}

char __cdecl sub_61F2FF(int a1, int a2, int a3, void *a4, size_t a5)
{
  char result; // al@4

  if ( (((_WORD)a2 - 1) & 0x158C) != 4100
    || (unsigned __int8)sub_61F832(a2) && NetClient::GetWowConnectionIndex((void *)a1, a3) != 1 )
  {
    result = 0;
  }
  else
  {
    sub_653B0B(a1, 23, a3, a4, a5);
    result = 1;
  }
  return result;
}

char __cdecl sub_CAE52D(int a1, int a2, int a3, void *a4, size_t a5)
{
  __int16 v5; // ax@1
  char result; // al@9

  v5 = a2 - 1;
  if ( (((_WORD)a2 - 1) & 0xD54) != 2048
    && (v5 & 0x5D6) != 274
    && (v5 & 0x56E) != 330
    && (v5 & 0xDE4) != 352
    && (v5 & 0x5F4) != 496
    && (v5 & 0x74A) != 1802
    || (unsigned __int8)sub_CAE4CB(a2) && NetClient::GetWowConnectionIndex(a3) != 1 )
  {
    result = 0;
  }
  else
  {
    sub_653B0B(a1, 23, a3, a4, a5);
    result = 1;
  }
  return result;
}
*/

void DumpEnum(Enum const& enumData, std::string const& fileNameBase)
{
    std::ofstream dump(fileNameBase + ".h");
    dump << SourceOutput<Enum>(std::make_unique<CppEnum>(), enumData, 0);
    dump.close();

    dump.open(fileNameBase + ".idc");
    dump << SourceOutput<Enum>(std::make_unique<IdcEnum>(), enumData, 0);
}

void DumpSpellFailures()
{
    typedef char*(__cdecl* pGetErrorString)(int);
    pGetErrorString GetErrorString = (pGetErrorString)((DWORD_PTR)GetModuleHandle(NULL) + 0x23ABDB);

    Enum spellFailures;
    spellFailures.SetName("SPELL_FAILED_REASON");
    int err = 0;
    std::string error = GetErrorString(err);
    while (true)
    {
        if (!error.empty())
            spellFailures.AddMember(Enum::Member(std::size_t(err), error, ""));

        if (error == "SPELL_FAILED_UNKNOWN")
            break;

        error = GetErrorString(++err);
    }

    DumpEnum(spellFailures, "SpellCastResult");
}

void DumpSwitchedEnums()
{
    DumpSpellFailures();
}

BOOL WINAPI DllMain(HANDLE hDllHandle, DWORD dwReason, LPVOID lpreserved)
{
    initopcodes();

    DWORD_PTR BaseAddress = (DWORD_PTR)GetModuleHandle(NULL);
    BaseAddress -= 0x400000; // because im lazy and use not rebased offsets

    JamGroup grp[4];
    grp[0].BelongsToGroup = ((JamCheckFn)(BaseAddress + 0x6030C7));
    grp[0].RequiresInstanceConnection = ((JamCheckFn)(BaseAddress + 0x603077));
    grp[1].BelongsToGroup = ((JamCheckFn)(BaseAddress + 0x6073AA));
    grp[1].RequiresInstanceConnection = ((JamCheckFn)(BaseAddress + 0x60811C));
    grp[2].BelongsToGroup = ((JamCheckFn)(BaseAddress + 0x62D089));
    grp[2].RequiresInstanceConnection = ((JamCheckFn)(BaseAddress + 0x62D02D));
    grp[3].BelongsToGroup = ((JamCheckFn)(BaseAddress + 0xCDC3CF));
    grp[3].RequiresInstanceConnection = ((JamCheckFn)(BaseAddress + 0xCDC27E));

    FILE* dump = nullptr;
    fopen_s(&dump, "dump.txt", "w");
    if (!dump)
        return FALSE;

    for (WORD opc = 0; opc < 0x1FFF; ++opc)
    {
        if (((opc - 1) & 0x17CE) == 1026 || ((opc - 1) & 0x17BC) == 1672) // auth
            continue;

        if (grp[0].BelongsToGroup(opc))
        {
            if (grp[0].RequiresInstanceConnection(opc))
            {
                if (OpcodeNames[opc])
                    fprintf(dump, "        case %s: // ClientQuest\n", OpcodeNames[opc]);
                else
                    fprintf(dump, "        case 0x%04X: // ClientQuest\n", opc);
            }
        }
        else if (grp[1].BelongsToGroup(opc))
        {
            if (grp[1].RequiresInstanceConnection(opc))
            {
                if (OpcodeNames[opc])
                    fprintf(dump, "        case %s: // Client\n", OpcodeNames[opc]);
                else
                    fprintf(dump, "        case 0x%04X: // Client\n", opc);
            }
        }
        else if (grp[2].BelongsToGroup(opc))
        {
            if (grp[2].RequiresInstanceConnection(opc))
            {
                if (OpcodeNames[opc])
                    fprintf(dump, "        case %s: // ClientGuild\n", OpcodeNames[opc]);
                else
                    fprintf(dump, "        case 0x%04X: // ClientGuild\n", opc);
            }
        }
        else if (grp[3].BelongsToGroup(opc))
        {
            if (grp[3].RequiresInstanceConnection(opc))
            {
                if (OpcodeNames[opc])
                    fprintf(dump, "        case %s: // ClientSpell\n", OpcodeNames[opc]);
                else
                    fprintf(dump, "        case 0x%04X: // ClientSpell\n", opc);
            }
        }
    }

    fclose(dump);

    DumpSwitchedEnums();

    // don't stay loaded, work was already done.
    // yes, this is horrible
    return FALSE;
}

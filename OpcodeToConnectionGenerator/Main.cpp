
#include <Windows.h>
#include <cstdio>
#include "ServerOpcodes.h"

typedef BYTE(__cdecl *JamCheckFn)(DWORD opcode);

struct JamGroup
{
    JamCheckFn BelongsToGroup;
    JamCheckFn RequiresInstanceConnection;
};

BYTE __cdecl IsClientQuest(DWORD a2)
{
    if ((((WORD)a2 - 1) & 0x1508) != 5376)
    {
        return FALSE;
    }

    return TRUE;
}

BYTE __cdecl IsClient(DWORD a2)
{
    __int16 v5; // ax@1
    int v6; // ecx@3
    int v7; // ecx@13
    int v8; // ecx@16
    int v9; // eax@18

    v5 = (WORD)a2 - 1;
    if ((((WORD)a2 - 1) & 0x1F54) != 4608
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
        && v9 != 1482)
    {
        return FALSE;
    }

    return TRUE;
}

BYTE __cdecl IsClientGuild(DWORD a2)
{
    if ((((WORD)a2 - 1) & 0x158C) != 4100)
    {
        return FALSE;
    }

    return TRUE;
}

BYTE __cdecl IsClientSpell(DWORD a2)
{
    __int16 v5; // ax@1

    v5 = (WORD)a2 - 1;
    if ((((WORD)a2 - 1) & 0xD54) != 2048
        && (v5 & 0x5D6) != 274
        && (v5 & 0x56E) != 330
        && (v5 & 0xDE4) != 352
        && (v5 & 0x5F4) != 496
        && (v5 & 0x74A) != 1802)
    {
        return FALSE;
    }

    return TRUE;
}

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

BOOL WINAPI DllMain(HANDLE hDllHandle, DWORD dwReason, LPVOID lpreserved)
{
    initopcodes();

    DWORD_PTR BaseAddress = (DWORD_PTR)GetModuleHandle(NULL);
    BaseAddress -= 0x400000; // because im lazy and use not rebased offsets

    JamGroup grp[4];
    grp[0].BelongsToGroup = &IsClientQuest;
    grp[0].RequiresInstanceConnection = ((JamCheckFn)(BaseAddress + 0x5F534B));
    grp[1].BelongsToGroup = &IsClient;
    grp[1].RequiresInstanceConnection = ((JamCheckFn)(BaseAddress + 0x5FA8D3));
    grp[2].BelongsToGroup = &IsClientGuild;
    grp[2].RequiresInstanceConnection = ((JamCheckFn)(BaseAddress + 0x61F832));
    grp[3].BelongsToGroup = &IsClientSpell;
    grp[3].RequiresInstanceConnection = ((JamCheckFn)(BaseAddress + 0xCAE4CB));

    FILE* dump = nullptr;
    fopen_s(&dump, "dump.txt", "w");
    if (!dump)
        return FALSE;

    for (DWORD opc = 0; opc < 0x1FFF; ++opc)
    {
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

    // don't stay loaded, work was already done.
    // yes, this is horrible
    return FALSE;
}

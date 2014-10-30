
#include "ProcessTools.h"
#include <cstdio>
#include <TlHelp32.h>

HANDLE ProcessTools::GetHandleByName(TCHAR* name, DWORD_PTR* baseAddress, DWORD build, bool log, FileVersionInfo* versionInfo)
{
    PROCESSENTRY32 entry;
    entry.dwSize = sizeof(PROCESSENTRY32);

    HANDLE snapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, NULL);

    if (!Process32First(snapshot, &entry))
    {
        CloseHandle(snapshot);
        if (log)
            _tprintf(_T("Cannot find any process in system!\n"));
        return INVALID_HANDLE_VALUE;
    }

    HANDLE process = INVALID_HANDLE_VALUE;
    do
    {
        if (!_tcsicmp(entry.szExeFile, name))
        {
            process = OpenProcess(PROCESS_VM_OPERATION | PROCESS_VM_READ | PROCESS_VM_WRITE, FALSE, entry.th32ProcessID);
            if (process)
            {
                if (baseAddress || build)
                {
                    MODULEENTRY32 module;
                    module.dwSize = sizeof(MODULEENTRY32);

                    HANDLE moduleSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPMODULE, entry.th32ProcessID);
                    if (Module32First(moduleSnapshot, &module))
                    {
                        if (baseAddress)
                            *baseAddress = (DWORD)module.modBaseAddr;

                        if (build)
                        {
                            FileVersionInfo info;
                            GetFileVersion(module.szExePath, &info);
                            if (info.FilePrivatePart != build)
                            {
                                CloseHandle(moduleSnapshot);
                                CloseHandle(process);
                                process = INVALID_HANDLE_VALUE;
                                continue;
                            }

                            *versionInfo = info;
                        }
                    }

                    CloseHandle(moduleSnapshot);
                }

                break;
            }
        }
    } while (Process32Next(snapshot, &entry));

    CloseHandle(snapshot);

    if (process == INVALID_HANDLE_VALUE)
    {
        if (log)
            _tprintf(_T("Process with name %s not running.\n"), name);
        return INVALID_HANDLE_VALUE;
    }

    if (!process)
    {
        if (log)
            _tprintf(_T("Process with name %s is couldn't be opened.\n"), name);
        return INVALID_HANDLE_VALUE;
    }

    return process;
}

void ProcessTools::GetFileVersion(TCHAR* path, FileVersionInfo* info)
{
    DWORD size = GetFileVersionInfoSize(path, NULL);
    if (!size)
    {
        _tprintf(_T("Error in GetFileVersionInfoSize: %d\n"), GetLastError());
        return;
    }

    BYTE* buffer = new BYTE[size];
    if (!GetFileVersionInfo(path, NULL, size, buffer))
    {
        _tprintf(_T("Error in GetFileVersionInfo: %d\n"), GetLastError());
        delete[] buffer;
        return;
    }

    VS_FIXEDFILEINFO* fileInfo;
    UINT fileInfoSize;
    if (!VerQueryValue(buffer, _T("\\"), (LPVOID*)&fileInfo, &fileInfoSize))
    {
        _tprintf(_T("Error in VerQueryValue: %d\n"), GetLastError());
        delete[] buffer;
        return;
    }

    info->Init(fileInfo->dwFileVersionMS, fileInfo->dwFileVersionLS);
    delete[] buffer;
}


#ifndef ProcessTools_h__
#define ProcessTools_h__

#include <Windows.h>
#include <tchar.h>

struct FileVersionInfo
{
    WORD FileMajorPart;
    WORD FileMinorPart;
    WORD FileBuildPart;
    WORD FilePrivatePart;

    void Init(DWORD ms, DWORD ls)
    {
        FileMajorPart = HIWORD(ms);
        FileMinorPart = LOWORD(ms);
        FileBuildPart = HIWORD(ls);
        FilePrivatePart = LOWORD(ls);
    }
};

namespace ProcessTools
{
    HANDLE GetHandleByName(TCHAR* name, DWORD_PTR* baseAddress, DWORD build, bool log, FileVersionInfo* versionInfo);
    void GetFileVersion(TCHAR* path, FileVersionInfo* info);
}

#endif // ProcessTools_h__

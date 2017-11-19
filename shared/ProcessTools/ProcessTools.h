
#ifndef ProcessTools_h__
#define ProcessTools_h__

#include "Cache.h"
#include <Windows.h>
#include <tchar.h>
#include <memory>
#include <string>
#include <vector>

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
class Process
{
public:
    Process(HANDLE hnd, std::uintptr_t baseAddress, FileVersionInfo const& ver) : _handle(hnd), _baseAddress(baseAddress), _version(ver) { }
    ~Process()
    {
        CloseHandle(_handle);
    }

    template<typename T>
    T const& Read(std::uintptr_t address, bool relative = true)
    {
        if (relative)
            address += _baseAddress;

        if (T const* value = _dataCache.Retrieve<T>(address))
            return *value;

        T uncached;
        ReadProcessMemory(_handle, reinterpret_cast<LPCVOID>(address), &uncached, sizeof(T), nullptr);
        return _dataCache.Store(address, uncached);
    }

    template<typename T>
    std::vector<T> ReadArray(std::uintptr_t address, std::size_t arraySize, bool relative = true) const
    {
        if (relative)
            address += _baseAddress;

        std::vector<T> data(arraySize);
        ReadProcessMemory(_handle, reinterpret_cast<LPCVOID>(address), data.data(), sizeof(T) * arraySize, nullptr);
        return data;
    }

    // pointer wrappers
    template<typename T>
    T const& Read(void const* address) { return Read<T>(reinterpret_cast<std::uintptr_t>(address), false); }

    template<typename T>
    std::vector<T> ReadArray(void const* address, std::size_t arraySize) { return ReadArray<T>(reinterpret_cast<std::uintptr_t>(address), arraySize, false); }

    bool IsValidAddress(void const* address)
    {
        MEMORY_BASIC_INFORMATION mbi;
        if (!VirtualQueryEx(_handle, address, &mbi, sizeof(mbi)))
            return false;

        return mbi.Protect != PAGE_NOACCESS;
    }

    FileVersionInfo const& GetFileVersionInfo() const { return _version; }

private:
    HANDLE _handle;
    std::uintptr_t _baseAddress;
    FileVersionInfo _version;
    Cache<256> _dataCache;
    Cache<1024> _stringCache;
};

template<>
std::string const& Process::Read<std::string>(std::uintptr_t address, bool relative);

namespace ProcessTools
{
    std::shared_ptr<Process> Open(TCHAR* name, DWORD build, bool log);

    HANDLE GetHandleByName(TCHAR* name, DWORD_PTR* baseAddress, DWORD build, bool log, FileVersionInfo* versionInfo);
    void GetFileVersion(TCHAR* path, FileVersionInfo* info);
}

#endif // ProcessTools_h__

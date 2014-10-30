#pragma once

#include <Windows.h>
#include <unordered_set>
#include <type_traits>
#include "ProcessTools/ProcessTools.h"
#include "UpdateFieldDumper.h"

class UpdateFieldDumper;

template<class T>
UpdateFieldDumper* Create(HANDLE source, Data* input, FileVersionInfo const& version)
{
    return new T(source, input, version);
}

typedef UpdateFieldDumper*(*FactoryMethod)(HANDLE, Data*, FileVersionInfo const&);

class DumperFactory
{
public:

    template<class T>
    void Register()
    {
        static_assert(std::is_base_of<UpdateFieldDumper, T>::value, "T must be a subclass of UpdateFieldDumper");
        _creators.insert(&Create<T>);
    }

    std::unordered_set<UpdateFieldDumper*> CreateDumpers(HANDLE source, Data* input, FileVersionInfo const& version) const;

private:
    std::unordered_set<FactoryMethod> _creators;
};

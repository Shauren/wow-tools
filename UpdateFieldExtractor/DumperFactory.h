#pragma once

#include <Windows.h>
#include <unordered_set>
#include <type_traits>
#include "ProcessTools/ProcessTools.h"
#include "UpdateFieldDumper.h"

class UpdateFieldDumper;

template<class T>
std::unique_ptr<UpdateFieldDumper> Create(std::shared_ptr<Data> input)
{
    return std::make_unique<T>(input);
}

typedef std::unique_ptr<UpdateFieldDumper>(*FactoryMethod)(std::shared_ptr<Data>);

class DumperFactory
{
public:

    template<class T>
    void Register()
    {
        static_assert(std::is_base_of<UpdateFieldDumper, T>::value, "T must be a subclass of UpdateFieldDumper");
        _creators.insert(&Create<T>);
    }

    std::unordered_set<std::unique_ptr<UpdateFieldDumper>> CreateDumpers(std::shared_ptr<Data> input) const;

private:
    std::unordered_set<FactoryMethod> _creators;
};

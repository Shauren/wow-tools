
#include "DumperFactory.h"

std::unordered_set<UpdateFieldDumper*> DumperFactory::CreateDumpers(HANDLE source, Data* input, FileVersionInfo const& version) const
{
    std::unordered_set<UpdateFieldDumper*> dumpers;
    for (std::unordered_set<FactoryMethod>::iterator itr = _creators.begin(); itr != _creators.end(); ++itr)
        dumpers.insert((*itr)(source, input, version));

    return std::move(dumpers);
}

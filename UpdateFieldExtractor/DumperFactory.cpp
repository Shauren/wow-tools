
#include "DumperFactory.h"

std::unordered_set<std::unique_ptr<UpdateFieldDumper>> DumperFactory::CreateDumpers(std::shared_ptr<Data> input) const
{
    std::unordered_set<std::unique_ptr<UpdateFieldDumper>> dumpers;
    for (std::unordered_set<FactoryMethod>::iterator itr = _creators.begin(); itr != _creators.end(); ++itr)
        dumpers.insert((*itr)(input));

    return std::move(dumpers);
}

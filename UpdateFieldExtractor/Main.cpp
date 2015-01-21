
#include <tchar.h>
#include "DumperFactory.h"
#include "ProcessTools/ProcessTools.h"
#include "DumperFormats/CppUpdateFieldDumper.h"
#include "DumperFormats/CsUpdateFieldDumper.h"

int main()
{
    std::shared_ptr<Process> wow = ProcessTools::Open(_T("Wow.exe"), 19342, true);
    if (!wow)
        return 1;

    DumperFactory factory;
    factory.Register<CppUpdateFieldDumper>();
    factory.Register<CsUpdateFieldDumper>();

    std::shared_ptr<Data> data = std::make_shared<Data>(wow);
    std::unordered_set<std::unique_ptr<UpdateFieldDumper>> dumpers = factory.CreateDumpers(data);
    for (auto& dumper : dumpers)
        dumper->Dump();
}

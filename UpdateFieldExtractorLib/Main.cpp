
#include "Export.h"
#include <tchar.h>
#include "DumperFactory.h"
#include "ProcessTools/ProcessTools.h"
#include "DumperFormats/CppUpdateFieldDumper.h"
#include "DumperFormats/CsUpdateFieldDumper.h"
#include "DumperFormats/StructureUpdateFieldDumper.h"

void Extract(UpdateFieldOffsets const* offsets)
{
    std::shared_ptr<Process> wow = ProcessTools::Open(_T("Wow.exe"), 20444, true);
    if (!wow)
        return;

    DumperFactory factory;
    factory.Register<CppUpdateFieldDumper>();
    factory.Register<CsUpdateFieldDumper>();
    factory.Register<StructureUpdateFieldDumper>();

    std::shared_ptr<Data> data = std::make_shared<Data>(wow, offsets);
    std::unordered_set<std::unique_ptr<UpdateFieldDumper>> dumpers = factory.CreateDumpers(data);
    for (auto& dumper : dumpers)
        dumper->Dump();
}

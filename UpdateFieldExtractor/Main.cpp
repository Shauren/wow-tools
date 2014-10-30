
#include <tchar.h>
#include "DumperFactory.h"
#include "ProcessTools/ProcessTools.h"
#include "DumperFormats/CppUpdateFieldDumper.h"
#include "DumperFormats/CsUpdateFieldDumper.h"

int main()
{
    FileVersionInfo version;
    HANDLE wow = ProcessTools::GetHandleByName(_T("Wow.exe"), &Offsets::BaseAddress, 19034, true, &version);
    if (wow == INVALID_HANDLE_VALUE)
        return 1;

    Data* data = new Data(wow);

    DumperFactory factory;
    factory.Register<CppUpdateFieldDumper>();
    factory.Register<CsUpdateFieldDumper>();

    std::unordered_set<UpdateFieldDumper*> dumpers = factory.CreateDumpers(wow, data, version);
    for (std::unordered_set<UpdateFieldDumper*>::iterator itr = dumpers.begin(); itr != dumpers.end(); ++itr)
    {
        (*itr)->Dump();
        delete *itr;
    }

    delete data;
    CloseHandle(wow);
}

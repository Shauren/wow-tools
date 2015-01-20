
#ifndef CppUpdateFieldDumper_h__
#define CppUpdateFieldDumper_h__

#include "UpdateFieldDumper.h"

class CppUpdateFieldDumper : public UpdateFieldDumper
{
    enum
    {
        PaddingSize = 55
    };

public:
    CppUpdateFieldDumper(HANDLE source, Data* input, FileVersionInfo const& version) : UpdateFieldDumper(source, input, version, PaddingSize) { }

    ~CppUpdateFieldDumper() { }

    void Dump();

protected:
    void DumpEnum(std::ofstream& file, Enum const& enumData) override;
    void DumpFlags(std::ofstream& file, std::string const& varName, std::vector<UpdateField*> const& fields, std::vector<UpdateFieldSizes> const& counts);
    void DumpDynamicFlags(std::ofstream& file, std::string const& varName, std::vector<DynamicUpdateField*> const& fields, std::vector<UpdateFieldSizes> const& counts);
};

#endif // CppUpdateFieldDumper_h__

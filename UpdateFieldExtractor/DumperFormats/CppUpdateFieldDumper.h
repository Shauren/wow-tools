
#ifndef CppUpdateFieldDumper_h__
#define CppUpdateFieldDumper_h__

#include "UpdateFieldDumper.h"

class CppUpdateFieldDumper : public UpdateFieldDumper
{
public:
    CppUpdateFieldDumper(HANDLE source, Data* input, FileVersionInfo const& version) : UpdateFieldDumper(source, input, version) { }

    ~CppUpdateFieldDumper() { }

    void Dump();

protected:
    void DumpUpdateFields(std::ofstream& file, std::string const& name, UpdateField* data, UpdateFieldSizes count, std::string const& end, std::string const& fieldBase) override;
    void DumpDynamicFields(std::ofstream& file, std::string const& name, DynamicUpdateField* data, UpdateFieldSizes count, std::string const& end, std::string const& fieldBase) override;
    void DumpFlags(std::ofstream& file, std::string const& varName, std::vector<UpdateField*> const& fields, std::vector<UpdateFieldSizes> const& counts);
    void DumpDynamicFlags(std::ofstream& file, std::string const& varName, std::vector<DynamicUpdateField*> const& fields, std::vector<UpdateFieldSizes> const& counts);

private:
    enum
    {
        PaddingSize = 55
    };
};

#endif // CppUpdateFieldDumper_h__

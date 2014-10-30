
#ifndef CsUpdateFieldDumper_h__
#define CsUpdateFieldDumper_h__

#include "UpdateFieldDumper.h"

class CsUpdateFieldDumper : public UpdateFieldDumper
{
public:
    CsUpdateFieldDumper(HANDLE source, Data* input, FileVersionInfo const& version) : UpdateFieldDumper(source, input, version) { }

    ~CsUpdateFieldDumper() { }

    void Dump();

protected:
    void DumpUpdateFields(std::ofstream& file, std::string const& name, UpdateField* data, UpdateFieldSizes count, std::string const& end, std::string const& fieldBase) override;
    void DumpDynamicFields(std::ofstream& file, std::string const& name, DynamicUpdateField* data, UpdateFieldSizes count, std::string const& end, std::string const& fieldBase) override;

private:
    enum
    {
        PaddingSize = 55
    };
};

#endif // CsUpdateFieldDumper_h__


#ifndef CsUpdateFieldDumper_h__
#define CsUpdateFieldDumper_h__

#include "UpdateFieldDumper.h"

class CsUpdateFieldDumper : public UpdateFieldDumper
{
    enum
    {
        PaddingSize = 55
    };

public:
    CsUpdateFieldDumper(HANDLE source, Data* input, FileVersionInfo const& version) : UpdateFieldDumper(source, input, version, PaddingSize) { }

    ~CsUpdateFieldDumper() { }

    void Dump();

protected:
    void DumpEnum(std::ofstream& file, Enum const& enumData) override;
};

#endif // CsUpdateFieldDumper_h__


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
    CsUpdateFieldDumper(std::shared_ptr<Data> input) : UpdateFieldDumper(input, PaddingSize) { }

    ~CsUpdateFieldDumper() { }

    void Dump();

protected:
    void DumpEnum(std::ofstream& file, Outputs const& enumData) override;
};

#endif // CsUpdateFieldDumper_h__


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
    CppUpdateFieldDumper(std::shared_ptr<Data> input) : UpdateFieldDumper(input, PaddingSize) { }

    ~CppUpdateFieldDumper() { }

    void Dump();

protected:
    void DumpEnum(std::ofstream& file, Outputs const& enumData) override;
    void DumpFlags(std::ofstream& file, std::string const& varName, std::vector<std::vector<UpdateField>*> const& fields);
    void DumpDynamicFlags(std::ofstream& file, std::string const& varName, std::vector<std::vector<DynamicUpdateField>*> const& fields);
};

#endif // CppUpdateFieldDumper_h__

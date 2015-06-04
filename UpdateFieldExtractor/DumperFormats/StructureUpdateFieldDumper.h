
#ifndef StructureUpdateFieldDumper_h__
#define StructureUpdateFieldDumper_h__

#include "UpdateFieldDumper.h"

class StructureUpdateFieldDumper : public UpdateFieldDumper
{
public:
    StructureUpdateFieldDumper(std::shared_ptr<Data> input) : UpdateFieldDumper(input, 0) { }

    ~StructureUpdateFieldDumper() { }

    void Dump();

protected:
    void DumpEnum(std::ofstream& file, Outputs const& enumData) override;
};

#endif // StructureUpdateFieldDumper_h__

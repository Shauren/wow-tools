
#ifndef Structure_h__
#define Structure_h__

#include "LanguageConstruct.h"

struct StructureMember
{
    StructureMember(std::uint32_t ordinal, std::string const& type, std::string const& name, std::string const& comment)
        : Offset(ordinal), TypeName(type), ValueName(name), Comment(comment) { }

    std::uint32_t Offset;
    std::string TypeName;
    std::string ValueName;
    std::string Comment;

    bool operator<(StructureMember const& right) const { return Offset < right.Offset; }
};

class Structure : public LanguageConstruct<StructureMember>
{
private:
    std::string _variableName;
};

#endif // Structure_h__

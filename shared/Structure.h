
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
public:
    void SetComment(std::string const& comment) { _comment = comment; }
    void SetValueCommentPadding(std::uint32_t cp) { _valueCommentPadding = cp; }

    std::string const& GetComment() const { return _comment; }
    std::uint32_t GetValueCommentPadding() const { return _valueCommentPadding; }

private:
    std::string _comment;
    std::uint32_t _valueCommentPadding;
};

class CppStruct : public Formatter<Structure>
{
public:
    CppStruct(bool nested) : _nested(nested) { }

    void ProcessDefinition(std::ostream& stream, Structure const& structure, std::uint32_t indent) override
    {
        if (!_nested)
            stream << std::string(indent, ' ');

        if (!structure.GetComment().empty())
            stream << "// " << structure.GetComment() << std::endl << std::string(indent, ' ');

        stream << "struct";
        if (!structure.GetName().empty())
            stream << ' ' << structure.GetName();

        stream << std::endl << std::string(indent, ' ') << '{' << std::endl;
    }

    void ProcessMember(std::ostream& stream, Structure const& structure, Structure::Member const& member, std::uint32_t indent) override
    {
        stream << std::string(indent + 4, ' ') << member.TypeName;
        if (!member.ValueName.empty())
            stream << ' ' << member.ValueName;

        stream << ';';
        if (!member.Comment.empty())
        {
            std::int32_t commentPaddingLength = structure.GetValueCommentPadding();
            --commentPaddingLength; // ;
            commentPaddingLength -= std::int32_t(member.ValueName.length());
            --commentPaddingLength; // space between type and name
            commentPaddingLength -= std::int32_t(member.TypeName.length());
            commentPaddingLength -= indent + 4;
            if (commentPaddingLength > 0)
                stream << std::string(commentPaddingLength, ' ');

            stream << "// " << member.Comment;
        }

        stream << std::endl;
    }

    void ProcessEnd(std::ostream& stream, Structure const& /*structure*/, std::uint32_t indent) override
    {
        stream << std::string(indent, ' ') << '}';

        if (!_nested)
            stream << ';' << std::endl;
    }

protected:
    bool _nested;
};

class CppUnion : public CppStruct
{
public:
    CppUnion(bool nested) : CppStruct(nested) { }

    void ProcessDefinition(std::ostream& stream, Structure const& structure, std::uint32_t indent) override
    {
        if (!_nested)
            stream << std::string(indent, ' ');

        if (!structure.GetComment().empty())
            stream << "// " << structure.GetComment() << std::endl << std::string(indent, ' ');

        stream << "union";
        if (!structure.GetName().empty())
            stream << ' ' << structure.GetName();

        stream << std::endl << std::string(indent, ' ') << '{' << std::endl;
    }
};

#endif // Structure_h__


#ifndef Enum_h__
#define Enum_h__

#include "LanguageConstruct.h"

struct EnumMember
{
    EnumMember(std::uint32_t ordinal, std::string const& name, std::string const& comment)
        : Offset(ordinal), ValueName(name), Value(std::to_string(ordinal)), Comment(comment) { }

    EnumMember(std::uint32_t ordinal, std::string const& value, std::string const& name, std::string const& comment)
        : Offset(ordinal), ValueName(name), Value(value), Comment(comment) { }

    std::uint32_t Offset;
    std::string ValueName;
    std::string Value;
    std::string Comment;

    bool operator<(EnumMember const& right) const { return Offset < right.Offset; }
};

class Enum : public LanguageConstruct<EnumMember>
{
    typedef LanguageConstruct<EnumMember> Base;

public:
    Enum() { }
    Enum(std::string const& name) : Base(name) { }

    void SetPaddingAfterValueName(std::uint32_t padding) { _paddingAfterName = padding; }
    std::uint32_t GetPadding() const { return _paddingAfterName; }

private:
    std::uint32_t _paddingAfterName;
};

class EnumFormatter : public Formatter < Enum >
{
public:
    void ProcessMember(std::ostream& stream, Enum const& enumData, Enum::Member const& member, std::uint32_t indent) override
    {
        stream << std::string(indent + 4, ' ')
            << member.ValueName << std::string(std::max<std::size_t>(enumData.GetPadding() - member.ValueName.length(), 1), ' ')
            << "= " << member.Value << ',';

        if (!member.Comment.empty())
            stream << " // " << member.Comment;

        stream << std::endl;
    }

    void ProcessEnd(std::ostream& stream, std::uint32_t indent) override
    {
        stream << std::string(indent, ' ') << "};" << std::endl;
    }
};

class CsEnum : public EnumFormatter
{
public:
    void ProcessDefinition(std::ostream& stream, std::string const& name, std::uint32_t indent) override
    {
        stream << std::string(indent, ' ')
            << "public enum " << name << std::endl << std::string(indent, ' ')
            << '{' << std::endl;
    }
};

class CppEnum : public EnumFormatter
{
public:
    void ProcessDefinition(std::ostream& stream, std::string const& name, std::uint32_t indent) override
    {
        stream << std::string(indent, ' ')
            << "enum " << name << std::endl << std::string(indent, ' ')
            << '{' << std::endl;
    }
};

#endif // Enum_h__


#ifndef Enum_h__
#define Enum_h__

#include <string>
#include <sstream>
#include <list>
#include <algorithm>
#include <memory>

class Enum
{
public:
    struct Member
    {
        Member(std::uint32_t ordinal, std::string const& name, std::string const& comment)
            : Ordinal(ordinal), Value(std::to_string(ordinal)), ValueName(name), Comment(comment) { }

        Member(std::uint32_t ordinal, std::string const& value, std::string const& name, std::string const& comment)
            : Ordinal(ordinal), Value(value), ValueName(name), Comment(comment) { }

        std::uint32_t Ordinal;
        std::string Value;
        std::string ValueName;
        std::string Comment;

        bool operator<(Member const& right) const { return Ordinal < right.Ordinal; }
    };

    Enum() { }
    Enum(std::string const& name) : _name(name) { }

    void SetName(std::string const& name) { _name = name; }
    void SetPaddingAfterValueName(std::uint32_t padding) { _paddingAfterName = padding; }
    void AddMember(Member&& member) { _members.push_back(std::move(member)); }
    void AddMemberSorted(Member&& member) { _members.push_back(std::move(member)); _members.sort(); }

    std::string const& GetName() const { return _name; }
    std::uint32_t GetPadding() const { return _paddingAfterName; }
    std::list<Member> const& GetMembers() const { return _members; }
    Member const* GetMember(std::string const& memberName) const
    {
        auto itr = std::find_if(_members.begin(), _members.end(), [&memberName](Member const& member)
        {
            return member.ValueName == memberName;
        });

        if (itr != _members.end())
            return &(*itr);

        return nullptr;
    }

private:
    std::string _name;
    std::list<Member> _members;
    std::uint32_t _paddingAfterName;
};

class Language
{
public:
    virtual std::string FormatDefinition(std::string const& enumName, std::uint32_t enumIndent) = 0;
    std::string FormatMember(Enum::Member const& member, std::uint32_t enumIndent, std::uint32_t valueIndent)
    {
        if (member.Comment.empty())
            return static_cast<std::ostringstream&>(std::ostringstream() << std::string(enumIndent + 4, ' ')
                << member.ValueName << std::string(std::max<std::size_t>(valueIndent - member.ValueName.length(), 1), ' ')
                << "= " << member.Value << ',' << std::endl).str();

        return static_cast<std::ostringstream&>(std::ostringstream() << std::string(enumIndent + 4, ' ')
            << member.ValueName << std::string(std::max<std::size_t>(valueIndent - member.ValueName.length(), 1), ' ')
            << "= " << member.Value << ", // " << member.Comment << std::endl).str();
    }

    virtual std::string FormatEnd(std::uint32_t enumIndent) = 0;
};

class Cs : public Language
{
public:
    std::string FormatDefinition(std::string const& enumName, std::uint32_t enumIndent) override
    {
        return static_cast<std::ostringstream&>(std::ostringstream() << std::string(enumIndent, ' ')
            << "public enum " << enumName << std::endl << std::string(enumIndent, ' ')
            << '{' << std::endl).str();
    }

    std::string FormatEnd(std::uint32_t enumIndent) override
    {
        return static_cast<std::ostringstream&>(std::ostringstream() << std::string(enumIndent, ' ')
            << "}" << std::endl).str();
    }
};

class Cpp : public Language
{
public:
    std::string FormatDefinition(std::string const& enumName, std::uint32_t enumIndent) override
    {
        return static_cast<std::ostringstream&>(std::ostringstream() << std::string(enumIndent, ' ')
            << "enum " << enumName << std::endl << std::string(enumIndent, ' ')
            << '{' << std::endl).str();
    }

    std::string FormatEnd(std::uint32_t enumIndent) override
    {
        return static_cast<std::ostringstream&>(std::ostringstream() << std::string(enumIndent, ' ')
            << "};" << std::endl).str();
    }
};

class EnumOutput;

inline std::ostream& operator<<(std::ostream& stream, EnumOutput const& enumData);

class EnumOutput
{
    friend std::ostream& operator<<(std::ostream& stream, EnumOutput const& enumData);

public:
    EnumOutput(std::unique_ptr<Language>&& lang, Enum const& enumData, std::uint32_t enumIndent) : _lang(std::move(lang)), _enum(enumData), _enumIndent(enumIndent) { }

    std::ostream& Format(std::ostream& stream) const
    {
        stream << _lang->FormatDefinition(_enum.GetName(), _enumIndent);

        for (Enum::Member const& m : _enum.GetMembers())
            stream << _lang->FormatMember(m, _enumIndent, _enum.GetPadding());

        stream << _lang->FormatEnd(_enumIndent) << std::endl;
        return stream;
    }

private:
    std::unique_ptr<Language> _lang;
    Enum const& _enum;
    std::uint32_t _enumIndent;
};

inline std::ostream& operator<<(std::ostream& stream, EnumOutput const& enumData)
{
    return enumData.Format(stream);
}

#endif // Enum_h__

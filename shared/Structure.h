
#ifndef Structure_h__
#define Structure_h__

#include <string>
#include <sstream>
#include <list>
#include <algorithm>
#include <memory>

struct StructureMember
{
    StructureMember(std::uint32_t ordinal, std::string const& name, std::string const& comment)
        : Offset(ordinal), ValueName(name), Comment(comment) { }

    std::uint32_t Offset;
    std::string ValueName;
    std::string Comment;

    bool operator<(StructureMember const& right) const { return Offset < right.Offset; }
};

template<class MemberType = StructureMember>
class Structure
{
public:
    typedef MemberType Member;

    Structure() { }
    Structure(std::string const& name) : _name(name) { }

    void SetName(std::string const& name) { _name = name; }
    void AddMember(Member&& member) { _members.push_back(std::move(member)); }
    void AddMemberSorted(Member&& member) { _members.push_back(std::move(member)); _members.sort(); }

    std::string const& GetName() const { return _name; }
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
};

template<class StructureType>
class Formatter
{
public:
    virtual void FormatDefinition(std::ostream& stream, std::string const& name, std::uint32_t indent) = 0;
    virtual void FormatMember(std::ostream& stream, StructureType const& structure, typename StructureType::Member const& member, std::uint32_t indent) = 0;
    virtual void FormatEnd(std::ostream& stream, std::uint32_t indent) = 0;
};

template<class StructureType>
class StructureOutput;

template<class StructureType>
inline std::ostream& operator<<(std::ostream& stream, StructureOutput<StructureType> const& structure);

template<class StructureType>
class StructureOutput
{
public:
    StructureOutput(std::unique_ptr<Formatter<StructureType>>&& lang, StructureType const& structure, std::uint32_t indent) : _lang(std::move(lang)), _structure(structure), _indent(indent) { }

    std::ostream& Format(std::ostream& stream) const
    {
        _lang->FormatDefinition(stream, _structure.GetName(), _indent);

        for (StructureType::Member const& m : _structure.GetMembers())
            _lang->FormatMember(stream, _structure, m, _indent);

        _lang->FormatEnd(stream, _indent);
        stream << std::endl;
        return stream;
    }

private:
    std::unique_ptr<Formatter<StructureType>> _lang;
    StructureType const& _structure;
    std::uint32_t _indent;
};

template<class StructureType>
std::ostream& operator<<(std::ostream& stream, StructureOutput<StructureType> const& structure)
{
    return structure.Format(stream);
}

#endif // Structure_h__

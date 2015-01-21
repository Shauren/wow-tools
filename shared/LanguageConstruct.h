
#ifndef LanguageConstruct_h__
#define LanguageConstruct_h__

#include <cstdint>
#include <string>
#include <sstream>
#include <list>
#include <algorithm>
#include <memory>

template<class MemberType>
class LanguageConstruct
{
public:
    typedef MemberType Member;

    LanguageConstruct() { }
    LanguageConstruct(std::string const& name) : _name(name) { }

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

template<class ConstructType>
class Formatter
{
public:
    virtual void ProcessDefinition(std::ostream& stream, ConstructType const& structure, std::uint32_t indent) = 0;
    virtual void ProcessMember(std::ostream& stream, ConstructType const& structure, typename ConstructType::Member const& member, std::uint32_t indent) = 0;
    virtual void ProcessEnd(std::ostream& stream, ConstructType const& structure, std::uint32_t indent) = 0;
};

template<class ConstructType>
class SourceOutput
{
public:
    SourceOutput(std::unique_ptr<Formatter<ConstructType>>&& formatter, ConstructType const& structure, std::uint32_t indent) : _formatter(std::move(formatter)), _structure(structure), _indent(indent) { }

    std::ostream& Format(std::ostream& stream) const
    {
        _formatter->ProcessDefinition(stream, _structure, _indent);

        for (ConstructType::Member const& m : _structure.GetMembers())
            _formatter->ProcessMember(stream, _structure, m, _indent);

        _formatter->ProcessEnd(stream, _structure, _indent);
        return stream;
    }

private:
    std::unique_ptr<Formatter<ConstructType>> _formatter;
    ConstructType const& _structure;
    std::uint32_t _indent;
};

template<class ConstructType>
std::ostream& operator<<(std::ostream& stream, SourceOutput<ConstructType> const& output)
{
    return output.Format(stream);
}

#endif // LanguageConstruct_h__


#ifndef Cache_h__
#define Cache_h__

#include <memory>
#include <cstdint>
#include <unordered_map>
#include <array>

template<std::size_t N>
class Cache
{
    class MemoryObject
    {
    public:
        template<typename T>
        T const* Get() const { return reinterpret_cast<T const*>(This()); }

    protected:
        virtual void const* This() const = 0;
    };

    template<typename V>
    class MemoryObjectHolder : public MemoryObject
    {
    public:
        MemoryObjectHolder(V const& v) : _value(v) { }

    protected:
        void const* This() const override { return &_value; }

    private:
        V _value;
    };

public:
    template<typename V>
    V const& Store(std::uintptr_t key, V const& value)
    {
        // only update stored keys if we dont already have the value
        if (!_storage.count(key))
        {
            std::size_t oldest = (_top + 1) % N;
            if (_storage.size() >= N)
            {
                // remove oldest entry
                _storage.erase(_keys[oldest]);
            }

            _keys[_top] = key;
            _top = oldest;
        }

        _storage[key] = std::make_unique<MemoryObjectHolder<V>>(value);
        return *_storage[key]->Get<V>();
    }

    template<typename V>
    V const* Retrieve(std::uintptr_t key) const
    {
        auto itr = _storage.find(key);
        if (itr == _storage.end())
            return nullptr;

        return itr->second->Get<V>();
    }


private:
    std::unordered_map<std::uintptr_t, std::unique_ptr<MemoryObject>> _storage;
    std::array<std::uintptr_t, N> _keys; // Circular buffer of stored keys, top = current, top+1 = oldest
    std::size_t _top = 0;
};

#endif // Cache_h__

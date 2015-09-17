
#ifndef Export_h__
#define Export_h__

#include <cstdint>

struct UpdateFieldOffsets
{
    std::uintptr_t ObjectFields;
    std::uintptr_t ObjectDynamicFields;
    std::uint32_t ObjectCount;
    std::uint32_t ObjectDynamicCount;

    std::uintptr_t ItemFields;
    std::uintptr_t ItemDynamicFields;
    std::uint32_t ItemCount;
    std::uint32_t ItemDynamicCount;

    std::uintptr_t ContainerFields;
    std::uintptr_t ContainerDynamicFields;
    std::uint32_t ContainerCount;
    std::uint32_t ContainerDynamicCount;

    std::uintptr_t UnitFields;
    std::uintptr_t UnitDynamicFields;
    std::uint32_t UnitCount;
    std::uint32_t UnitDynamicCount;

    std::uintptr_t PlayerFields;
    std::uintptr_t PlayerDynamicFields;
    std::uint32_t PlayerCount;
    std::uint32_t PlayerDynamicCount;

    std::uintptr_t GameObjectFields;
    std::uintptr_t GameObjectDynamicFields;
    std::uint32_t GameObjectCount;
    std::uint32_t GameObjectDynamicCount;

    std::uintptr_t DynamicObjectFields;
    std::uintptr_t DynamicObjectDynamicFields;
    std::uint32_t DynamicObjectCount;
    std::uint32_t DynamicObjectDynamicCount;

    std::uintptr_t CorpseFields;
    std::uintptr_t CorpseDynamicFields;
    std::uint32_t CorpseCount;
    std::uint32_t CorpseDynamicCount;

    std::uintptr_t AreaTriggerFields;
    std::uintptr_t AreaTriggerDynamicFields;
    std::uint32_t AreaTriggerCount;
    std::uint32_t AreaTriggerDynamicCount;

    std::uintptr_t SceneObjectFields;
    std::uintptr_t SceneObjectDynamicFields;
    std::uint32_t SceneObjectCount;
    std::uint32_t SceneObjectDynamicCount;

    std::uintptr_t ConversationFields;
    std::uintptr_t ConversationDynamicFields;
    std::uint32_t ConversationCount;
    std::uint32_t ConversationDynamicCount;
};

extern "C" _declspec(dllexport) void _cdecl Extract(UpdateFieldOffsets const*);
typedef decltype(&Extract) ExportFn;

#endif // Export_h__

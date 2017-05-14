
#include "Export.h"
#include <Windows.h>

enum UpdateFieldSizes : std::uint32_t
{
    OBJECT_COUNT                = 12,
    OBJECT_DYNAMIC_COUNT        = 0,
    ITEM_COUNT                  = 73,
    ITEM_DYNAMIC_COUNT          = 4,
    CONTAINER_COUNT             = 145,
    CONTAINER_DYNAMIC_COUNT     = 0,
    UNIT_COUNT                  = 200,
    UNIT_DYNAMIC_COUNT          = 3,
    PLAYER_COUNT                = 4316,
    PLAYER_DYNAMIC_COUNT        = 13,
    GAMEOBJECT_COUNT            = 21,
    GAMEOBJECT_DYNAMIC_COUNT    = 1,
    DYNAMICOBJECT_COUNT         = 9,
    DYNAMICOBJECT_DYNAMIC_COUNT = 0,
    CORPSE_COUNT                = 34,
    CORPSE_DYNAMIC_COUNT        = 0,
    AREATRIGGER_COUNT           = 31,
    AREATRIGGER_DYNAMIC_COUNT   = 0,
    SCENEOBJECT_COUNT           = 7,
    SCENEOBJECT_DYNAMIC_COUNT   = 0,
    CONVERSATION_COUNT          = 1,
    CONVERSATION_DYNAMIC_COUNT  = 2
};

namespace Offsets
{
    std::uintptr_t const ObjectFields = 0x100E5C8;
    std::uintptr_t const ItemFields = 0x100E658;
    std::uintptr_t const ItemDynamicFields = 0x100E9C4;
    std::uintptr_t const ContainerFields = 0x100E9F0;
    std::uintptr_t const UnitFields = 0x100F0C0;
    std::uintptr_t const UnitDynamicFields = 0x100FA20;
    std::uintptr_t const PlayerFields = 0x100FA38;
    std::uintptr_t const PlayerDynamicFields = 0x101C488;
    std::uintptr_t const GameObjectFields = 0x101C4F0;
    std::uintptr_t const GameObjectDynamicFields = 0x101C5EC;
    std::uintptr_t const DynamicObjectFields = 0x101C5F8;
    std::uintptr_t const CorpseFields = 0x101C668;
    std::uintptr_t const AreaTriggerFields = 0x101C800;
    std::uintptr_t const SceneObjectFields = 0x101C978;
    std::uintptr_t const ConversationFields = 0x100E9E4;
    std::uintptr_t const ConversationDynamicFields = 0x101C9CC;
}

int main()
{
    HMODULE dll = LoadLibraryA("UpdateFieldExtractorLib.dll");
    if (!dll)
        return 1;

    ExportFn extract = (ExportFn)GetProcAddress(dll, "Extract");
    if (!extract)
    {
        FreeLibrary(dll);
        return 1;
    }

    UpdateFieldOffsets offsets;
    memset(&offsets, 0, sizeof(offsets));
    offsets.ObjectFields = Offsets::ObjectFields;
    offsets.ObjectCount = OBJECT_COUNT;
    offsets.ItemFields = Offsets::ItemFields;
    offsets.ItemDynamicFields = Offsets::ItemDynamicFields;
    offsets.ItemCount = ITEM_COUNT;
    offsets.ItemDynamicCount = ITEM_DYNAMIC_COUNT;
    offsets.ContainerFields = Offsets::ContainerFields;
    offsets.ContainerCount = CONTAINER_COUNT;
    offsets.UnitFields = Offsets::UnitFields;
    offsets.UnitDynamicFields = Offsets::UnitDynamicFields;
    offsets.UnitCount = UNIT_COUNT;
    offsets.UnitDynamicCount = UNIT_DYNAMIC_COUNT;
    offsets.PlayerFields = Offsets::PlayerFields;
    offsets.PlayerDynamicFields = Offsets::PlayerDynamicFields;
    offsets.PlayerCount = PLAYER_COUNT;
    offsets.PlayerDynamicCount = PLAYER_DYNAMIC_COUNT;
    offsets.GameObjectFields = Offsets::GameObjectFields;
    offsets.GameObjectDynamicFields = Offsets::GameObjectDynamicFields;
    offsets.GameObjectCount = GAMEOBJECT_COUNT;
    offsets.GameObjectDynamicCount = GAMEOBJECT_DYNAMIC_COUNT;
    offsets.DynamicObjectFields = Offsets::DynamicObjectFields;
    offsets.DynamicObjectCount = DYNAMICOBJECT_COUNT;
    offsets.CorpseFields = Offsets::CorpseFields;
    offsets.CorpseCount = CORPSE_COUNT;
    offsets.AreaTriggerFields = Offsets::AreaTriggerFields;
    offsets.AreaTriggerCount = AREATRIGGER_COUNT;
    offsets.SceneObjectFields = Offsets::SceneObjectFields;
    offsets.SceneObjectCount = SCENEOBJECT_COUNT;
    offsets.ConversationFields = Offsets::ConversationFields;
    offsets.ConversationDynamicFields = Offsets::ConversationDynamicFields;
    offsets.ConversationCount = CONVERSATION_COUNT;
    offsets.ConversationDynamicCount = CONVERSATION_DYNAMIC_COUNT;

    extract(&offsets);
    FreeLibrary(dll);
}

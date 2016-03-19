
#include "Export.h"
#include <Windows.h>

enum UpdateFieldSizes : std::uint32_t
{
    OBJECT_COUNT                = 12,
    OBJECT_DYNAMIC_COUNT        = 0, // Doesn't Exist Yet
    ITEM_COUNT                  = 72,
    ITEM_DYNAMIC_COUNT          = 4,
    CONTAINER_COUNT             = 145,
    CONTAINER_DYNAMIC_COUNT     = 0, // Doesn't Exist Yet
    UNIT_COUNT                  = 202,
    UNIT_DYNAMIC_COUNT          = 2,
    PLAYER_COUNT                = 3432,
    PLAYER_DYNAMIC_COUNT        = 10,
    GAMEOBJECT_COUNT            = 21,
    GAMEOBJECT_DYNAMIC_COUNT    = 1,
    DYNAMICOBJECT_COUNT         = 8,
    DYNAMICOBJECT_DYNAMIC_COUNT = 0, // Doesn't Exist Yet
    CORPSE_COUNT                = 34,
    CORPSE_DYNAMIC_COUNT        = 0, // Doesn't Exist Yet
    AREATRIGGER_COUNT           = 27,
    AREATRIGGER_DYNAMIC_COUNT   = 0, // Doesn't Exist Yet
    SCENEOBJECT_COUNT           = 7,
    SCENEOBJECT_DYNAMIC_COUNT   = 0, // Doesn't Exist Yet
    CONVERSATION_COUNT          = 1,
    CONVERSATION_DYNAMIC_COUNT  = 2
};

namespace Offsets
{
    std::uintptr_t const ObjectFields = 0xED9910;
    std::uintptr_t const ItemFields = 0xED99A0;
    std::uintptr_t const ItemDynamicFields = 0xED9D00;
    std::uintptr_t const ContainerFields = 0xED9D20;
    std::uintptr_t const UnitFields = 0xEDA3F8;
    std::uintptr_t const UnitDynamicFields = 0xEDAD70;
    std::uintptr_t const PlayerFields = 0xEDAD80;
    std::uintptr_t const PlayerDynamicFields = 0xEE4E60;
    std::uintptr_t const GameObjectFields = 0xEE4EB0;
    std::uintptr_t const GameObjectDynamicFields = 0xEE4FAC;
    std::uintptr_t const DynamicObjectFields = 0xEE4FB8;
    std::uintptr_t const CorpseFields = 0xEE5018;
    std::uintptr_t const AreaTriggerFields = 0xEE51B0;
    std::uintptr_t const SceneObjectFields = 0xEE52F8;
    std::uintptr_t const ConversationFields = 0xEDA3EC;
    std::uintptr_t const ConversationDynamicFields = 0xEE534C;
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

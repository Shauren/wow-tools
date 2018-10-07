
#include "Export.h"
#include <Windows.h>

enum UpdateFieldSizes : std::uint32_t
{
    OBJECT_COUNT                            = 7,
    OBJECT_DYNAMIC_COUNT                    = 0,
    ITEM_COUNT                              = 73,
    ITEM_DYNAMIC_COUNT                      = 4,
    CONTAINER_COUNT                         = 145,
    CONTAINER_DYNAMIC_COUNT                 = 0,
    AZERITE_EMPOWERED_ITEM_COUNT            = 4,
    AZERITE_EMPOWERED_ITEM_DYNAMIC_COUNT    = 0,
    AZERITE_ITEM_COUNT                      = 6,
    AZERITE_ITEM_DYNAMIC_COUNT              = 0,
    UNIT_COUNT                              = 206,
    UNIT_DYNAMIC_COUNT                      = 3,
    PLAYER_COUNT                            = 1672,
    PLAYER_DYNAMIC_COUNT                    = 1,
    ACTIVE_PLAYER_COUNT                     = 3991,
    ACTIVE_PLAYER_DYNAMIC_COUNT             = 15,
    GAMEOBJECT_COUNT                        = 26,
    GAMEOBJECT_DYNAMIC_COUNT                = 1,
    DYNAMICOBJECT_COUNT                     = 9,
    DYNAMICOBJECT_DYNAMIC_COUNT             = 0,
    CORPSE_COUNT                            = 38,
    CORPSE_DYNAMIC_COUNT                    = 0,
    AREATRIGGER_COUNT                       = 31,
    AREATRIGGER_DYNAMIC_COUNT               = 0,
    SCENEOBJECT_COUNT                       = 7,
    SCENEOBJECT_DYNAMIC_COUNT               = 0,
    CONVERSATION_COUNT                      = 1,
    CONVERSATION_DYNAMIC_COUNT              = 2
};

namespace Offsets
{
    std::uintptr_t const ObjectFields = 0x2464160;
    std::uintptr_t const ItemFields = 0x2464220;
    std::uintptr_t const ItemDynamicFields = 0x2464900;
    std::uintptr_t const ContainerFields = 0x2464940;
    std::uintptr_t const AzeriteEmpoweredItemFields = 0x24656E0;
    std::uintptr_t const AzeriteItemFields = 0x2465740;
    std::uintptr_t const UnitFields = 0x24657D0;
    std::uintptr_t const UnitDynamicFields = 0x2466B20;
    std::uintptr_t const PlayerFields = 0x2466B50;
    std::uintptr_t const PlayerDynamicFields = 0x2470810;
    std::uintptr_t const ActivePlayerFields = 0x2470820;
    std::uintptr_t const ActivePlayerDynamicFields = 0x2487E50;
    std::uintptr_t const GameObjectFields = 0x2487F40;
    std::uintptr_t const GameObjectDynamicFields = 0x24881B0;
    std::uintptr_t const DynamicObjectFields = 0x24881C0;
    std::uintptr_t const CorpseFields = 0x24882A0;
    std::uintptr_t const AreaTriggerFields = 0x2488630;
    std::uintptr_t const SceneObjectFields = 0x2488920;
    std::uintptr_t const ConversationFields = 0x2464208;
    std::uintptr_t const ConversationDynamicFields = 0x24889C8;
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
    offsets.AzeriteEmpoweredItemFields = Offsets::AzeriteEmpoweredItemFields;
    offsets.AzeriteEmpoweredItemCount = AZERITE_EMPOWERED_ITEM_COUNT;
    offsets.AzeriteItemFields = Offsets::AzeriteItemFields;
    offsets.AzeriteItemCount = AZERITE_ITEM_COUNT;
    offsets.UnitFields = Offsets::UnitFields;
    offsets.UnitDynamicFields = Offsets::UnitDynamicFields;
    offsets.UnitCount = UNIT_COUNT;
    offsets.UnitDynamicCount = UNIT_DYNAMIC_COUNT;
    offsets.PlayerFields = Offsets::PlayerFields;
    offsets.PlayerDynamicFields = Offsets::PlayerDynamicFields;
    offsets.PlayerCount = PLAYER_COUNT;
    offsets.PlayerDynamicCount = PLAYER_DYNAMIC_COUNT;
    offsets.ActivePlayerFields = Offsets::ActivePlayerFields;
    offsets.ActivePlayerDynamicFields = Offsets::ActivePlayerDynamicFields;
    offsets.ActivePlayerCount = ACTIVE_PLAYER_COUNT;
    offsets.ActivePlayerDynamicCount = ACTIVE_PLAYER_DYNAMIC_COUNT;
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

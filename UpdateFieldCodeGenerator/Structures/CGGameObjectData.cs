using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    [HasMutableInterface]
    public class CGGameObjectData
    {
        public static readonly ObjectType ObjectType = ObjectType.GameObject;

        public static readonly UpdateField m_displayID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_spellVisualID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_stateSpellVisualID = new UpdateField(typeof(uint), UpdateFieldFlag.None, customFlag: CustomUpdateFieldFlag.ViewerDependent);
        public static readonly UpdateField m_spawnTrackingStateAnimID = new UpdateField(typeof(uint), UpdateFieldFlag.None, customFlag: CustomUpdateFieldFlag.ViewerDependent);
        public static readonly UpdateField m_spawnTrackingStateAnimKitID = new UpdateField(typeof(uint), UpdateFieldFlag.None, customFlag: CustomUpdateFieldFlag.ViewerDependent);
        public static readonly UpdateField m_stateWorldEffectIDsSize = new UpdateField(typeof(BlzVectorField<int>), UpdateFieldFlag.None, typeof(CGGameObjectData).GetField("m_stateWorldEffectIDs", BindingFlags.Static | BindingFlags.Public), customFlag: CustomUpdateFieldFlag.ViewerDependent);
        public static readonly UpdateField m_stateWorldEffectsQuestObjectiveID = new UpdateField(typeof(uint), UpdateFieldFlag.None, customFlag: CustomUpdateFieldFlag.ViewerDependent);
        public static readonly UpdateField m_stateWorldEffectIDs = new UpdateField(typeof(BlzVectorField<uint>), UpdateFieldFlag.None, customFlag: CustomUpdateFieldFlag.ViewerDependent);
        public static readonly UpdateField m_createdBy = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_guildGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_flags = new UpdateField(typeof(uint), UpdateFieldFlag.None, customFlag: CustomUpdateFieldFlag.ViewerDependent);
        public static readonly UpdateField m_parentRotation = new UpdateField(typeof(Quaternion), UpdateFieldFlag.None);
        public static readonly UpdateField m_factionTemplate = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_state = new UpdateField(typeof(sbyte), UpdateFieldFlag.None, customFlag: CustomUpdateFieldFlag.ViewerDependent);
        public static readonly UpdateField m_typeID = new UpdateField(typeof(sbyte), UpdateFieldFlag.None);
        public static readonly UpdateField m_percentHealth = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_artKit = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_enableDoodadSets = new UpdateField(typeof(DynamicUpdateField<int>), UpdateFieldFlag.None);
        public static readonly UpdateField m_customParam = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_level = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_animGroupInstance = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_uiWidgetItemID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_uiWidgetItemQuality = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_uiWidgetItemUnknown1000 = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_worldEffects = new UpdateField(typeof(DynamicUpdateField<int>), UpdateFieldFlag.None);
        public static readonly UpdateField m_assistActionDataExists = new UpdateField(typeof(BlzOptionalField<JamMirrorGameObjectAssistActionData_C>), UpdateFieldFlag.None, typeof(CGGameObjectData).GetField("m_assistActionData", BindingFlags.Static | BindingFlags.Public), bitSize: 1);
        public static readonly UpdateField m_assistActionData = new UpdateField(typeof(BlzOptionalField<JamMirrorGameObjectAssistActionData_C>), UpdateFieldFlag.None);
    }
}

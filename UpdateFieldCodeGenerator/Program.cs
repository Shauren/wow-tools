using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UpdateFieldCodeGenerator.Formats;

namespace UpdateFieldCodeGenerator
{
    public static class Program
    {
        private static readonly Dictionary<ObjectType, ObjectType> _objectTypeInheritance = new Dictionary<ObjectType, ObjectType>
        {
            {ObjectType.Object, ObjectType.Object},
            {ObjectType.Item, ObjectType.Object},
            {ObjectType.Container, ObjectType.Item},
            {ObjectType.AzeriteEmpoweredItem, ObjectType.Item},
            {ObjectType.AzeriteItem, ObjectType.Item},
            {ObjectType.Unit, ObjectType.Object},
            {ObjectType.Player, ObjectType.Unit},
            {ObjectType.ActivePlayer, ObjectType.Player},
            {ObjectType.GameObject, ObjectType.Object},
            {ObjectType.DynamicObject, ObjectType.Object},
            {ObjectType.Corpse, ObjectType.Object},
            {ObjectType.AreaTrigger, ObjectType.Object},
            {ObjectType.SceneObject, ObjectType.Object},
            {ObjectType.Conversation, ObjectType.Object}
        };

        public static void Main()
        {
            var referencedByDict = new Dictionary<Type, List<Type>>();
            var referencesDict = new Dictionary<Type, List<Type>>();
            var structureRefTypes = new Dictionary<Type, StructureReferenceType>();
            var unsortedTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.Namespace == "UpdateFieldCodeGenerator.Structures")
                .ToList();

            foreach (var type in unsortedTypes)
                structureRefTypes[type] = StructureReferenceType.Root;

            foreach (var type in unsortedTypes)
            {
                var allFields = type.GetFields(BindingFlags.Static | BindingFlags.Public)
                    .Where(field => typeof(UpdateField).IsAssignableFrom(field.FieldType))
                    .Select(field => ResolveField(field).Field)
                    .OrderBy(field => field.Order)
                    .Select(field => GetFieldElementType(field.Type, StructureReferenceType.Embedded))
                    .Where(fieldType => fieldType.Item1.Namespace == "UpdateFieldCodeGenerator.Structures");

                foreach (var (fieldType, refType) in allFields)
                {
                    var reference = referencesDict.ComputeIfAbsent(type, k => new List<Type>());
                    if (!reference.Contains(fieldType))
                        reference.Add(fieldType);
                    referencedByDict.ComputeIfAbsent(fieldType, k => new List<Type>()).Add(type);
                    structureRefTypes[fieldType] = refType;
                }
            }

            var typeList = structureRefTypes.Where(kvp => kvp.Value == StructureReferenceType.Root)
                .Select(kvp => kvp.Key)
                .OrderBy(type => GetObjectType(type))
                .ToList();

            foreach (var type in typeList)
                unsortedTypes.Remove(type);

            // add all missed types
            while (unsortedTypes.Count > 0)
            {
                for (var i = 0; i < typeList.Count; ++i)
                {
                    if (referencesDict.TryGetValue(typeList[i], out var referencedTypes))
                    {
                        var insertIndex = i;
                        foreach (var referencedType in referencedTypes)
                        {
                            var unsortedTypeIndex = unsortedTypes.IndexOf(referencedType);
                            if (unsortedTypeIndex == -1)
                                continue;

                            typeList.Insert(insertIndex, referencedType);
                            unsortedTypes.RemoveAt(unsortedTypeIndex);
                            i = 0;
                            break;
                        }
                    }
                }
            }

            if (unsortedTypes.Count > 0)
                typeList.AddRange(unsortedTypes);

            using (var handlers = new UpdateFieldHandlers())
            {
                handlers.BeforeStructures();
                foreach (var dataType in typeList)
                {

                    var objectType = structureRefTypes[dataType] == StructureReferenceType.Root
                        ? GetObjectType(dataType)
                        : GetCommonObjectType(GetObjectTypeReferenceRoots(dataType, referencedByDict).Select(t => GetObjectType(t)).ToList());

                    WriteCreate(dataType, objectType, handlers);
                    WriteUpdate(dataType, objectType, handlers);
                }

                handlers.AfterStructures();
            }
        }

        public static IEnumerable<Type> GetObjectTypeReferenceRoots(Type key, Dictionary<Type, List<Type>> dict)
        {
            if (dict.ContainsKey(key))
                return dict[key].SelectMany(v => GetObjectTypeReferenceRoots(v, dict));

            return Enumerable.Repeat(key, 1);
        }

        public static ObjectType GetCommonObjectType(List<ObjectType> types)
        {
            if (types.Count == 1)
                return types[0];

            var inheritanceDepths = types.Select(objectType => GetInheritanceDepth(objectType)).ToList();
            var minDepth = inheritanceDepths.Min();

            // normalize all types to same level
            var typesAtDepth = new List<ObjectType>();
            for (var i = 0; i < types.Count; ++i)
            {
                while (inheritanceDepths[i] != minDepth)
                {
                    types[i] = _objectTypeInheritance[types[i]];
                    --inheritanceDepths[i];
                }
            }

            // now go down one level until all elements are equal
            while (types.Distinct().Count() > 1 && minDepth > 0)
            {
                for (var i = 0; i < types.Count; ++i)
                    types[i] = _objectTypeInheritance[types[i]];

                --minDepth;
            }

            return types[0];
        }

        public static int GetInheritanceDepth(ObjectType objectType)
        {
            if (objectType == ObjectType.Object)
                return 0;

            return 1 + GetInheritanceDepth(_objectTypeInheritance[objectType]);
        }

        public static ObjectType GetObjectType(Type type)
        {
            var objectTypeField = type.GetField("ObjectType", BindingFlags.Static | BindingFlags.Public);
            if (objectTypeField != null)
                return (ObjectType)objectTypeField.GetValue(null);

            throw new ArgumentOutOfRangeException(nameof(type));
        }

        private static (Type, StructureReferenceType) GetFieldElementType(Type type, StructureReferenceType structureReferenceType)
        {
            if (type.IsArray)
                return GetFieldElementType(type.GetElementType(), StructureReferenceType.Embedded);

            if (typeof(DynamicUpdateField).IsAssignableFrom(type)
                || typeof(BlzVectorField).IsAssignableFrom(type)
                || typeof(BlzOptionalField).IsAssignableFrom(type))
                return GetFieldElementType(type.GenericTypeArguments[0], StructureReferenceType.Embedded);

            return (type, structureReferenceType);
        }

        private static void WriteCreate(Type dataType, ObjectType objectType, UpdateFieldHandlers fieldHandler)
        {
            // Create
            // * In declaration order
            // * JamDynamicField[].Count (CGActivePlayerData::m_research is a blz::array<JamDynamicField, 1>)
            // * JamDynamicField[].Values (separate loop from count)
            // * JamDynamicField.Values
            // * Bits
            // * JamDynamicField.Values that have bits in them
            var allFields = dataType.GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(field => typeof(UpdateField).IsAssignableFrom(field.FieldType))
                .Select(field => ResolveField(field))
                .GroupBy(field => GetCreateTypeOrder(field.Field))
                .Select(group => (group.Key, group.OrderBy(field => field.Field.Order)))
                .ToDictionary(thing => thing.Key, thing => thing.Item2);

            fieldHandler.OnStructureBegin(dataType, objectType, true, dataType.GetCustomAttribute<HasChangesMaskAttribute>() != null);

            IOrderedEnumerable<(UpdateField Field, string Name)> fieldGroup;
            var firstBunchOfFields = Enumerable.Empty<(UpdateField Field, string Name)>();
            if (allFields.TryGetValue(CreateTypeOrder.Default, out fieldGroup))
                firstBunchOfFields = firstBunchOfFields.Concat(fieldGroup);
            if (allFields.TryGetValue(CreateTypeOrder.JamDynamicField, out fieldGroup))
                firstBunchOfFields = firstBunchOfFields.Concat(fieldGroup);
            if (allFields.TryGetValue(CreateTypeOrder.JamDynamicFieldArray, out fieldGroup))
                firstBunchOfFields = firstBunchOfFields.Concat(fieldGroup);
            if (allFields.TryGetValue(CreateTypeOrder.JamDynamicFieldWithBits, out fieldGroup))
                firstBunchOfFields = firstBunchOfFields.Concat(fieldGroup);

            foreach (var (Field, Name) in firstBunchOfFields.OrderBy(field => field.Field.Order))
            {
                if (typeof(DynamicUpdateField).IsAssignableFrom(Field.Type) || (Field.Type.IsArray && typeof(DynamicUpdateField).IsAssignableFrom(Field.Type.GetElementType())))
                    fieldHandler.OnDynamicFieldSizeCreate(Name, Field);
                else
                    fieldHandler.OnField(Name, Field);
            }

            if (allFields.TryGetValue(CreateTypeOrder.JamDynamicFieldArray, out fieldGroup))
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnField(Name, Field);

            if (allFields.TryGetValue(CreateTypeOrder.JamDynamicField, out fieldGroup))
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnField(Name, Field);

            if (allFields.TryGetValue(CreateTypeOrder.DefaultWithBits, out fieldGroup))
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnField(Name, Field);

            if (allFields.TryGetValue(CreateTypeOrder.ArrayWithBits, out fieldGroup))
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnField(Name, Field);

            if (allFields.ContainsKey(CreateTypeOrder.Bits) || allFields.ContainsKey(CreateTypeOrder.Optional))
            {
                fieldHandler.FinishControlBlocks();
                fieldHandler.FinishBitPack();
            }

            if (allFields.TryGetValue(CreateTypeOrder.Bits, out fieldGroup))
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnField(Name, Field);

            if (allFields.TryGetValue(CreateTypeOrder.Optional, out fieldGroup))
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnOptionalFieldInitCreate(Name, Field);

            if (allFields.TryGetValue(CreateTypeOrder.Optional, out fieldGroup))
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnField(Name, Field);

            if (allFields.TryGetValue(CreateTypeOrder.JamDynamicFieldWithBits, out fieldGroup))
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnField(Name, Field);

            fieldHandler.OnStructureEnd(StructureHasBitFields(dataType), false);
        }

        private static void WriteUpdate(Type dataType, ObjectType objectType, UpdateFieldHandlers fieldHandler)
        {
            // Update - if the value is a complex structure, it gets its own bits for each field
            // -- dynamic only if sizeof(struct) > 32
            // -- arrays always
            // if next array field is same size, merge it into previous loop
            // Order
            // * bools (as bits, not part of masks)
            // * blz::vectors (CGItemData::m_modifiers)
            // * JamDynamicField.Count
            // * JamDynamicField[].Count (CGActivePlayerData::m_research is a blz::array<JamDynamicField, 1>)
            // * JamDynamicField[].Values (separate loop from count)
            // * JamDynamicField.Values
            // * Values
            // * Arrays
            var allFields = dataType.GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(field => typeof(UpdateField).IsAssignableFrom(field.FieldType))
                .Select(field => ResolveField(field))
                .GroupBy(field => GetUpdateTypeOrder(field.Field))
                .Select(group => (group.Key, group.OrderBy(field => field.Field.Order)))
                .ToDictionary(thing => thing.Key, thing => thing.Item2);

            var hasChangesMask = dataType.GetCustomAttribute<HasChangesMaskAttribute>();
            fieldHandler.OnStructureBegin(dataType, objectType, false, hasChangesMask != null);

            IOrderedEnumerable<(UpdateField Field, string Name)> fieldGroup;

            if (allFields.TryGetValue(UpdateTypeOrder.Bits, out fieldGroup))
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnField(Name, Field);

            if (allFields.TryGetValue(UpdateTypeOrder.BlzVector, out fieldGroup))
            {
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnField(Name, Field);

                fieldHandler.FinishControlBlocks();
                fieldHandler.FinishBitPack();
            }

            if (allFields.TryGetValue(UpdateTypeOrder.JamDynamicField, out fieldGroup))
            {
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnDynamicFieldSizeUpdate(Name, Field);

                fieldHandler.FinishControlBlocks();
            }

            if (allFields.TryGetValue(UpdateTypeOrder.JamDynamicFieldArray, out fieldGroup))
            {
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnDynamicFieldSizeUpdate(Name, Field);

                fieldHandler.FinishControlBlocks();
            }

            if (allFields.TryGetValue(UpdateTypeOrder.JamDynamicFieldArray, out fieldGroup))
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnField(Name, Field);

            if (hasChangesMask != null || allFields.ContainsKey(UpdateTypeOrder.Bits) ||
                allFields.ContainsKey(UpdateTypeOrder.JamDynamicField) || allFields.ContainsKey(UpdateTypeOrder.JamDynamicFieldArray))
            {
                fieldHandler.FinishControlBlocks();
                fieldHandler.FinishBitPack();
            }

            if (allFields.TryGetValue(UpdateTypeOrder.JamDynamicField, out fieldGroup))
            {
                foreach (var (Field, Name) in fieldGroup.Where(field => !StructureHasBitFields(field.Field.Type.GenericTypeArguments[0])))
                    fieldHandler.OnField(Name, Field);

                foreach (var (Field, Name) in fieldGroup.Where(field => StructureHasBitFields(field.Field.Type.GenericTypeArguments[0])))
                    fieldHandler.OnField(Name, Field);
            }

            if (allFields.TryGetValue(UpdateTypeOrder.Default, out fieldGroup))
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnField(Name, Field);

            if (allFields.TryGetValue(UpdateTypeOrder.Optional, out fieldGroup))
            {
                fieldHandler.FinishControlBlocks();
                fieldHandler.FinishBitPack();

                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnOptionalFieldInitUpdate(Name, Field);
            }

            if (allFields.TryGetValue(UpdateTypeOrder.Optional, out fieldGroup))
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnField(Name, Field);

            if (allFields.TryGetValue(UpdateTypeOrder.Array, out fieldGroup))
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnField(Name, Field);

            fieldHandler.OnStructureEnd(StructureHasBitFields(dataType),
                hasChangesMask != null && hasChangesMask.ForceMaskMask);
        }

        private static (UpdateField Field, string Name) ResolveField(FieldInfo fieldInfo)
        {
            var field = fieldInfo.GetValue(null) as UpdateField;
            if (field.SizeForField == null)
                return (field, fieldInfo.Name);

            if (typeof(BlzOptionalField).IsAssignableFrom(field.Type))
                return (new UpdateField(typeof(Bits), field.Flag, field.SizeForField, bitSize: 1, order: field.Order), field.SizeForField.Name + ".is_initialized()");

            return (new UpdateField(typeof(uint), field.Flag, field.SizeForField, order: field.Order), field.SizeForField.Name + "{0}size()");
        }

        private static CreateTypeOrder GetCreateTypeOrder(UpdateField fieldType)
        {
            if (typeof(DynamicUpdateField).IsAssignableFrom(fieldType.Type))
                return StructureHasBitFields(fieldType.Type.GenericTypeArguments[0]) ? CreateTypeOrder.JamDynamicFieldWithBits : CreateTypeOrder.JamDynamicField;

            if (typeof(bool).IsAssignableFrom(fieldType.Type))
                return CreateTypeOrder.Bits;

            //if (typeof(BlzOptionalField).IsAssignableFrom(fieldType.Type))
            //    return CreateTypeOrder.Optional;

            if (fieldType.Type.IsArray)
            {
                if (typeof(DynamicUpdateField).IsAssignableFrom(fieldType.Type.GetElementType()))
                    return CreateTypeOrder.JamDynamicFieldArray;

                if (StructureHasBitFields(fieldType.Type.GetElementType()))
                    return CreateTypeOrder.ArrayWithBits;
            }

            if (StructureHasBitFields(fieldType.Type))
                return CreateTypeOrder.DefaultWithBits;

            return CreateTypeOrder.Default;
        }

        private static UpdateTypeOrder GetUpdateTypeOrder(UpdateField fieldType)
        {
            if (fieldType.SizeForField != null)
                return GetUpdateTypeOrder(fieldType.SizeForField.GetValue(null) as UpdateField);

            if (typeof(DynamicUpdateField).IsAssignableFrom(fieldType.Type))
                return UpdateTypeOrder.JamDynamicField;

            //if (typeof(BlzOptionalField).IsAssignableFrom(fieldType.Type))
            //    return UpdateTypeOrder.Optional;

            if (typeof(BlzVectorField).IsAssignableFrom(fieldType.Type))
                return UpdateTypeOrder.BlzVector;

            if (fieldType.Type.IsArray)
                return typeof(DynamicUpdateField).IsAssignableFrom(fieldType.Type.GetElementType()) ? UpdateTypeOrder.JamDynamicFieldArray : UpdateTypeOrder.Array;

            if (typeof(bool).IsAssignableFrom(fieldType.Type))
                return UpdateTypeOrder.Bits;

            return UpdateTypeOrder.Default;
        }

        private static int GetStructureSize(Type type)
        {
            return type.GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(field => typeof(UpdateField).IsAssignableFrom(field.FieldType))
                .Select(field => field.GetValue(null) as UpdateField)
                .Where(field => field.SizeForField == null)
                .Aggregate(0, (total, current) =>
                {
                    return total + (current.BitSize > 0 ? (current.BitSize + 7) / 8 : GetFieldSize(current.Type)) * Math.Max(current.Size, 1);
                });
        }

        private static int GetFieldSize(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Object:
                    if (type.IsArray)
                        return GetFieldSize(type.GetElementType());
                    return GetStructureSize(type);
                case TypeCode.Boolean:
                case TypeCode.SByte:
                case TypeCode.Byte:
                    return 1;
                case TypeCode.Int16:
                case TypeCode.UInt16:
                    return 2;
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Single:
                    return 4;
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return 8;
                default:
                    break;
            }

            throw new ArgumentOutOfRangeException(nameof(type));
        }

        private static bool StructureHasBitFields(Type type)
        {
            return type.GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(field => typeof(UpdateField).IsAssignableFrom(field.FieldType))
                .Select(field => field.GetValue(null) as UpdateField)
                .Count(field => field.Type == typeof(bool) || (field.SizeForField == null && field.BitSize > 0)) > 0;
        }
    }
}

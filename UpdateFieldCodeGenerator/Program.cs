using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UpdateFieldCodeGenerator.Formats;

namespace UpdateFieldCodeGenerator
{
    public static class Program
    {
        public static void Main()
        {
            var referencedByDict = new Dictionary<Type, List<Type>>();
            var referencesDict = new Dictionary<Type, List<Type>>();
            var structureRefTypes = new Dictionary<Type, StructureReferenceType>();
            var unsortedTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.Namespace == "UpdateFieldCodeGenerator.Structures");

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

            foreach (var referencedBy in referencedByDict)
                referencedBy.Value.Sort((a, b) => GetObjectType(a).CompareTo(GetObjectType(b)));

            var typeList = structureRefTypes.Where(kvp => kvp.Value == StructureReferenceType.Root)
                .Select(kvp => kvp.Key)
                .OrderBy(type => GetObjectType(type))
                .SelectMany(type => (referencesDict.ContainsKey(type) ? referencesDict[type] : Enumerable.Empty<Type>()).Concat(Enumerable.Repeat(type, 1)))
                .Distinct();

            using (var handlers = new UpdateFieldHandlers())
            {
                handlers.BeforeStructures();
                foreach (var dataType in typeList)
                {
                    var objectType = GetObjectType(referencedByDict.ContainsKey(dataType) ? referencedByDict[dataType].First() : dataType);

                    WriteCreate(dataType, objectType, handlers);
                    WriteUpdate(dataType, objectType, structureRefTypes[dataType], handlers);
                }

                handlers.AfterStructures();
            }
        }

        private static ObjectType GetObjectType(Type type)
        {
            var objectTypeField = type.GetField("ObjectType", BindingFlags.Static | BindingFlags.Public);
            if (objectTypeField != null)
                return ((ObjectType)objectTypeField.GetValue(null));

            throw new ArgumentOutOfRangeException(nameof(type));
        }

        private static (Type, StructureReferenceType) GetFieldElementType(Type type, StructureReferenceType structureReferenceType)
        {
            if (type.IsArray)
                return GetFieldElementType(type.GetElementType(), StructureReferenceType.Array);

            if (typeof(DynamicUpdateField).IsAssignableFrom(type) || typeof(BlzVectorField).IsAssignableFrom(type))
                return GetFieldElementType(type.GenericTypeArguments[0], StructureReferenceType.DynamicField);

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

            fieldHandler.OnStructureBegin(dataType, objectType, true, false);

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

            if (allFields.TryGetValue(CreateTypeOrder.ArrayWithBits, out fieldGroup))
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnField(Name, Field);

            if (allFields.TryGetValue(CreateTypeOrder.Bits, out fieldGroup))
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnField(Name, Field);

            if (allFields.TryGetValue(CreateTypeOrder.JamDynamicFieldWithBits, out fieldGroup))
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnField(Name, Field);

            fieldHandler.OnStructureEnd(StructureHasBitFields(dataType), false);
        }

        private static void WriteUpdate(Type dataType, ObjectType objectType, StructureReferenceType structureReferenceType, UpdateFieldHandlers fieldHandler)
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

            //          NAME                   | FIELDS | SIZE | HAS ARRAY | HAS BITS | HAS MASK
            //        JamMirrorArtifactPower_C |      3 |    4 |        NO |       NO |       NO
            //          JamMirrorSocketedGem_C |     18 |   40 |       YES |       NO |      YES <-
            //  JamMirrorPassiveSpellHistory_C |      2 |    8 |        NO |       NO |       NO
            //        JamMirrorArenaCooldown_C |      7 |   28 |        NO |       NO |      YES <-
            // JamMirrorCharacterRestriction_C |      4 |   16 |        NO |      YES |       NO
            //   JamMirrorSpellPctModByLabel_C |      4 |   12 |        NO |       NO |       NO
            //  JamMirrorSpellFlatModByLabel_C |      4 |   12 |        NO |       NO |       NO
            //    JamMirrorConversationActor_C |      5 |   32 |        NO |       NO |       NO
            var writeUpdateMasks = structureReferenceType == StructureReferenceType.Root
                || structureReferenceType == StructureReferenceType.Array
                || GetFieldCount(dataType) > 5;
            fieldHandler.OnStructureBegin(dataType, objectType, false, writeUpdateMasks);

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

            var dynamicFields = Enumerable.Empty<(UpdateField Field, string Name)>();
            if (allFields.TryGetValue(UpdateTypeOrder.JamDynamicField, out fieldGroup))
                dynamicFields = dynamicFields.Concat(fieldGroup);
            if (allFields.TryGetValue(UpdateTypeOrder.JamDynamicFieldWithBits, out fieldGroup))
                dynamicFields = dynamicFields.Concat(fieldGroup);

            if (allFields.ContainsKey(UpdateTypeOrder.JamDynamicField) || allFields.ContainsKey(UpdateTypeOrder.JamDynamicFieldWithBits))
            {
                foreach (var (Field, Name) in dynamicFields)
                    fieldHandler.OnDynamicFieldSizeUpdate(Name, Field);

                fieldHandler.FinishControlBlocks();
            }

            if (allFields.TryGetValue(UpdateTypeOrder.JamDynamicFieldArray, out fieldGroup))
            {
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnDynamicFieldSizeUpdate(Name, Field);

                fieldHandler.FinishControlBlocks();
            }

            fieldHandler.FinishBitPack();

            if (allFields.TryGetValue(UpdateTypeOrder.JamDynamicFieldArray, out fieldGroup))
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnField(Name, Field);

            if (allFields.TryGetValue(UpdateTypeOrder.JamDynamicField, out fieldGroup))
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnField(Name, Field);

            if (allFields.TryGetValue(UpdateTypeOrder.JamDynamicFieldWithBits, out fieldGroup))
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnField(Name, Field);

            if (allFields.TryGetValue(UpdateTypeOrder.Default, out fieldGroup))
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnField(Name, Field);

            if (allFields.TryGetValue(UpdateTypeOrder.Array, out fieldGroup))
                foreach (var (Field, Name) in fieldGroup)
                    fieldHandler.OnField(Name, Field);

            fieldHandler.OnStructureEnd(StructureHasBitFields(dataType),
                allFields.ContainsKey(UpdateTypeOrder.Array) || allFields.ContainsKey(UpdateTypeOrder.JamDynamicFieldArray));
        }

        private static (UpdateField Field, string Name) ResolveField(FieldInfo fieldInfo)
        {
            var field = fieldInfo.GetValue(null) as UpdateField;
            if (field.SizeForField == null)
                return (field, fieldInfo.Name);

            return (new UpdateField(typeof(uint), field.Flag, field.SizeForField, order: field.Order), field.SizeForField.Name + "->size()");
        }

        private static CreateTypeOrder GetCreateTypeOrder(UpdateField fieldType)
        {
            if (typeof(DynamicUpdateField).IsAssignableFrom(fieldType.Type))
                return StructureHasBitFields(fieldType.Type.GenericTypeArguments[0]) ? CreateTypeOrder.JamDynamicFieldWithBits : CreateTypeOrder.JamDynamicField;

            if (typeof(bool).IsAssignableFrom(fieldType.Type))
                return CreateTypeOrder.Bits;

            if (fieldType.Type.IsArray)
            {
                if (typeof(DynamicUpdateField).IsAssignableFrom(fieldType.Type.GetElementType()))
                    return CreateTypeOrder.JamDynamicFieldArray;

                if (StructureHasBitFields(fieldType.Type.GetElementType()))
                    return CreateTypeOrder.ArrayWithBits;
            }

            return CreateTypeOrder.Default;
        }

        private static UpdateTypeOrder GetUpdateTypeOrder(UpdateField fieldType)
        {
            if (fieldType.SizeForField != null)
                return GetUpdateTypeOrder(fieldType.SizeForField.GetValue(null) as UpdateField);

            if (typeof(DynamicUpdateField).IsAssignableFrom(fieldType.Type))
                return StructureHasBitFields(fieldType.Type.GenericTypeArguments[0]) ? UpdateTypeOrder.JamDynamicFieldWithBits : UpdateTypeOrder.JamDynamicField;

            if (typeof(BlzVectorField).IsAssignableFrom(fieldType.Type))
                return UpdateTypeOrder.BlzVector;

            if (fieldType.Type.IsArray)
                return typeof(DynamicUpdateField).IsAssignableFrom(fieldType.Type.GetElementType()) ? UpdateTypeOrder.JamDynamicFieldArray : UpdateTypeOrder.Array;

            if (typeof(bool).IsAssignableFrom(fieldType.Type))
                return UpdateTypeOrder.Bits;

            return UpdateTypeOrder.Default;
        }

        private static int GetFieldCount(Type type)
        {
            return type.GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(field => typeof(UpdateField).IsAssignableFrom(field.FieldType))
                .Select(field => field.GetValue(null) as UpdateField)
                .Where(field => field.SizeForField == null)
                .Sum(field => Math.Max(field.Size, 1));
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

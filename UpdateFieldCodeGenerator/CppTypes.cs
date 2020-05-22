using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace UpdateFieldCodeGenerator
{
    public static class CppTypes
    {
        private static readonly ModuleBuilder _moduleBuilder = AssemblyBuilder
            .DefineDynamicAssembly(new AssemblyName("CppTypes"), AssemblyBuilderAccess.ReflectionOnly)
            .DefineDynamicModule("CppTypes.dll");

        private static readonly Dictionary<Type, Type> _csharpToCppTypeDictionary = CreateTypeMapping();

        private static Dictionary<Type, Type> CreateTypeMapping()
        {
            var typeMap = new Dictionary<Type, Type>()
            {
                { typeof(bool), typeof(bool) },
                { typeof(char), typeof(char) },
                { typeof(sbyte), CreateType("int8") },
                { typeof(byte), CreateType("uint8") },
                { typeof(short), CreateType("int16") },
                { typeof(ushort), CreateType("uint16") },
                { typeof(int), CreateType("int32") },
                { typeof(uint), CreateType("uint32") },
                { typeof(long), CreateType("int64") },
                { typeof(ulong), CreateType("uint64") },
                { typeof(float), typeof(float) },
                { typeof(double), typeof(double) },
                { typeof(string), CreateType("std::string") },
                { typeof(BlzVectorField<>), CreateType("std::vector", "T") },
                { typeof(WowGuid), CreateType("ObjectGuid") },
                { typeof(Bits), CreateType("uint32") },
                { typeof(Vector2), CreateType("TaggedPosition", "Tag").MakeGenericType(CreateType("Position::XY")) },
                { typeof(Quaternion), CreateType("QuaternionData") }
            };
            return typeMap;
        }

        public static Type CreateType(string name, params string[] genericTypeArguments)
        {
            if (genericTypeArguments.Length > 0)
                name += $"`{genericTypeArguments.Length}";

            var existingType = _moduleBuilder.GetType(name);
            if (existingType != null)
                return existingType;

            var type = _moduleBuilder.DefineType(name, TypeAttributes.Class);
            if (genericTypeArguments.Length > 0)
                type.DefineGenericParameters(genericTypeArguments);
            return type.CreateType();
        }

        public static Type CreateConstantForTemplateParameter<T>(T value)
        {
            var name = "" + value;
            var existingType = _moduleBuilder.GetType(name);
            if (existingType != null)
                return existingType;

            return _moduleBuilder.DefineType(name, TypeAttributes.Class)
                .CreateType();
        }

        public static Type GetCppType(Type csharpType)
        {
            if (csharpType.IsArray)
                if (_csharpToCppTypeDictionary.TryGetValue(csharpType.GetElementType(), out var arrayElementType))
                    return arrayElementType.MakeArrayType(csharpType.GetArrayRank());

            if (csharpType.IsGenericType)
                if (_csharpToCppTypeDictionary.TryGetValue(csharpType.GetGenericTypeDefinition(), out var genericDefinitionType))
                    return genericDefinitionType.MakeGenericType(csharpType.GenericTypeArguments.Select(genericTypeArgument => GetCppType(genericTypeArgument)).ToArray());

            if (_csharpToCppTypeDictionary.TryGetValue(csharpType, out var cppType))
                return cppType;

            return csharpType;
        }
    }
}

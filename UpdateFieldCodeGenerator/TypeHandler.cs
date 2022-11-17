using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace UpdateFieldCodeGenerator
{
    public static class TypeHandler
    {
        private static readonly CodeDomProvider _codeDomProvider = CodeDomProvider.CreateProvider("CSharp");
        private static readonly ModuleBuilder _moduleBuilder = AssemblyBuilder
            .DefineDynamicAssembly(new AssemblyName("UpdateFieldCodeGeneratorDynamic"), AssemblyBuilderAccess.RunAndCollect)
            .DefineDynamicModule("UpdateFieldCodeGeneratorDynamic.dll");

        public static string GetFriendlyName(Type type)
        {
            return _codeDomProvider.GetTypeOutput(ResolveTypeReference(type));
        }

        private static Type MakeNullable(Type type)
        {
            if (type.IsValueType)
                return typeof(Nullable<>).MakeGenericType(type);
            else
                return type;
        }

        private static CodeTypeReference ResolveTypeReference(Type type)
        {
            if (type.Assembly != Assembly.GetExecutingAssembly() && !type.Assembly.IsDynamic)
                return new CodeTypeReference(type);

            if (type.IsArray)
                return new CodeTypeReference(ResolveTypeReference(type.GetElementType()), type.GetArrayRank());

            return new CodeTypeReference(type.Name, type.GenericTypeArguments.Select(genericType => ResolveTypeReference(genericType)).ToArray());
        }

        public static Type ConvertToInterfaces(Type genericOrArrayType, Func<string, string> nameConverter, bool asNullable)
        {
            if (genericOrArrayType.IsArray)
                return ConvertToInterfaces(genericOrArrayType.GetElementType(), nameConverter, asNullable).MakeArrayType();

            if (genericOrArrayType.IsGenericType && Nullable.GetUnderlyingType(genericOrArrayType) == null)
            {
                var genericTemplate = genericOrArrayType.GetGenericTypeDefinition();
                var genericArguments = genericOrArrayType.GenericTypeArguments
                    .Select(genericArgument => ConvertToInterfaces(genericArgument, nameConverter, asNullable))
                    .ToArray();
                return genericTemplate.MakeGenericType(genericArguments);
            }

            // WowGuid special case, its abstract in WPP
            if (genericOrArrayType.IsAbstract)
                return genericOrArrayType;

            if (genericOrArrayType.Assembly == Assembly.GetExecutingAssembly() || genericOrArrayType.Assembly.IsDynamic)
                return CreateInterfaceTypeFor(genericOrArrayType, nameConverter);

            return asNullable ? MakeNullable(genericOrArrayType) : genericOrArrayType;
        }

        public static Type CreateInterfaceTypeFor(Type type, Func<string, string> nameConverter)
        {
            var interfaceName = type.Namespace + ".I" + (nameConverter != null ? nameConverter(type.Name) : type.Name);
            if (_moduleBuilder.GetType(interfaceName) != null)
                return _moduleBuilder.GetType(interfaceName);

            return _moduleBuilder.DefineType(interfaceName, TypeAttributes.Interface | TypeAttributes.Abstract)
                .CreateType();
        }
    }
}

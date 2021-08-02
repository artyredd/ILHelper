using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ILHelper
{
    public interface IDynamicAssemblyBuilder
    {
        object CreateInstance(string Name);
        IDynamicTypeBuilder CreateType(Func<ModuleBuilder, TypeBuilder> Expression);
        IDynamicTypeBuilder CreateType(string Name, TypeAttributes AccessModifiers = TypeAttributes.Public);
        IDynamicTypeBuilder Builder(string DynamicTypeName);
        Type Type(string DynamicTypeName);
    }
}
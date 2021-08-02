using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ILHelper
{
    public interface IDynamicTypeBuilder
    {
        string Name { get; }

        Type Build();
        IDynamicTypeBuilder CreateField(Type FieldType, string Name, FieldAttributes Attributes = FieldAttributes.Public, object DefaultValue = null);
        IDynamicMethodBuilder CreateMethod(string Name, MethodAttributes AccessModifiers = MethodAttributes.Public);
        IDynamicMethodBuilder CreateMethod(Func<TypeBuilder, MethodBuilder> Expression);
        IDynamicTypeBuilder CreateProperty(Type PropertyType, string Name, object DefaultValue = null);
        FieldInfo Field(string Name);
        MethodInfo Method(string Name);
        IDynamicTypeBuilder Implements(Type Interface);
        PropertyInfo Property(string Name);
        Type Type();
        object CreateInstance();
        IDynamicTypeBuilder Inherits(Type BaseClass);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Extensions
{
    public static class DynamicTypeExtensions
    {
        public static IDynamicTypeBuilder Implements<T>(this IDynamicTypeBuilder builder)
        {
            return builder.Implements(typeof(T));
        }

        public static IDynamicTypeBuilder Inherits<T>(this IDynamicTypeBuilder builder)
        {
            return builder.Inherits(typeof(T));
        }

        public static IDynamicTypeBuilder CreateField<T>(this IDynamicTypeBuilder builder, string Name, object DefaultValue = null)
        {
            return builder.CreateField(typeof(T), Name, FieldAttributes.Public, DefaultValue);
        }

        public static IDynamicTypeBuilder CreateField<T>(this IDynamicTypeBuilder builder, string Name, FieldAttributes Attributes, object DefaultValue = null)
        {
            return builder.CreateField(typeof(T), Name, Attributes, DefaultValue);
        }

        public static IDynamicTypeBuilder CreateProperty<T>(this IDynamicTypeBuilder builder, string Name, object DefaultValue = null)
        {
            return builder.CreateProperty(typeof(T), Name, DefaultValue);
        }
    }
}

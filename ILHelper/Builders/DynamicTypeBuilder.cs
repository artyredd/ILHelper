using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ILHelper.Extensions;

namespace ILHelper
{
    public class DynamicTypeBuilder : BuilderBase, IDynamicTypeBuilder
    {
        public string Name { get; }

        private readonly TypeBuilder Builder;
        private readonly IDynamicAssemblyBuilder AssemblyBuilder;

        private readonly IDictionary<string, object> Members = new Dictionary<string, object>();
        private readonly IDictionary<string, object> DefaultValues = new Dictionary<string, object>();
        public DynamicTypeBuilder(IDynamicAssemblyBuilder assemblyBuilder, TypeBuilder builder)
        {
            this.Name = builder.Name;
            this.Builder = builder;
            this.AssemblyBuilder = assemblyBuilder;
        }

        public Type Build()
        {
            return Builder.CreateType();
        }

        public IDynamicMethodBuilder CreateMethod(string Name, MethodAttributes AccessModifiers = MethodAttributes.Public)
        {
            return CreateMethod(builder => builder.DefineMethod(Name, AccessModifiers, CallingConventions.HasThis));
        }

        public IDynamicMethodBuilder CreateMethod(Func<TypeBuilder, MethodBuilder> Expression)
        {
            MethodBuilder newMethodBuilder = null;

            InvokeCritical(() => newMethodBuilder = Expression(Builder));

            if (newMethodBuilder is null)
            {
                throw Helpers.Exceptions.Generic($"Failed to create method in type {Builder.Name} becuase the MethodBuilder created was null when the expression was evaluated.");
            }

            InvokeCritical(() => Members.Add(newMethodBuilder.Name, newMethodBuilder));

            return new DynamicMethodBuilder(this, newMethodBuilder);
        }

        public IDynamicTypeBuilder CreateField(Type FieldType, string Name, FieldAttributes Attributes = FieldAttributes.Public, object DefaultValue = null)
        {
            InvokeCritical(() =>
            {
                var builder = Builder.DefineField(Name, FieldType, Attributes);

                if (DefaultValue is not null)
                {
                    DefaultValues.Add(Name, DefaultValue);
                }

                Members.Add(Name, builder);
            });

            return this;
        }

        public IDynamicTypeBuilder CreateProperty(Type PropertyType, string Name, object DefaultValue = null)
        {
            // becuase a property is really just two methods in a trench coat we are going to create a private backing field
            string fieldName = $"_{Name}";

            CreateField(PropertyType, fieldName, FieldAttributes.Private);

            // now create the set and get methods each of which set and get the private backing fields
            var setMethod = CreateMethod($"set_{Name}", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig)
                .Returns(typeof(void))
                .Accepts(PropertyType)
                .Emit(OpCodes.Ldarg_0)
                .Emit(OpCodes.Ldarg_1)
                .Emit(OpCodes.Stfld, Field(fieldName))
                .Emit(OpCodes.Ret);

            setMethod.Build();

            // now create the set and get methods each of which set and get the private backing fields
            var getMethod = CreateMethod($"get_{Name}", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig)
                .Returns(PropertyType)
                .Emit(OpCodes.Ldarg_0)
                .Emit(OpCodes.Ldfld, Field(fieldName))
                .Emit(OpCodes.Ret);

            getMethod.Build();

            InvokeCritical(() =>
            {
                // attribute is always has default since we use the set and get methods to access the backing field
                var property = Builder.DefineProperty(Name, PropertyAttributes.HasDefault, PropertyType, null);

                property.SetGetMethod(getMethod.GetBuilder());

                property.SetSetMethod(setMethod.GetBuilder());

                Members.Add(Name, property);

                if (DefaultValue is not null)
                {
                    DefaultValues.Add(Name, DefaultValue);
                }
            });

            return this;
        }

        public IDynamicTypeBuilder Implements(Type Interface)
        {
            InvokeCritical(() =>
            {
                Builder.AddInterfaceImplementation(Interface);
            });

            return this;
        }

        public IDynamicTypeBuilder Inherits(Type BaseClass)
        {
            InvokeCritical(() =>
            {
                Builder.SetParent(BaseClass);
            });

            return this;
        }

        public FieldInfo Field(string Name)
        {
            // if the type has been created we dont have to use the dictionary
            if (Builder.IsCreated())
            {
                return Builder.GetField(Name);
            }

            return Members.FindAndCast<FieldInfo>(Name);
        }

        public PropertyInfo Property(string Name)
        {
            return Members.FindAndCast<PropertyInfo>(Name);
        }

        public MethodInfo Method(string Name)
        {
            // if the type has been created we dont have to use the dictionary
            if (Builder.IsCreated())
            {
                return Builder.GetMethod(Name);
            }

            // since the type has not been compiled yet we should look for the builder itself
            return Members.FindAndCast<MethodInfo>(Name);
        }

        public Type Type()
        {
            return Builder as Type;
        }

        /// <summary>
        /// Creates a new instance of this type
        /// <code>
        /// Caution! This will compile this type and prevent further changes to it's structure
        /// </code>
        /// </summary>
        /// <returns></returns>
        public object CreateInstance()
        {
            // make sure the type has been defined before we can create an instance
            if (Builder.IsCreated() is false)
            {
                Build();
            }

            object instance = AssemblyBuilder.CreateInstance(Name);

            foreach (var item in DefaultValues)
            {
                var field = Field(item.Key);
                if (field != null)
                {
                    field.SetValue(instance, item.Value);
                    continue;
                }

                var property = instance.GetType().GetProperty(item.Key);
                if (property != null)
                {
                    property.SetMethod?.Invoke(instance, new object[] { item.Value });
                    continue;
                }
            }

            return instance;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
#nullable enable
namespace ILHelper
{
    public class DynamicAssemblyBuilder : BuilderBase, IDynamicAssemblyBuilder
    {
        public string Name { get; set; } = Guid.NewGuid().ToString();

        public AssemblyBuilderAccess BuilderAccess { get; set; } = AssemblyBuilderAccess.RunAndCollect;

        public string ModuleName { get; set; } = "<Module>";

        private readonly AssemblyName AssemblyName;
        private readonly AssemblyBuilder AssemblyBuilder;
        private readonly ModuleBuilder ModuleBuilder;
        private readonly IDictionary<string, IDynamicTypeBuilder> Types = new Dictionary<string, IDynamicTypeBuilder>();

        public DynamicAssemblyBuilder(string Name)
        {
            this.Name = Name;

            this.AssemblyName = new(Name);

            this.AssemblyBuilder = System.Reflection.Emit.AssemblyBuilder.DefineDynamicAssembly(this.AssemblyName, BuilderAccess);

            this.ModuleBuilder = AssemblyBuilder.DefineDynamicModule(ModuleName);
        }

        public IDynamicTypeBuilder CreateType(string Name, TypeAttributes AccessModifiers = TypeAttributes.Public)
        {
            return CreateType(builder => builder.DefineType(Name, AccessModifiers));
        }

        public IDynamicTypeBuilder CreateType(Func<ModuleBuilder, TypeBuilder> Expression)
        {
            TypeBuilder? result = null;

            InvokeCritical(() =>
            {
                result = Expression(this.ModuleBuilder!);
            });

            if (result is null)
            {
                throw Helpers.Exceptions.Generic($"Failed to {nameof(CreateType)} for assembly {Name}, result of expression was null.");
            }

            IDynamicTypeBuilder newBuilder = new DynamicTypeBuilder(this, result);

            InvokeCritical(() => Types.Add(newBuilder.Name, newBuilder));

            return newBuilder;
        }

        public object CreateInstance(string Name)
        {
            object? newInstance = AssemblyBuilder.CreateInstance(Name);

            if (newInstance is null)
            {
                throw Helpers.Exceptions.Generic($"Failed to create an instance of type {Name} in the assembly {Name} make sure the type is created first by using {nameof(CreateType)}");
            }

            return newInstance;
        }

        public IDynamicTypeBuilder? Builder(string DynamicTypeName)
        {
            if (Types.TryGetValue(DynamicTypeName, out IDynamicTypeBuilder? builder))
            {
                if (builder is not null)
                {
                    return builder;
                }
            }

            return null;
        }

        public Type? Type(string DynamicTypeName)
        {
            IDynamicTypeBuilder? builder = Builder(DynamicTypeName);

            return builder?.Type();
        }
    }
}

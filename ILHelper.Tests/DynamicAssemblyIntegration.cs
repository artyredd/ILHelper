using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Reflection;
using ILHelper.Extensions;

namespace ILHelper.Tests
{
    public class DynamicAssemblyIntegration
    {
        [Fact]
        public void IntegrateToMethod()
        {
            var assembly = new DynamicAssemblyBuilder("TestAssembly");

            var testClass = assembly.CreateType("TestClass")
                    .CreateMethod("Adder")
                        .Returns<int>()
                        .Accepts<int>()
                        .Emit(OpCodes.Ldarg_1)
                        .Return();

            var instance = testClass.CreateInstance();

            int result = (int)instance.GetType().GetMethod("Adder").Invoke(instance, new object[] { (int)44 });

            int expected = 44;

            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestMethod()
        {
            var asmName = new AssemblyName("TestAssembly");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
            var module = assemblyBuilder.DefineDynamicModule("<Module>");
            var typeBuilder = module.DefineType("TestClass", TypeAttributes.Public);

            var methodBuilder = typeBuilder.DefineMethod("Adder", MethodAttributes.Public, CallingConventions.HasThis, typeof(int), new Type[] { typeof(int) });

            var ilgen = methodBuilder.GetILGenerator();

            ilgen.Emit(OpCodes.Ldarg_1);
            ilgen.Emit(OpCodes.Ret);

            var type = typeBuilder.CreateType();

            var instance = (dynamic)assemblyBuilder.CreateInstance(type.Name);

            //int actual = (dynamic)instance.Adder(44);

            int actual = (int)type.GetMethod("Adder").Invoke(instance, new object[] { 44 });

            Assert.Equal(44, actual);
        }

        [Fact]
        public void TestField()
        {
            var assembly = new DynamicAssemblyBuilder("TestAssembly");

            var testClass = assembly.CreateType("TestClass")
                .CreateField<int>("Value", 44)
                .CreateMethod("Adder")
                .Returns<int>()
                .Accepts<int>()
                .Emit(OpCodes.Ldarg_0)
                .Emit(OpCodes.Ldfld, "Value")
                .Return();

            var instance = testClass.CreateInstance();

            int result = (int)instance.GetType().GetMethod("Adder").Invoke(instance, new object[] { (int)44 });

            int expected = 44;

            Assert.Equal(expected, result);
        }

        [Fact]
        public void DereferenceThrows()
        {
            var assembly = new DynamicAssemblyBuilder("TestAssembly");

            var type = assembly.CreateType("TestClass");

            Assert.Throws<Exception>(() =>
            {
                type.CreateMethod((typeBuilder) =>
                {
                    return null;
                });
            });
        }

        [Fact]
        public void TestDynamicClassParam()
        {
            var assembly = new DynamicAssemblyBuilder("TestAssembly");

            var testClass1 = assembly.CreateType("TestClass")
                .CreateField<int>("Value", 44);

            var testClass2 = assembly.CreateType("TestClass2")
                .CreateMethod("Adder")
                .Returns<int>()
                .Accepts(assembly.Type("TestClass"))
                .Emit(OpCodes.Ldarg_1)
                .Emit(OpCodes.Ldfld, testClass1.Field("Value"))
                .Return();

            var instance = testClass1.CreateInstance();

            var other = testClass2.CreateInstance();

            int result = (int)other.GetType().GetMethod("Adder").Invoke(other, new object[] { instance });

            int expected = 44;

            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestDynamicClassProperty()
        {
            var assembly = new DynamicAssemblyBuilder("TestAssembly");

            var testClass1 = assembly.CreateType("TestClass")
                .CreateField<int>("Value", 44)
                .CreateProperty<int>("Property", 55);

            var testClass2 = assembly.CreateType("TestClass2")
                .CreateMethod("Adder")
                .Returns<int>()
                .Accepts(assembly.Type("TestClass"))
                .Emit(OpCodes.Ldarg_1)
                .Emit(OpCodes.Call, testClass1.Method("get_Property"))
                .Return();

            var instance = testClass1.CreateInstance();

            var other = testClass2.CreateInstance();

            int result = (int)other.GetType().GetMethod("Adder").Invoke(other, new object[] { instance });

            int expected = 55;

            Assert.Equal(expected, result);

            instance.GetType().GetProperty("Property").SetMethod.Invoke(instance, new object[] { 99 });

            result = (int)other.GetType().GetMethod("Adder").Invoke(other, new object[] { instance });

            expected = 99;

            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestInterfaceImplements()
        {
            var assembly = new DynamicAssemblyBuilder("TestAssembly");

            var testClass1 = assembly.CreateType("TestClass")
                .CreateField<int>("Value", 44)
                .CreateProperty<int>("Property", 55)
                .Implements<IBlankInterface>();

            var instance = testClass1.CreateInstance();

            Assert.True(instance is IBlankInterface);
        }

        [Fact]
        public void TestInheritsBaseClass()
        {
            var assembly = new DynamicAssemblyBuilder("TestAssembly");

            var testClass1 = assembly.CreateType("TestClass")
                .CreateField<int>("Value", 44)
                .CreateProperty<int>("Property", 55)
                .Inherits<BlankClass>();

            var instance = testClass1.CreateInstance();

            Assert.True(instance is BlankClass);
        }

        public interface IBlankInterface
        {

        }
        public abstract class BlankClass
        {
            public int BaseClassValue = 13;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static ILHelper.Delegates.Delegates;

namespace ILHelper.Tests
{
    public class DynamicMethodBuilderTests
    {
        [Fact]
        public void BuildsCorrectly()
        {
            var builder = new DynamicMethodBuilder();

            builder.Returns<int>().Accepts<int, int>();

            builder.Emit(OpCodes.Ldarg_0);
            builder.Emit(OpCodes.Ldarg_1);
            builder.Emit(OpCodes.Add);
            builder.Emit(OpCodes.Ret);

            var built = builder.Build().As<Func<int, int, int>>();

            int result = built(44, 44);

            int expected = 88;

            Assert.Equal(expected, result);
        }

        [Fact]
        public void RefReturnAndArgsWorks()
        {
            var builder = new DynamicMethodBuilder();

            builder.Returns(typeof(float).MakeByRefType()).Accepts(typeof(float).MakeByRefType());

            // load the address to the stack
            builder.Emit(OpCodes.Ldarg_0);

            // return the address( ref float)
            builder.Emit(OpCodes.Ret);

            var built = builder.Build().As<FullRefFunc<float, float>>();

            float localFloat = 1f;

            ref float result = ref built(ref localFloat);

            result = 12;

            int expected = 12;

            Assert.Equal(expected, result);
        }
    }
}

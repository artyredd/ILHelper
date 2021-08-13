using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ILHelper.Linter;
using ILHelper.Linter.Extensions;

namespace ILHelper.Tests
{
    public class BitMaskTests
    {
        [Fact]
        public void Test_BitMask()
        {
            var mask = new BitMask();

            mask.AddIndex(TypeCode.Boolean);

            Assert.True(mask.ContainsIndex(TypeCode.Boolean));

            Assert.True(mask.ContainsIndex(3));

            Assert.False(mask.ContainsIndex(1));
        }

        [Fact]
        public void Itegration()
        {
            // binary numeric conversions

            // int
            var mask = new BitMask();

            mask.AddValue(StackVerificationType.Int32);
            mask.AddValue(StackVerificationType.NativeInt);
            mask.AddValue(StackVerificationType.Address);

            _ = mask.Value;
        }

        [Fact]
        public void MaskTableCollectionTEsts()
        {
            // binary numeric conversions

            // int
            var mask = new BitMaskTable<StackVerificationType, Enum>();

            mask.AddValue(StackVerificationType.Int32, OperandType.BinaryNumericAdd, StackVerificationType.Int32);
            mask.AddValue(StackVerificationType.Int32, OperandType.BinaryNumericAdd, StackVerificationType.NativeInt);
            mask.AddValue(StackVerificationType.Int32, OperandType.BinaryNumericAdd, StackVerificationType.Address);

            Assert.True(mask.ContainsValue(StackVerificationType.Int32, OperandType.BinaryNumericAdd, StackVerificationType.Int32));
            Assert.True(mask.ContainsValue(StackVerificationType.Int32, OperandType.BinaryNumericAdd, StackVerificationType.NativeInt));
            Assert.True(mask.ContainsValue(StackVerificationType.Int32, OperandType.BinaryNumericAdd, StackVerificationType.Address));

            Assert.False(mask.ContainsValue(StackVerificationType.Int32, OperandType.BinaryNumericAdd, StackVerificationType.Float64));
            Assert.False(mask.ContainsValue(StackVerificationType.Int32, OperandType.BinaryNumericAdd, StackVerificationType.Pointer));
            Assert.False(mask.ContainsValue(StackVerificationType.Int32, OperandType.BinaryNumericAdd, StackVerificationType.Int64));
            Assert.False(mask.ContainsValue(StackVerificationType.Int32, OperandType.BinaryNumericAdd, StackVerificationType.Object));
        }
    }
}

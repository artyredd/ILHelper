using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILHelper.Linter;
using Xunit;

namespace ILHelper.Tests
{
    public class TypeResultantsTests
    {
        private static StackTypeHandler TypeHandler = new();

        [Theory]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Int32, StackVerificationType.Int32, StackVerificationType.Int32)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Int64, StackVerificationType.Int64, StackVerificationType.Int64)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.NativeInt, StackVerificationType.NativeInt, StackVerificationType.NativeInt)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Float64, StackVerificationType.Float64, StackVerificationType.Float64)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Address, StackVerificationType.Address, StackVerificationType.Address)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Object, StackVerificationType.Object, StackVerificationType.Object)]
        public void Test_SameType(Enum operation, StackVerificationType left, StackVerificationType right, StackVerificationType expected)
        {
            Assert.Equal(expected, TypeHandler.ResultantType(left, right, operation));
        }

        [Theory]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Int32, StackVerificationType.NativeInt, StackVerificationType.NativeInt)]
        [InlineData(OperandType.BinaryNumericAdd, StackVerificationType.Int32, StackVerificationType.Address, StackVerificationType.Address)]
        public void Test_Int32(Enum operation, StackVerificationType left, StackVerificationType right, StackVerificationType expected)
        {
            Assert.Equal(expected, TypeHandler.ResultantType(left, right, operation));
        }

        [Theory]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.NativeInt, StackVerificationType.Int32, StackVerificationType.NativeInt)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.NativeInt, StackVerificationType.NativeInt, StackVerificationType.NativeInt)]
        [InlineData(OperandType.BinaryNumericAdd, StackVerificationType.NativeInt, StackVerificationType.Address, StackVerificationType.Address)]
        public void Test_NativeInt(Enum operation, StackVerificationType left, StackVerificationType right, StackVerificationType expected)
        {
            Assert.Equal(expected, TypeHandler.ResultantType(left, right, operation));
        }

        [Theory]
        [InlineData(OperandType.BinaryNumericAdd, StackVerificationType.Address, StackVerificationType.Int32, StackVerificationType.Address)]
        [InlineData(OperandType.BinaryNumericSub, StackVerificationType.Address, StackVerificationType.Int32, StackVerificationType.Address)]
        [InlineData(OperandType.BinaryNumericAdd, StackVerificationType.Address, StackVerificationType.NativeInt, StackVerificationType.Address)]
        [InlineData(OperandType.BinaryNumericSub, StackVerificationType.Address, StackVerificationType.NativeInt, StackVerificationType.Address)]
        [InlineData(OperandType.BinaryNumericSub, StackVerificationType.Address, StackVerificationType.Address, StackVerificationType.Address)]
        public void Test_Address(Enum operation, StackVerificationType left, StackVerificationType right, StackVerificationType expected)
        {
            Assert.Equal(expected, TypeHandler.ResultantType(left, right, operation));
        }
    }
}

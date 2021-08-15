using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILHelper.Linter;
using Xunit;

namespace ILHelper.Tests
{
    public class TypeCompatibilitiesTests
    {
        [Theory]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Int32, StackVerificationType.Int32, true)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Int32, StackVerificationType.Int64, false)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Int32, StackVerificationType.NativeInt, true)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Int32, StackVerificationType.Float64, false)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Int32, StackVerificationType.Address, false)]
        [InlineData(OperandType.BinaryNumericAdd, StackVerificationType.Int32, StackVerificationType.Address, true)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Int32, StackVerificationType.Object, false)]
        public void Test_Int_BinaryNumeric(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, MethodLinter.TypeCompatibilites.ContainsValue(Left, Operation, Right));
        }

        [Theory]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Int64, StackVerificationType.Int32, false)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Int64, StackVerificationType.Int64, true)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Int64, StackVerificationType.NativeInt, false)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Int64, StackVerificationType.Float64, false)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Int64, StackVerificationType.Address, false)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Int64, StackVerificationType.Object, false)]
        public void Test_Int64_BinaryNumeric(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, MethodLinter.TypeCompatibilites.ContainsValue(Left, Operation, Right));
        }

        [Theory]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.NativeInt, StackVerificationType.Int32, true)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.NativeInt, StackVerificationType.Int64, false)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.NativeInt, StackVerificationType.NativeInt, true)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.NativeInt, StackVerificationType.Float64, false)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.NativeInt, StackVerificationType.Address, false)]
        [InlineData(OperandType.BinaryNumericAdd, StackVerificationType.NativeInt, StackVerificationType.Address, true)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.NativeInt, StackVerificationType.Object, false)]
        public void Test_NativeInt_BinaryNumeric(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, MethodLinter.TypeCompatibilites.ContainsValue(Left, Operation, Right));
        }

        [Theory]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Float64, StackVerificationType.Int32, false)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Float64, StackVerificationType.Int64, false)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Float64, StackVerificationType.NativeInt, false)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Float64, StackVerificationType.Float64, true)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Float64, StackVerificationType.Address, false)]
        [InlineData(OperandType.BinaryNumericAdd, StackVerificationType.Float64, StackVerificationType.Address, false)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Float64, StackVerificationType.Object, false)]
        public void Test_Float64_BinaryNumeric(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, MethodLinter.TypeCompatibilites.ContainsValue(Left, Operation, Right));
        }

        [Theory]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Address, StackVerificationType.Int32, false)]
        [InlineData(OperandType.BinaryNumericAdd, StackVerificationType.Address, StackVerificationType.Int32, true)]
        [InlineData(OperandType.BinaryNumericSub, StackVerificationType.Address, StackVerificationType.Int32, true)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Address, StackVerificationType.Int64, false)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Address, StackVerificationType.NativeInt, false)]
        [InlineData(OperandType.BinaryNumericAdd, StackVerificationType.Address, StackVerificationType.NativeInt, true)]
        [InlineData(OperandType.BinaryNumericSub, StackVerificationType.Address, StackVerificationType.NativeInt, true)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Address, StackVerificationType.Float64, false)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Address, StackVerificationType.Address, false)]
        [InlineData(OperandType.BinaryNumericAdd, StackVerificationType.Address, StackVerificationType.Address, false)]
        [InlineData(OperandType.BinaryNumericSub, StackVerificationType.Address, StackVerificationType.Address, true)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Address, StackVerificationType.Object, false)]
        public void Test_Address_BinaryNumeric(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, MethodLinter.TypeCompatibilites.ContainsValue(Left, Operation, Right));
        }

        [Theory]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Object, StackVerificationType.Int32, false)]
        [InlineData(OperandType.BinaryNumericAdd, StackVerificationType.Object, StackVerificationType.Int32, false)]
        [InlineData(OperandType.BinaryNumericSub, StackVerificationType.Object, StackVerificationType.Int32, false)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Object, StackVerificationType.Int64, false)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Object, StackVerificationType.NativeInt, false)]
        [InlineData(OperandType.BinaryNumericAdd, StackVerificationType.Object, StackVerificationType.NativeInt, false)]
        [InlineData(OperandType.BinaryNumericSub, StackVerificationType.Object, StackVerificationType.NativeInt, false)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Object, StackVerificationType.Float64, false)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Object, StackVerificationType.Address, false)]
        [InlineData(OperandType.BinaryNumericAdd, StackVerificationType.Object, StackVerificationType.Address, false)]
        [InlineData(OperandType.BinaryNumericSub, StackVerificationType.Object, StackVerificationType.Address, false)]
        [InlineData(OperandType.BinaryNumericAny, StackVerificationType.Object, StackVerificationType.Object, false)]
        public void Test_Object_BinaryNumeric(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, MethodLinter.TypeCompatibilites.ContainsValue(Left, Operation, Right));
        }
    }
}

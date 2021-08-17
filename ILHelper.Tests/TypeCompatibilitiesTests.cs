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
        private static StackTypeHandler TypeHandler = new();
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

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
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

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
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

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
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

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
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

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
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

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
        }

        [Theory]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Int32, StackVerificationType.Int32, true)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Int32, StackVerificationType.Int64, false)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Int32, StackVerificationType.NativeInt, true)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Int32, StackVerificationType.Float64, false)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Int32, StackVerificationType.Address, false)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Int32, StackVerificationType.Object, false)]
        public void Test_Object_BinaryNumericComparison_Int32(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
        }

        [Theory]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Int64, StackVerificationType.Int32, false)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Int64, StackVerificationType.Int64, true)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Int64, StackVerificationType.NativeInt, false)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Int64, StackVerificationType.Float64, false)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Int64, StackVerificationType.Address, false)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Int64, StackVerificationType.Object, false)]
        public void Test_Object_BinaryNumericComparison_Int64(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
        }

        [Theory]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.NativeInt, StackVerificationType.Int32, true)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.NativeInt, StackVerificationType.Int64, false)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.NativeInt, StackVerificationType.NativeInt, true)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.NativeInt, StackVerificationType.Float64, false)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.NativeInt, StackVerificationType.Address, false)]
        [InlineData(OperandType.EqualityComparison, StackVerificationType.NativeInt, StackVerificationType.Address, true)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.NativeInt, StackVerificationType.Object, false)]
        public void Test_Object_BinaryNumericComparison_NativeInt(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
        }

        [Theory]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Float64, StackVerificationType.Int32, false)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Float64, StackVerificationType.Int64, false)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Float64, StackVerificationType.NativeInt, false)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Float64, StackVerificationType.Float64, true)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Float64, StackVerificationType.Address, false)]
        [InlineData(OperandType.EqualityComparison, StackVerificationType.Float64, StackVerificationType.Address, false)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Float64, StackVerificationType.Object, false)]
        public void Test_Object_BinaryNumericComparison_Float(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
        }

        [Theory]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Address, StackVerificationType.Int32, false)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Address, StackVerificationType.Int64, false)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Address, StackVerificationType.NativeInt, false)]
        [InlineData(OperandType.EqualityComparison, StackVerificationType.Address, StackVerificationType.NativeInt, true)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Address, StackVerificationType.Float64, false)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Address, StackVerificationType.Address, true)]
        [InlineData(OperandType.EqualityComparison, StackVerificationType.Address, StackVerificationType.Address, false)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Address, StackVerificationType.Object, false)]
        public void Test_Object_BinaryNumericComparison_Address(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
        }

        [Theory]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Object, StackVerificationType.Int32, false)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Object, StackVerificationType.Int64, false)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Object, StackVerificationType.NativeInt, false)]
        [InlineData(OperandType.EqualityComparison, StackVerificationType.Object, StackVerificationType.NativeInt, false)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Object, StackVerificationType.Float64, false)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Object, StackVerificationType.Address, false)]
        [InlineData(OperandType.EqualityComparison, StackVerificationType.Object, StackVerificationType.Address, false)]
        [InlineData(OperandType.BinaryComparison, StackVerificationType.Object, StackVerificationType.Object, false)]
        [InlineData(OperandType.EqualityComparison, StackVerificationType.Object, StackVerificationType.Object, true)]
        public void Test_Object_BinaryNumericComparison_Object(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
        }

        [Theory]
        [InlineData(OperandType.Integer, StackVerificationType.Int32, StackVerificationType.Int32, true)]
        [InlineData(OperandType.Integer, StackVerificationType.Int32, StackVerificationType.Int64, false)]
        [InlineData(OperandType.Integer, StackVerificationType.Int32, StackVerificationType.NativeInt, true)]
        [InlineData(OperandType.Integer, StackVerificationType.Int32, StackVerificationType.Float64, false)]
        [InlineData(OperandType.Integer, StackVerificationType.Int32, StackVerificationType.Address, false)]
        [InlineData(OperandType.Integer, StackVerificationType.Int32, StackVerificationType.Object, false)]
        public void Test_Integer_Int32(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
        }

        [Theory]
        [InlineData(OperandType.Integer, StackVerificationType.Int64, StackVerificationType.Int32, false)]
        [InlineData(OperandType.Integer, StackVerificationType.Int64, StackVerificationType.Int64, true)]
        [InlineData(OperandType.Integer, StackVerificationType.Int64, StackVerificationType.NativeInt, false)]
        [InlineData(OperandType.Integer, StackVerificationType.Int64, StackVerificationType.Float64, false)]
        [InlineData(OperandType.Integer, StackVerificationType.Int64, StackVerificationType.Address, false)]
        [InlineData(OperandType.Integer, StackVerificationType.Int64, StackVerificationType.Object, false)]
        public void Test_Integer_Int64(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
        }

        [Theory]
        [InlineData(OperandType.Integer, StackVerificationType.NativeInt, StackVerificationType.Int32, true)]
        [InlineData(OperandType.Integer, StackVerificationType.NativeInt, StackVerificationType.Int64, false)]
        [InlineData(OperandType.Integer, StackVerificationType.NativeInt, StackVerificationType.NativeInt, true)]
        [InlineData(OperandType.Integer, StackVerificationType.NativeInt, StackVerificationType.Float64, false)]
        [InlineData(OperandType.Integer, StackVerificationType.NativeInt, StackVerificationType.Address, false)]
        [InlineData(OperandType.Integer, StackVerificationType.NativeInt, StackVerificationType.Object, false)]
        public void Test_Integer_Native(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
        }

        [Theory]
        [InlineData(OperandType.Integer, StackVerificationType.Float64, StackVerificationType.Int32, false)]
        [InlineData(OperandType.Integer, StackVerificationType.Float64, StackVerificationType.Int64, false)]
        [InlineData(OperandType.Integer, StackVerificationType.Float64, StackVerificationType.NativeInt, false)]
        [InlineData(OperandType.Integer, StackVerificationType.Float64, StackVerificationType.Float64, false)]
        [InlineData(OperandType.Integer, StackVerificationType.Float64, StackVerificationType.Address, false)]
        [InlineData(OperandType.Integer, StackVerificationType.Float64, StackVerificationType.Object, false)]
        [InlineData(OperandType.Integer, StackVerificationType.Address, StackVerificationType.Int32, false)]
        [InlineData(OperandType.Integer, StackVerificationType.Address, StackVerificationType.Int64, false)]
        [InlineData(OperandType.Integer, StackVerificationType.Address, StackVerificationType.NativeInt, false)]
        [InlineData(OperandType.Integer, StackVerificationType.Address, StackVerificationType.Float64, false)]
        [InlineData(OperandType.Integer, StackVerificationType.Address, StackVerificationType.Address, false)]
        [InlineData(OperandType.Integer, StackVerificationType.Address, StackVerificationType.Object, false)]
        [InlineData(OperandType.Integer, StackVerificationType.Object, StackVerificationType.Int32, false)]
        [InlineData(OperandType.Integer, StackVerificationType.Object, StackVerificationType.Int64, false)]
        [InlineData(OperandType.Integer, StackVerificationType.Object, StackVerificationType.NativeInt, false)]
        [InlineData(OperandType.Integer, StackVerificationType.Object, StackVerificationType.Float64, false)]
        [InlineData(OperandType.Integer, StackVerificationType.Object, StackVerificationType.Address, false)]
        [InlineData(OperandType.Integer, StackVerificationType.Object, StackVerificationType.Object, false)]
        public void Test_Integer_Others(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
        }

        [Theory]
        [InlineData(OperandType.Shift, StackVerificationType.Int32, StackVerificationType.Int32, true)]
        [InlineData(OperandType.Shift, StackVerificationType.Int32, StackVerificationType.Int64, false)]
        [InlineData(OperandType.Shift, StackVerificationType.Int32, StackVerificationType.NativeInt, true)]
        [InlineData(OperandType.Shift, StackVerificationType.Int32, StackVerificationType.Float64, false)]
        [InlineData(OperandType.Shift, StackVerificationType.Int32, StackVerificationType.Address, false)]
        [InlineData(OperandType.Shift, StackVerificationType.Int32, StackVerificationType.Object, false)]
        public void Test_Shift_Int32(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
        }

        [Theory]
        [InlineData(OperandType.Shift, StackVerificationType.Int64, StackVerificationType.Int32, true)]
        [InlineData(OperandType.Shift, StackVerificationType.Int64, StackVerificationType.Int64, false)]
        [InlineData(OperandType.Shift, StackVerificationType.Int64, StackVerificationType.NativeInt, true)]
        [InlineData(OperandType.Shift, StackVerificationType.Int64, StackVerificationType.Float64, false)]
        [InlineData(OperandType.Shift, StackVerificationType.Int64, StackVerificationType.Address, false)]
        [InlineData(OperandType.Shift, StackVerificationType.Int64, StackVerificationType.Object, false)]
        public void Test_Shift_Int64(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
        }

        [Theory]
        [InlineData(OperandType.Shift, StackVerificationType.NativeInt, StackVerificationType.Int32, true)]
        [InlineData(OperandType.Shift, StackVerificationType.NativeInt, StackVerificationType.Int64, false)]
        [InlineData(OperandType.Shift, StackVerificationType.NativeInt, StackVerificationType.NativeInt, true)]
        [InlineData(OperandType.Shift, StackVerificationType.NativeInt, StackVerificationType.Float64, false)]
        [InlineData(OperandType.Shift, StackVerificationType.NativeInt, StackVerificationType.Address, false)]
        [InlineData(OperandType.Shift, StackVerificationType.NativeInt, StackVerificationType.Object, false)]
        public void Test_Shift_Native(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
        }

        [Theory]
        [InlineData(OperandType.Shift, StackVerificationType.Float64, StackVerificationType.Int32, false)]
        [InlineData(OperandType.Shift, StackVerificationType.Float64, StackVerificationType.Int64, false)]
        [InlineData(OperandType.Shift, StackVerificationType.Float64, StackVerificationType.NativeInt, false)]
        [InlineData(OperandType.Shift, StackVerificationType.Float64, StackVerificationType.Float64, false)]
        [InlineData(OperandType.Shift, StackVerificationType.Float64, StackVerificationType.Address, false)]
        [InlineData(OperandType.Shift, StackVerificationType.Float64, StackVerificationType.Object, false)]
        [InlineData(OperandType.Shift, StackVerificationType.Address, StackVerificationType.Int32, false)]
        [InlineData(OperandType.Shift, StackVerificationType.Address, StackVerificationType.Int64, false)]
        [InlineData(OperandType.Shift, StackVerificationType.Address, StackVerificationType.NativeInt, false)]
        [InlineData(OperandType.Shift, StackVerificationType.Address, StackVerificationType.Float64, false)]
        [InlineData(OperandType.Shift, StackVerificationType.Address, StackVerificationType.Address, false)]
        [InlineData(OperandType.Shift, StackVerificationType.Address, StackVerificationType.Object, false)]
        [InlineData(OperandType.Shift, StackVerificationType.Object, StackVerificationType.Int32, false)]
        [InlineData(OperandType.Shift, StackVerificationType.Object, StackVerificationType.Int64, false)]
        [InlineData(OperandType.Shift, StackVerificationType.Object, StackVerificationType.NativeInt, false)]
        [InlineData(OperandType.Shift, StackVerificationType.Object, StackVerificationType.Float64, false)]
        [InlineData(OperandType.Shift, StackVerificationType.Object, StackVerificationType.Address, false)]
        [InlineData(OperandType.Shift, StackVerificationType.Object, StackVerificationType.Object, false)]
        public void Test_Shift_Others(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
        }

        [Theory]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Int32, StackVerificationType.Int32, true)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Int32, StackVerificationType.Int64, false)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Int32, StackVerificationType.NativeInt, true)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Int32, StackVerificationType.Float64, false)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Int32, StackVerificationType.Address, false)]
        [InlineData(OperandType.OverflowAddUnsigned, StackVerificationType.Int32, StackVerificationType.Address, true)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Int32, StackVerificationType.Object, false)]
        public void Test_Overflow_Int32(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
        }

        [Theory]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Int64, StackVerificationType.Int32, false)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Int64, StackVerificationType.Int64, true)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Int64, StackVerificationType.NativeInt, false)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Int64, StackVerificationType.Float64, false)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Int64, StackVerificationType.Address, false)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Int64, StackVerificationType.Object, false)]
        public void Test_Overflow_Int64(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
        }

        [Theory]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.NativeInt, StackVerificationType.Int32, true)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.NativeInt, StackVerificationType.Int64, false)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.NativeInt, StackVerificationType.NativeInt, true)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.NativeInt, StackVerificationType.Float64, false)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.NativeInt, StackVerificationType.Address, false)]
        [InlineData(OperandType.OverflowAddUnsigned, StackVerificationType.NativeInt, StackVerificationType.Address, true)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.NativeInt, StackVerificationType.Object, false)]
        public void Test_Overflow_Native(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
        }

        [Theory]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Float64, StackVerificationType.Int32, false)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Float64, StackVerificationType.Int64, false)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Float64, StackVerificationType.NativeInt, false)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Float64, StackVerificationType.Float64, false)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Float64, StackVerificationType.Address, false)]
        [InlineData(OperandType.OverflowAddUnsigned, StackVerificationType.Float64, StackVerificationType.Address, false)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Float64, StackVerificationType.Object, false)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Object, StackVerificationType.Int32, false)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Object, StackVerificationType.Int64, false)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Object, StackVerificationType.NativeInt, false)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Object, StackVerificationType.Float64, false)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Object, StackVerificationType.Address, false)]
        [InlineData(OperandType.OverflowAddUnsigned, StackVerificationType.Object, StackVerificationType.Address, false)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Object, StackVerificationType.Object, false)]
        public void Test_Overflow_FloatObject(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
        }

        [Theory]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Address, StackVerificationType.Int32, false)]
        [InlineData(OperandType.OverflowAddUnsigned, StackVerificationType.Address, StackVerificationType.Int32, true)]
        [InlineData(OperandType.OverflowSubUnsigned, StackVerificationType.Address, StackVerificationType.Int32, true)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Address, StackVerificationType.Int64, false)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Address, StackVerificationType.NativeInt, false)]
        [InlineData(OperandType.OverflowAddUnsigned, StackVerificationType.Address, StackVerificationType.NativeInt, true)]
        [InlineData(OperandType.OverflowSubUnsigned, StackVerificationType.Address, StackVerificationType.NativeInt, true)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Address, StackVerificationType.Float64, false)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Address, StackVerificationType.Address, false)]
        [InlineData(OperandType.OverflowAddUnsigned, StackVerificationType.Address, StackVerificationType.Address, false)]
        [InlineData(OperandType.OverflowSubUnsigned, StackVerificationType.Address, StackVerificationType.Address, true)]
        [InlineData(OperandType.OverflowArithmetic, StackVerificationType.Address, StackVerificationType.Object, false)]
        public void Test_Overflow_Address(Enum Operation, StackVerificationType Left, StackVerificationType Right, bool Expected)
        {
            _ = new MethodLinter();

            Assert.Equal(Expected, TypeHandler.AreCompatible(Left, Right, Operation));
        }
    }
}

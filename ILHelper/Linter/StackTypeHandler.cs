using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Linter
{
    public class StackTypeHandler
    {
        private readonly BitMaskTable<StackVerificationType, Enum> TypeCompatibilites = new();

        private readonly Dictionary<Enum, Dictionary<StackVerificationType, Dictionary<StackVerificationType, StackVerificationType>>> OperationTypeResults = new();

        public StackTypeHandler()
        {
            InitializeTypeCompatibilities();
            InitializeOperationResultants();
        }

        public StackVerificationType ResultantType(StackVerificationType left, StackVerificationType right, Enum operation)
        {
            // if they are the same the result will always be the same
            if (left == right)
            {
                return left;
            }

            if (OperationTypeResults.ContainsKey(operation))
            {
                if (OperationTypeResults[operation].ContainsKey(left))
                {
                    if (OperationTypeResults[operation][left].ContainsKey(right))
                    {
                        return OperationTypeResults[operation][left][right];
                    }
                }
            }

            throw Helpers.Exceptions.Generic($"Failed to determine what the result type should be when {left} is {operation} with {right}.");
        }

        public bool AreCompatible(StackVerificationType left, StackVerificationType right, Enum Operation)
        {
            return TypeCompatibilites.ContainsValue(left, Operation, right);
        }

        private void InitializeTypeCompatibilities()
        {
            // Binary Numeric Operations

            // int
            TypeCompatibilites.AddValue(StackVerificationType.Int32, OperandType.BinaryNumericAny, StackVerificationType.Int32);
            TypeCompatibilites.AddValue(StackVerificationType.Int32, OperandType.BinaryNumericAny, StackVerificationType.NativeInt);
            TypeCompatibilites.AddValue(StackVerificationType.Int32, OperandType.BinaryNumericAdd, StackVerificationType.Address);

            // int64
            TypeCompatibilites.AddValue(StackVerificationType.Int64, OperandType.BinaryNumericAny, StackVerificationType.Int64);

            // native int
            TypeCompatibilites.AddValue(StackVerificationType.NativeInt, OperandType.BinaryNumericAny, StackVerificationType.Int32);
            TypeCompatibilites.AddValue(StackVerificationType.NativeInt, OperandType.BinaryNumericAny, StackVerificationType.NativeInt);
            TypeCompatibilites.AddValue(StackVerificationType.NativeInt, OperandType.BinaryNumericAdd, StackVerificationType.Address);

            // F
            TypeCompatibilites.AddValue(StackVerificationType.Float64, OperandType.BinaryNumericAny, StackVerificationType.Float64);

            // &
            TypeCompatibilites.AddValue(StackVerificationType.Address, OperandType.BinaryNumericAdd, StackVerificationType.Int32);
            TypeCompatibilites.AddValue(StackVerificationType.Address, OperandType.BinaryNumericSub, StackVerificationType.Int32);
            TypeCompatibilites.AddValue(StackVerificationType.Address, OperandType.BinaryNumericAdd, StackVerificationType.NativeInt);
            TypeCompatibilites.AddValue(StackVerificationType.Address, OperandType.BinaryNumericSub, StackVerificationType.NativeInt);
            TypeCompatibilites.AddValue(StackVerificationType.Address, OperandType.BinaryNumericSub, StackVerificationType.Address);

            // o 
            // none

            // Binary Comparison

            // int
            TypeCompatibilites.AddValue(StackVerificationType.Int32, OperandType.BinaryComparison, StackVerificationType.Int32);
            TypeCompatibilites.AddValue(StackVerificationType.Int32, OperandType.BinaryComparison, StackVerificationType.NativeInt);

            // int64
            TypeCompatibilites.AddValue(StackVerificationType.Int64, OperandType.BinaryComparison, StackVerificationType.Int64);

            // native int
            TypeCompatibilites.AddValue(StackVerificationType.NativeInt, OperandType.BinaryComparison, StackVerificationType.Int32);
            TypeCompatibilites.AddValue(StackVerificationType.NativeInt, OperandType.BinaryComparison, StackVerificationType.NativeInt);
            TypeCompatibilites.AddValue(StackVerificationType.NativeInt, OperandType.EqualityComparison, StackVerificationType.Address);

            // F
            TypeCompatibilites.AddValue(StackVerificationType.Float64, OperandType.BinaryComparison, StackVerificationType.Float64);

            // &
            TypeCompatibilites.AddValue(StackVerificationType.Address, OperandType.EqualityComparison, StackVerificationType.NativeInt);
            TypeCompatibilites.AddValue(StackVerificationType.Address, OperandType.BinaryComparison, StackVerificationType.Address);

            // o
            TypeCompatibilites.AddValue(StackVerificationType.Object, OperandType.EqualityComparison, StackVerificationType.Object);

            // integer
            // int
            TypeCompatibilites.AddValue(StackVerificationType.Int32, OperandType.Integer, StackVerificationType.Int32);
            TypeCompatibilites.AddValue(StackVerificationType.Int32, OperandType.Integer, StackVerificationType.NativeInt);
            //long
            TypeCompatibilites.AddValue(StackVerificationType.Int64, OperandType.Integer, StackVerificationType.Int64);
            // native
            TypeCompatibilites.AddValue(StackVerificationType.NativeInt, OperandType.Integer, StackVerificationType.Int32);
            TypeCompatibilites.AddValue(StackVerificationType.NativeInt, OperandType.Integer, StackVerificationType.NativeInt);

            // shift operations
            // int
            TypeCompatibilites.AddValue(StackVerificationType.Int32, OperandType.Shift, StackVerificationType.Int32);
            TypeCompatibilites.AddValue(StackVerificationType.Int32, OperandType.Shift, StackVerificationType.NativeInt);
            // long
            TypeCompatibilites.AddValue(StackVerificationType.Int64, OperandType.Shift, StackVerificationType.Int32);
            TypeCompatibilites.AddValue(StackVerificationType.Int64, OperandType.Shift, StackVerificationType.NativeInt);
            // native
            TypeCompatibilites.AddValue(StackVerificationType.NativeInt, OperandType.Shift, StackVerificationType.Int32);
            TypeCompatibilites.AddValue(StackVerificationType.NativeInt, OperandType.Shift, StackVerificationType.NativeInt);

            // overflow
            // int
            TypeCompatibilites.AddValue(StackVerificationType.Int32, OperandType.OverflowArithmetic, StackVerificationType.Int32);
            TypeCompatibilites.AddValue(StackVerificationType.Int32, OperandType.OverflowArithmetic, StackVerificationType.NativeInt);
            TypeCompatibilites.AddValue(StackVerificationType.Int32, OperandType.OverflowAddUnsigned, StackVerificationType.Address);
            // long
            TypeCompatibilites.AddValue(StackVerificationType.Int64, OperandType.OverflowArithmetic, StackVerificationType.Int64);
            // native
            TypeCompatibilites.AddValue(StackVerificationType.NativeInt, OperandType.OverflowArithmetic, StackVerificationType.Int32);
            TypeCompatibilites.AddValue(StackVerificationType.NativeInt, OperandType.OverflowArithmetic, StackVerificationType.NativeInt);
            TypeCompatibilites.AddValue(StackVerificationType.NativeInt, OperandType.OverflowAddUnsigned, StackVerificationType.Address);
            // &
            TypeCompatibilites.AddValue(StackVerificationType.Address, OperandType.OverflowAddUnsigned, StackVerificationType.Int32);
            TypeCompatibilites.AddValue(StackVerificationType.Address, OperandType.OverflowSubUnsigned, StackVerificationType.Int32);
            TypeCompatibilites.AddValue(StackVerificationType.Address, OperandType.OverflowAddUnsigned, StackVerificationType.NativeInt);
            TypeCompatibilites.AddValue(StackVerificationType.Address, OperandType.OverflowSubUnsigned, StackVerificationType.NativeInt);
            TypeCompatibilites.AddValue(StackVerificationType.Address, OperandType.OverflowSubUnsigned, StackVerificationType.Address);
        }

        private void InitializeOperationResultants()
        {
            CreateOperandDictionaries();

            // NOTE
            // Same-Type operations never result in a different type, this is checked in ResultantType()

            // Binary Numeric
            // int
            AddOperationResulant(OperandType.BinaryNumericAny, StackVerificationType.Int32, StackVerificationType.NativeInt, StackVerificationType.NativeInt);
            AddOperationResulant(OperandType.BinaryNumericAdd, StackVerificationType.Int32, StackVerificationType.Address, StackVerificationType.Address);

            // native int
            AddOperationResulant(OperandType.BinaryNumericAny, StackVerificationType.NativeInt, StackVerificationType.Int32, StackVerificationType.NativeInt);
            AddOperationResulant(OperandType.BinaryNumericAdd, StackVerificationType.NativeInt, StackVerificationType.Address, StackVerificationType.Address);

            // address
            AddOperationResulant(OperandType.BinaryNumericAdd, StackVerificationType.Address, StackVerificationType.Int32, StackVerificationType.Address);
            AddOperationResulant(OperandType.BinaryNumericAdd, StackVerificationType.Address, StackVerificationType.NativeInt, StackVerificationType.Address);
            AddOperationResulant(OperandType.BinaryNumericSub, StackVerificationType.Address, StackVerificationType.Int32, StackVerificationType.Address);
            AddOperationResulant(OperandType.BinaryNumericSub, StackVerificationType.Address, StackVerificationType.NativeInt, StackVerificationType.Address);
            AddOperationResulant(OperandType.BinaryNumericSub, StackVerificationType.Address, StackVerificationType.Address, StackVerificationType.NativeInt);

            // UNARY does not apply to multi object operations

            // Binary comparisons so not apply as they only result in branching and not a result

            // integer operations
            // int
            AddOperationResulant(OperandType.Integer, StackVerificationType.Int32, StackVerificationType.NativeInt, StackVerificationType.NativeInt);

            // native int
            AddOperationResulant(OperandType.Integer, StackVerificationType.NativeInt, StackVerificationType.Int32, StackVerificationType.NativeInt);

            // shift operations
            // int
            AddOperationResulant(OperandType.Shift, StackVerificationType.Int32, StackVerificationType.NativeInt, StackVerificationType.Int32);
            // int64
            AddOperationResulant(OperandType.Shift, StackVerificationType.Int64, StackVerificationType.NativeInt, StackVerificationType.Int64);
            // native
            AddOperationResulant(OperandType.Shift, StackVerificationType.NativeInt, StackVerificationType.Int32, StackVerificationType.NativeInt);

            // overflow arithmetic
            // int
            AddOperationResulant(OperandType.OverflowArithmetic, StackVerificationType.Int32, StackVerificationType.NativeInt, StackVerificationType.NativeInt);
            AddOperationResulant(OperandType.OverflowAddUnsigned, StackVerificationType.Int32, StackVerificationType.Address, StackVerificationType.Address);

            // native
            AddOperationResulant(OperandType.OverflowArithmetic, StackVerificationType.NativeInt, StackVerificationType.Int32, StackVerificationType.NativeInt);
            AddOperationResulant(OperandType.OverflowAddUnsigned, StackVerificationType.NativeInt, StackVerificationType.Address, StackVerificationType.Address);

            // &
            AddOperationResulant(OperandType.OverflowAddUnsigned, StackVerificationType.Address, StackVerificationType.Int32, StackVerificationType.Address);
            AddOperationResulant(OperandType.OverflowSubUnsigned, StackVerificationType.Address, StackVerificationType.Int32, StackVerificationType.Address);

            AddOperationResulant(OperandType.OverflowAddUnsigned, StackVerificationType.Address, StackVerificationType.NativeInt, StackVerificationType.Address);
            AddOperationResulant(OperandType.OverflowSubUnsigned, StackVerificationType.Address, StackVerificationType.NativeInt, StackVerificationType.Address);

            AddOperationResulant(OperandType.OverflowSubUnsigned, StackVerificationType.Address, StackVerificationType.Address, StackVerificationType.Address);
        }

        private void AddOperationResulant(Enum operation, StackVerificationType left, StackVerificationType right, StackVerificationType result)
        {
            if (OperationTypeResults[operation].ContainsKey(left))
            {
                if (OperationTypeResults[operation][left].ContainsKey(right))
                {
                    OperationTypeResults[operation][left][right] = result;
                }
                else
                {
                    OperationTypeResults[operation][left].Add(right, result);
                }
            }
            else
            {
                var newDict = new Dictionary<StackVerificationType, StackVerificationType>();

                newDict.Add(right, result);

                OperationTypeResults[operation].Add(left, newDict);
            }
        }

        private void CreateOperandDictionaries()
        {
            OperandType[] operands = Enum.GetValues<OperandType>();

            foreach (var item in operands)
            {
                OperationTypeResults.Add(item, new Dictionary<StackVerificationType, Dictionary<StackVerificationType, StackVerificationType>>());
            }
        }
    }
}

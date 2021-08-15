using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Linter
{
    public class MethodLinter
    {
        public static readonly Dictionary<StackVerificationType, Dictionary<OperandType, StackVerificationType>> OperationResultTypes = new();
        public static readonly BitMaskTable<StackVerificationType, Enum> TypeCompatibilites = new();

        public static readonly RuleDictionary<OpCode, StackState> OpRules = new();

        static MethodLinter()
        {
            InitializeOpRules();
            InitializeTypeCompatibilities();
        }


        [DoesNotReturn]
        public bool TryLint(IMethod method, out LintResult result)
        {
            throw new Exception();
            result = new();

            // every method gets its own stack frame
            StackState stack = new();

            foreach (IOpInstruction item in method.Instructions)
            {
                // each instruction 

            }
        }

        private static void InitializeTypeCompatibilities()
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
        }

        private static void InitializeOpRules()
        {

        }
    }
}

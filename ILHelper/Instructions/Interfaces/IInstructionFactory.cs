using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ILHelper
{
    public interface IInstructionFactory
    {
        IOpInstruction Create(OpCode Instruction);
        IOpInstruction Create(OpCode Instruction, FieldInfo Field);
        IOpInstruction Create(OpCode Instruction, MethodInfo Method);
        IOpInstruction Create(OpCode Instruction, Type Type);
    }
}
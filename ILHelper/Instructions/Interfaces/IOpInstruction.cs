using System.Reflection.Emit;

namespace ILHelper
{
    public interface IOpInstruction
    {
        OpCode Instruction { get; }
        void Emit(ILGenerator Generator);
    }
}
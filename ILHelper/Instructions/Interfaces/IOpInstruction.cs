using System.Reflection.Emit;

namespace ILHelper
{
    public interface IOpInstruction
    {
        void Emit(ILGenerator Generator);
    }
}
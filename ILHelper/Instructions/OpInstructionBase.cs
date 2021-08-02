using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper
{
    public class OpInstructionBase : IOpInstruction
    {
        public virtual OpCode Instruction { get; set; }

        public virtual void Emit(ILGenerator Generator)
        {
            Generator.Emit(Instruction);
        }
    }
}

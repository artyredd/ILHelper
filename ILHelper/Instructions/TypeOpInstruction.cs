using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper
{
    public class TypeOpInstruction : OpInstructionBase
    {
        public TypeOpInstruction(OpCode Instruction, Type type)
        {
            Type = type;
            base.Instruction = Instruction;
        }

        public Type Type { get; set; }

        public override void Emit(ILGenerator Generator)
        {
            Generator.Emit(Instruction, Type);
        }
    }
}

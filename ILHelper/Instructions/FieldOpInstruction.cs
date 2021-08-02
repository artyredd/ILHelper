using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper
{
    public class FieldOpInstruction : OpInstructionBase
    {
        public FieldOpInstruction(OpCode instruction, FieldInfo field)
        {
            Instruction = instruction;
            Field = field;
        }

        public FieldInfo Field { get; set; }

        public override void Emit(ILGenerator Generator)
        {
            Generator.Emit(Instruction, Field);
        }
    }
}

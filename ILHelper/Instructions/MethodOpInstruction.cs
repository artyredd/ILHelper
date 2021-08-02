using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper
{
    public class MethodOpInstruction : OpInstructionBase
    {
        public MethodOpInstruction(OpCode Instruction, MethodInfo method)
        {
            Method = method;
            base.Instruction = Instruction;
        }

        public MethodInfo Method { get; set; }

        public override void Emit(ILGenerator Generator)
        {
            Generator.Emit(Instruction, Method);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper
{
    public class DefaultInstructionFactory : IInstructionFactory
    {
        private static DefaultInstructionFactory _Instance;
        public static DefaultInstructionFactory Instance
        {
            get
            {
                _Instance ??= new();
                return _Instance;
            }
        }

        private DefaultInstructionFactory()
        {
        }

        public IOpInstruction Create(OpCode Instruction)
        {
            return new OpInstructionBase() { Instruction = Instruction };
        }

        public IOpInstruction Create(OpCode Instruction, FieldInfo Field)
        {
            return new FieldOpInstruction(Instruction, Field);
        }

        public IOpInstruction Create(OpCode Instruction, MethodInfo Method)
        {
            return new MethodOpInstruction(Instruction, Method);
        }

        public IOpInstruction Create(OpCode Instruction, Type Type)
        {
            return new TypeOpInstruction(Instruction, Type);
        }

        public IOpInstruction Create(OpCode Instruction, object Param = null)
        {
            if (Param is null)
            {
                return Create(Instruction);
            }

            if (Param is FieldInfo field)
            {
                return Create(Instruction, field);
            }

            if (Param is MethodInfo method)
            {
                return Create(Instruction, method);
            }

            if (Param is Type t)
            {
                return Create(Instruction, t);
            }

            throw Helpers.Exceptions.Generic($"Failed to create an OpCode Instruction becuase the parameter provided({Param.GetType().FullName}) is not supported.");
        }
    }
}

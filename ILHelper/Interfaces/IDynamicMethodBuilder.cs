using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ILHelper
{
    public interface IDynamicMethodBuilder
    {
        List<OpCode> Instructions { get; set; }

        DynamicMethod Build();

        IDynamicMethodBuilder Accepts(Type ParameterType);
        IDynamicMethodBuilder Returns(Type ReturnType);
        IDynamicMethodBuilder Accepts(params Type[] Parameters);
        IDynamicMethodBuilder Emit(OpCode Opcode);
        IDynamicMethodBuilder Clear();
    }
}
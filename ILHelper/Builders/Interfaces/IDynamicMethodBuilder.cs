using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace ILHelper
{
    public interface IDynamicMethodBuilder
    {
        MethodInfo Build();

        /// <summary>
        /// Adds the provided type as an argument to the method
        /// </summary>
        /// <param name="ParameterType"></param>
        /// <returns></returns>
        IDynamicMethodBuilder Accepts(Type ParameterType);

        /// <summary>
        /// Sets the return type of the method
        /// </summary>
        /// <param name="ReturnType"></param>
        /// <returns></returns>
        IDynamicMethodBuilder Returns(Type ReturnType);

        /// <summary>
        /// Sets the argument parameters for this method to the provided types
        /// </summary>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        IDynamicMethodBuilder Accepts(params Type[] Parameters);

        /// <summary>
        /// Emits the provided instruction for the method 
        /// </summary>
        /// <param name="Opcode"></param>
        /// <returns></returns>
        IDynamicMethodBuilder Emit(OpCode Opcode);

        /// <summary>
        /// Clears all the current instructions
        /// </summary>
        /// <returns></returns>
        IDynamicMethodBuilder Clear();

        IDynamicMethodBuilder Emit(Action<IInstructionFactory, IList<IOpInstruction>> Expression);
        IDynamicMethodBuilder Emit(Action<IInstructionFactory, IList<IOpInstruction>, IDynamicTypeBuilder> Expression);

        /// <summary>
        /// Emits a <see cref="OpCode.Ret"/> instruction, builds the method, and returns a <see cref="IDynamicTypeBuilder"/> who controls this method.
        /// </summary>
        /// <returns></returns>
        IDynamicTypeBuilder Return();
        MethodBuilder GetBuilder();
    }
}
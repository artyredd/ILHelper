using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

#nullable enable
namespace ILHelper
{
    public class DynamicMethodBuilder : BuilderBase, IDynamicMethodBuilder
    {
        public string Name { get; set; } = "NewMethod";

        public List<IOpInstruction> Instructions { get; set; } = new();

        public List<Type> ParameterTypes { get; set; } = new();

        public Type ReturnType { get; set; } = typeof(void);

        private DynamicMethod? Method;
        private readonly MethodBuilder? Builder;
        private readonly IDynamicTypeBuilder? TypeBuilder;
        private readonly IInstructionFactory InstructionFactory = DefaultInstructionFactory.Instance;

        public DynamicMethodBuilder(string Name)
        {
            this.Name = Name;
        }

        public DynamicMethodBuilder(IDynamicTypeBuilder TypeBuilder, MethodBuilder Builder)
        {
            this.Name = Builder.Name;
            this.Builder = Builder;
            this.TypeBuilder = TypeBuilder;
        }

        public DynamicMethodBuilder()
        {

        }

        /// <summary>
        /// Generates a DynamicMethod using this objects assigned <see cref="ReturnType"/>, <see cref="ParameterTypes"/> and <see cref="Instructions"/>
        /// </summary>
        /// <returns></returns>
        public MethodInfo Build()
        {
            ILGenerator generator = GetGenerator();

            EmitInstructions(generator);

            return BuildMethod();
        }

        public IDynamicMethodBuilder Emit(OpCode Opcode)
        {
            Emit((factory, list) => list.Add(factory.Create(Opcode)));

            return this;
        }

        public IDynamicMethodBuilder Emit(Action<IInstructionFactory, IList<IOpInstruction>> Expression)
        {
            InvokeCritical(() =>
            {
                Expression(this.InstructionFactory, this.Instructions);
            });

            return this;
        }

        public IDynamicMethodBuilder Emit(Action<IInstructionFactory, IList<IOpInstruction>, IDynamicTypeBuilder> Expression)
        {
            InvokeCritical(() =>
            {
                Expression(this.InstructionFactory, this.Instructions, this.TypeBuilder!);
            });

            return this;
        }

        public IDynamicMethodBuilder Clear()
        {
            InvokeCritical(() => this.Instructions.Clear());

            return this;
        }

        public IDynamicMethodBuilder Returns(Type? ReturnType)
        {
            if (ReturnType is null)
            {
                throw Helpers.Exceptions.Generic($"Can't assign the return type of {Name}({nameof(DynamicMethodBuilder)}) to null.");
            }

            InvokeCritical(() => this.ReturnType = ReturnType);

            return this;
        }

        public IDynamicMethodBuilder Accepts(Type? ParameterType)
        {
            if (ParameterType is null)
            {
                throw Helpers.Exceptions.Generic($"Can't add a new parameter to {Name}({nameof(DynamicMethodBuilder)}) becuase that parameter was null.");
            }

            InvokeCritical(() =>
            {
                this.ParameterTypes.Add(ParameterType);
            });

            return this;
        }

        public IDynamicMethodBuilder Accepts(params Type?[] Parameters)
        {
            InvokeCritical(() =>
            {
                ParameterTypes.Clear();

                for (int i = 0; i < Parameters.Length; i++)
                {
                    Type? param = Parameters[i];

                    if (param is null)
                    {
                        throw Helpers.Exceptions.Generic("Can't add a new parameter to {Name}({nameof(DynamicMethodBuilder)}) becuase that parameter was null.");
                    }

                    ParameterTypes.Add(param);
                }
            });

            return this;
        }

        public IDynamicTypeBuilder Return()
        {
            Emit(OpCodes.Ret);

            Build();

            return this.TypeBuilder!;
        }

        public MethodBuilder? GetBuilder()
        {
            return this.Builder;
        }

        private MethodInfo BuildMethod()
        {
            if (Builder is null || TypeBuilder is null)
            {
                if (Method is null)
                {
                    throw Helpers.Exceptions.Generic($"Failed to build dynamic method, {nameof(Method)} was null.");
                }
                return Method;
            }

            return Builder;
        }

        private ILGenerator GetGenerator()
        {
            if (Builder is null)
            {
                var newMethod = CreateMethod();

                InvokeCritical(() => this.Method = newMethod);

                return newMethod.GetILGenerator();
            }

            Builder.SetReturnType(ReturnType);

            Builder.SetParameters(ParameterTypes.ToArray());

            return Builder.GetILGenerator();
        }

        private DynamicMethod CreateMethod()
        {
            // there may be a possibility of changing types during runtime, avoid race condition with invoke critical
            Type[] parameterTypes = InvokeCritical(() => ParameterTypes.ToArray());

            return new DynamicMethod(
                Name,
                ReturnType,
                parameterTypes
            );
        }

        private void EmitInstructions(ILGenerator generator)
        {
            // safely get a copy of the instructions and emit them to the method
            Span<IOpInstruction> instructions = InvokeCritical(() => Instructions.ToArray());

            for (int i = 0; i < instructions.Length; i++)
            {
                instructions[i].Emit(generator);
            }
        }
    }
}

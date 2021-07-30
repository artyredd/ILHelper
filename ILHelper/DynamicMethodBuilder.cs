using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Threading;

#nullable enable
namespace ILHelper
{
    public class DynamicMethodBuilder : IDynamicMethodBuilder
    {
        public string Name { get; set; } = "NewMethod";

        public List<OpCode> Instructions { get; set; } = new();

        public List<Type> ParameterTypes { get; set; } = new();

        public void Emit(object opcode)
        {
            throw new NotImplementedException();
        }

        public Type ReturnType { get; set; } = typeof(void);

        public DynamicMethod? Method { get; private set; } = null;

        private readonly SemaphoreSlim SyncronizationLock = new(1, 1);

        public DynamicMethodBuilder(string Name)
        {
            this.Name = Name;
        }
        public DynamicMethodBuilder()
        {

        }

        /// <summary>
        /// Generates a DynamicMethod using this objects assigned <see cref="ReturnType"/>, <see cref="ParameterTypes"/> and <see cref="Instructions"/>
        /// </summary>
        /// <returns></returns>
        public DynamicMethod Build()
        {
            // there may be a possibility of changing types during runtime, avoid race condition with invoke critical
            Type[] parameterTypes = InvokeCritical(() => ParameterTypes.ToArray());

            var newMethod = new DynamicMethod(
                Name,
                ReturnType,
                parameterTypes
            );

            ILGenerator generator = newMethod.GetILGenerator();

            // safely get a copy of the instructions and emit them to the method
            Span<OpCode> instructions = InvokeCritical(() => Instructions.ToArray());

            for (int i = 0; i < instructions.Length; i++)
            {
                generator.Emit(instructions[i]);
            }

            InvokeCritical(() => this.Method = newMethod);

            return newMethod;
        }

        public IDynamicMethodBuilder Emit(OpCode Opcode)
        {
            InvokeCritical(() => this.Instructions.Add(Opcode));

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
                this.ParameterTypes.Clear();

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

        private T InvokeCritical<T>(Func<T> Expression)
        {
            SyncronizationLock?.Wait();
            try
            {
                return Expression.Invoke();
            }
            finally
            {
                SyncronizationLock?.Release();
            }
        }

        private void InvokeCritical(Action action)
        {
            SyncronizationLock?.Wait();
            try
            {
                action.Invoke();
            }
            finally
            {
                SyncronizationLock?.Release();
            }
        }
    }
}

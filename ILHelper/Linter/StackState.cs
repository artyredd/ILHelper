using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace ILHelper.Linter
{
    public class StackState
    {
        public Stack<IStackElement> Stack { get; } = new(10);

        private readonly IStackElementFactory Factory = new StackElementFactory();

        // https://www.ecma-international.org/wp-content/uploads/ECMA-335_6th_edition_june_2012.pdf CLS §III.1.8.1.2.1
        private readonly IMapper<Type, StackVerificationType> Mapper = new GenericMapper<Type, StackVerificationType>();

        public bool HasTop => Stack.Count >= 1;

        public int Count => Stack.Count;

        public bool HasCount(int count)
        {
            return Count >= count;
        }

        public IStackElement? Top()
        {
            if (Stack.TryPeek(out IStackElement frame))
            {
                if (frame is null && HasTop)
                {
                    throw Helpers.Exceptions.Generic($"Attempted to peek at stack frame when no stack frame element existed. Stack count and the peeked value do not match.");
                }

                return frame;
            }

            return default;
        }

        public IEnumerable<IStackElement> Top(int count = 1)
        {
            IStackElement[] values;
            if (Stack.TryPeek(out values, 0, count - 1))
            {
                return values;
            }

            return Array.Empty<IStackElement>();
        }

        public bool Pop()
        {
            return Stack.TryPop(out _);
        }

        public bool Pop(int N)
        {
            bool success = false;
            for (int i = 0; i < N; i++)
            {
                success &= Stack.TryPop(out _);
            }

            return success;
        }

        public void Push(Type underlyingType)
        {
            Stack.Push(Create(underlyingType));
        }

        private IStackElement Create(Type underlyingType)
        {
            // get the verification types of any primitives
            if (Mapper.TryGet(underlyingType, out StackVerificationType verificationType))
            {
                return Factory.Create(underlyingType, verificationType);
            }

            // if its a user defined or COM value type
            if (underlyingType.IsValueType)
            {
                return Factory.Create(underlyingType, StackVerificationType.Value);
            }

            // if it's a by-ref struct or class
            if (underlyingType.IsPointer)
            {
                return Factory.Create(underlyingType, StackVerificationType.Pointer);
            }

            // all others
            return Factory.Create(underlyingType, StackVerificationType.Object);
        }

        private void MapDefaultTypes()
        {
            // mapp values to conform with CLS §III.1.8.1.2.1

            // mapp CLI type to Stack state type
            Mapper.Map(typeof(sbyte), StackVerificationType.Int32)
                    .Map(typeof(bool), StackVerificationType.Int32)
                    .Map(typeof(byte), StackVerificationType.Int32);

            Mapper.Map(typeof(short), StackVerificationType.Int32)
                    .Map(typeof(ushort), StackVerificationType.Int32)
                    .Map(typeof(char), StackVerificationType.Int32);

            Mapper.Map(typeof(int), StackVerificationType.Int32)
                    .Map(typeof(uint), StackVerificationType.Int32);

            Mapper.Map(typeof(long), StackVerificationType.Int64)
                    .Map(typeof(ulong), StackVerificationType.Int64);

            Mapper.Map(typeof(IntPtr), StackVerificationType.NativeInt)
                    .Map(typeof(UIntPtr), StackVerificationType.NativeInt);

            Mapper.Map(typeof(float), StackVerificationType.Float64)
                    .Map(typeof(double), StackVerificationType.Float64);

            Mapper.Map(typeof(decimal), StackVerificationType.Float64);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace ILHelper.Linter
{
    /// <summary>
    /// Thread safe implementation of a Stack
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Stack<T> : BuilderBase
    {
        public const int DefaultCapacity = 100;

        public int Count => Index;

        public int Capacity => Elements.Length;

        // Access this member within InvokeCritcal to maintain thread saftey, it's not fast but its convenient
        protected T[] Elements;

        // Access this member within InvokeCritcal to maintain thread saftey, it's not fast but its convenient
        protected int Index = 0;

        public Stack(int Capacity = DefaultCapacity)
        {
            Elements = new T[Capacity];
        }

        public T? Pop()
        {
            return InvokeCritical(UnsafePop);
        }

        public bool TryPop([NotNullWhen(returnValue: true)] out T Value)
        {
            // if the index is a positive integer it means that there exists some value in index 0 and pop will always return a value
            // if index is 0 there exists no element in the backing array before it and any attempt to pop a value will return default(T)
            if (Index == 0)
            {
                Value = default!;
                return false;
            }

            Value = Pop()!;
            return true!;
        }

        public void Push(T Value)
        {
            InvokeCritical(() => UnsafePush(Value));
        }

        public bool TryPeek([NotNullWhen(returnValue: true)] out T Value, int OffsetFromLast = 0)
        {
            // index determines the next spot and we cant peek at a negative index
            if (OffsetFromLast >= Index || OffsetFromLast < 0)
            {
                Value = default!;
                return false;
            }

            Value = InvokeCritical(() => Elements[Index - OffsetFromLast - 1])!;
            return true;
        }

        public bool TryPeek(out T[] Values, int StartOffset = 0, int EndOffset = 0)
        {
            // make sure both are above or equal to 0 and are within range of the array
            Clamp(ref EndOffset, 0, Elements.Length);
            Clamp(ref StartOffset, 0, Elements.Length);

            //     end    start
            // 0 1 2 3 4 5
            // should return 5 4 3 2

            //     start  end
            // 0 1 2 3 4 5
            // invalid, would theoretically return 2 3 4 5 but that is unexpected behaviour
            if (EndOffset < StartOffset)
            {
                throw Helpers.Exceptions.Generic($"Failed to peek multiple values within stack argument {nameof(StartOffset)} cannot be less than {nameof(EndOffset)}");
            }

            // if there is nothing to peek at return nothing
            if (Index == 0)
            {
                Values = Array.Empty<T>();
                return false;
            }

            Values = InvokeCritical(() =>
            {
                // avoid race condition by calcing offsets in critical
                StartOffset = Index - StartOffset;
                EndOffset = Index - EndOffset - 1;

                return Elements[EndOffset..StartOffset];
            }).Reverse().ToArray();

            return true;
        }

        public void Clear()
        {
            InvokeCritical(UnsafeClear);
        }

        public Stack<T> Clone()
        {
            Stack<T> newStack = new();

            InvokeCritical(() =>
            {
                // avoid race condition here by using UnsafeCopyTo
                newStack.Index = this.Index;
                UnsafeCopyTo(newStack.Elements, 0);
            });

            return newStack;
        }

        public void CopyTo(T[] DestinationArray, int Index)
        {
            InvokeCritical(() => UnsafeCopyTo(DestinationArray, Index));
        }

        private void UnsafeCopyTo(T[] DestinationArray, int Index)
        {
            this.Elements.CopyTo(DestinationArray, Index);
        }

        private T? UnsafePop()
        {
            T? value = default;

            if (Index > 0)
            {
                // intentionally leave the old value in the array for efficiency
                value = Elements[--Index];
            }

            return value;
        }

        private void UnsafePush(T Value)
        {
            if (Index == Capacity - 1)
            {
                UnsafeIncreaseCapacity();
            }

            // blind overwrite old data
            Elements[Index++] = Value;
        }

        private void UnsafeIncreaseCapacity()
        {
            Array.Resize(ref Elements, Capacity + 1);
        }

        private void UnsafeClear()
        {
            Array.Clear(Elements, 0, Capacity);
            Index = 0;
        }

        private void Clamp(ref int Value, int Lower, int Upper)
        {
            Value = Math.Max(Value, Lower);
            Value = Math.Min(Value, Upper);
        }
    }
}

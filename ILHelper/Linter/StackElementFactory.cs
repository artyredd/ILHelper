using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Linter
{
    public class StackElementFactory : IStackElementFactory
    {
        public IStackElement Create(System.ValueType Value)
        {
            return new StackElement() { ElementType = StackElementType.Value, Value = Value };
        }
        public IStackElement CreateRef(System.ValueType Value)
        {
            return new StackElement() { ElementType = StackElementType.Address, Value = Value };
        }
        public IStackElement CreateBoxed<T>(T Value) where T : class
        {
            return new StackElement() { ElementType = StackElementType.BoxedReference, Value = Value };
        }
        public IStackElement Create<T>(T Value) where T : class
        {
            return new StackElement() { ElementType = StackElementType.Address, Value = Value };
        }
        public IStackElement CreateBoxed(System.ValueType Value)
        {
            return new StackElement() { ElementType = StackElementType.Value, Value = Value };
        }
        public IStackElement Create(Delegate Value)
        {
            return new StackElement() { ElementType = StackElementType.Pointer, Value = Value };
        }
        public IStackElement CreateBoxedClassOrValue(object Value)
        {
            return new StackElement() { ElementType = StackElementType.BoxedAny, Value = Value };
        }
        public IStackElement CreateLabel(int index)
        {
            return new StackElement() { ElementType = StackElementType.Label, Value = index };
        }
    }
}

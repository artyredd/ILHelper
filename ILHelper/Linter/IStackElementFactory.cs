using System;

namespace ILHelper.Linter
{
    public interface IStackElementFactory
    {
        IStackElement Create(Delegate Value);
        IStackElement Create(ValueType Value);
        IStackElement Create<T>(T Value) where T : class;
        IStackElement CreateBoxed(ValueType Value);
        IStackElement CreateBoxed<T>(T Value) where T : class;
        IStackElement CreateBoxedClassOrValue(object Value);
        IStackElement CreateLabel(int index);
        IStackElement CreateRef(ValueType Value);
    }
}
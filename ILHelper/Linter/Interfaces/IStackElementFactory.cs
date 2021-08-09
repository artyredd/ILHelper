using System;

namespace ILHelper.Linter
{
    public interface IStackElementFactory
    {
        IStackElement Create(Type underlyingType, StackVerificationType verificationType);
    }
}
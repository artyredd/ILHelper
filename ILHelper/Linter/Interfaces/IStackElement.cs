using System;

namespace ILHelper.Linter
{
    public interface IStackElement
    {
        StackVerificationType VerificationType { get; set; }
        bool HasType { get; }
        Type UnderlyingType { get; set; }
    }
}
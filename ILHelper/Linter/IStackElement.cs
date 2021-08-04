namespace ILHelper.Linter
{
    public interface IStackElement
    {
        StackElementType ElementType { get; set; }
        bool HasValue { get; }
        object Value { get; set; }
    }
}
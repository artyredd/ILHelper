namespace ILHelper.Linter
{
    public interface IMapper<T, U>
    {
        U GetMapping(T From);
        GenericMapper<T, U> Map(T From, U To);
        bool TryGet(T From, out U To);
    }
}
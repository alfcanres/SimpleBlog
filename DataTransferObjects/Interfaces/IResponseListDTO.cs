namespace DataTransferObjects.Interfaces
{
    public interface IResponseListDTO<T>
    {
        IEnumerable<T> List { get; }
        int RecordCount { get; }
    }
}
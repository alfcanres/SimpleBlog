namespace DataTransferObjects.Interfaces
{
    public interface IResponseDTO<DTO> where DTO : class
    {
        DTO Data { get; }
    }
}
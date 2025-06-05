using DataTransferObjects.Interfaces;

namespace BusinessLogicLayer.Response
{
    public class ResponseDTO<T> : IResponseDTO<T> where T : class
    {
        private readonly T _DTO;
        public ResponseDTO(T DTO)
        {
            _DTO = DTO;

        }

        public T Data
        {
            get
            {
                return _DTO;
            }
        }

    }
}

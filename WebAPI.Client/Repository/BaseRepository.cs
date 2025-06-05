using WebAPI.Client.Helpers;
using WebAPI.Client.ViewModels;


namespace WebAPI.Client.Repository
{
    public class BaseRepository<CreateDTO, ReadDTO, UpdateDTO>
        where ReadDTO : class
        where CreateDTO : class
        where UpdateDTO : class
    {
        private readonly IHttpClientHelper _httpClientHelper;

        public IHttpClientHelper HttpClientHelper => _httpClientHelper;

        public string BaseEndPoint { get => _baseEndPoint; set => _baseEndPoint = value; }

        private string _baseEndPoint = string.Empty;

        public BaseRepository(
            string baseEndPoint,
            IHttpClientHelper httpClientHelper)
        {
            _httpClientHelper = httpClientHelper;
            _baseEndPoint = baseEndPoint;
        }

        public async Task<ResponseViewModel<ReadDTO>> CreateAsync(CreateDTO createModel)
        {
            return await HttpClientHelper.GetResponse<ReadDTO, CreateDTO>(createModel, HttpVerbsEnum.POST, _baseEndPoint);
        }

        public async Task<ResponseViewModel<ReadDTO>> UpdateAsync(UpdateDTO createModel)
        {
            return await HttpClientHelper.GetResponse<ReadDTO, UpdateDTO>(createModel, HttpVerbsEnum.PUT, _baseEndPoint);
        }

        public async Task<ResponseViewModel<ReadDTO>> GetByIdAsync(int id)
        {
            return await HttpClientHelper.GetResponse<ReadDTO, int>(id, HttpVerbsEnum.GET, $"{_baseEndPoint}/{id}");
        }

        public async Task<ResponseViewModel<bool>> DeleteAsync(int id)
        {
            return await HttpClientHelper.GetValidateResponse(id, HttpVerbsEnum.DELETE, $"{_baseEndPoint}/{id}");
        }


    }
}

using DataTransferObjects;
using DataTransferObjects.DTO;
using WebAPI.Client.Helpers;
using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Repository.MoodType
{
    public class MoodTypeRepository :
        BaseRepository<MoodTypeCreateDTO, MoodTypeReadDTO, MoodTypeUpdateDTO>,
        IMoodTypeRepository
    {

        public MoodTypeRepository(
            IHttpClientHelper httpClientHelper
            ) : base("api/moodtype", httpClientHelper)
        {

        }

        public async Task<ResponseViewModel<ResponseList<MoodTypeReadDTO>>> GetIsAvailableAsync(bool isAvailable)
        {

            return await HttpClientHelper.GetResponse<ResponseList<MoodTypeReadDTO>, bool>(isAvailable, HttpVerbsEnum.GET, $"{BaseEndPoint}/available/{isAvailable}");
        }

        public async Task<ResponseViewModel<ResponsePagedList<MoodTypeReadDTO>>> GetPagedAsync(PagerParams pagerDTO)
        {
            return await HttpClientHelper.GetResponse<ResponsePagedList<MoodTypeReadDTO>, PagerParams>(pagerDTO, HttpVerbsEnum.GET, $"{BaseEndPoint}/paged{pagerDTO.ToQueryString()}");
        }

        public async Task<ResponseViewModel<ResponseList<MoodTypeReadDTO>>> GetTopTenAsync()
        {
            return await HttpClientHelper.GetResponse<ResponseList<MoodTypeReadDTO>, int>(10, HttpVerbsEnum.GET, $"{BaseEndPoint}/top/10");
        }
    }
}

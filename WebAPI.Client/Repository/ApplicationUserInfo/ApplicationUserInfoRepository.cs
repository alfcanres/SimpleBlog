using DataTransferObjects;
using DataTransferObjects.DTO;
using WebAPI.Client.Helpers;
using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Repository.ApplicationUserInfo
{
    public class ApplicationUserInfoRepository :
        BaseRepository<ApplicationUserInfoCreateDTO, ApplicationUserInfoReadDTO, ApplicationUserInfoUpdateDTO>,
        IApplicationUserInfoRepository
    {
        public ApplicationUserInfoRepository(
                    IHttpClientHelper httpClientHelper
                   ) : base("api/applicationuserinfo", httpClientHelper)
        {

        }

        public async Task<ResponseViewModel<ApplicationUserInfoReadDTO>> GetByEmailAsync(string email)
        {
            return await HttpClientHelper.GetResponse<ApplicationUserInfoReadDTO>(HttpVerbsEnum.GET, $"{BaseEndPoint}/GetByEmail/{email}");
        }

        public Task<ResponseViewModel<ResponsePagedList<ApplicationUserInfoListDTO>>> GetPagedAsync(PagerParams pagerDTO)
        {
            return HttpClientHelper.GetResponse<ResponsePagedList<ApplicationUserInfoListDTO>, PagerParams>(pagerDTO, HttpVerbsEnum.GET, $"{BaseEndPoint}/paged{pagerDTO.ToQueryString()}");
        }
        public Task<ResponseViewModel<ResponsePagedList<ApplicationUserInfoListDTO>>> GetPagedByActiveAsync(PagerParams pagerDTO)
        {
            return HttpClientHelper.GetResponse<ResponsePagedList<ApplicationUserInfoListDTO>, PagerParams>(pagerDTO, HttpVerbsEnum.GET, $"{BaseEndPoint}/pagedbyactive{pagerDTO.ToQueryString()}");
        }
        public Task<ResponseViewModel<ResponseList<ApplicationUserInfoListDTO>>> GetTopTenAsync()
        {
            return HttpClientHelper.GetResponse<ResponseList<ApplicationUserInfoListDTO>, int>(10, HttpVerbsEnum.GET, $"{BaseEndPoint}/top/10");
        }
    }
}

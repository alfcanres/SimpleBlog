using DataTransferObjects.DTO;
using DataTransferObjects;
using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Repository.ApplicationUserInfo
{
    public interface IApplicationUserInfoRepository 
    {
        Task<ResponseViewModel<ApplicationUserInfoReadDTO>> CreateAsync(ApplicationUserInfoCreateDTO createModel);
        Task<ResponseViewModel<bool>> DeleteAsync(int id);
        Task<ResponseViewModel<ApplicationUserInfoReadDTO>> GetByIdAsync(int id);
        Task<ResponseViewModel<ApplicationUserInfoReadDTO>> GetByEmailAsync(string email);
        Task<ResponseViewModel<ApplicationUserInfoReadDTO>> UpdateAsync(ApplicationUserInfoUpdateDTO updateModel);

        Task<ResponseViewModel<ResponsePagedList<ApplicationUserInfoListDTO>>> GetPagedAsync(PagerParams pagerDTO);

        Task<ResponseViewModel<ResponsePagedList<ApplicationUserInfoListDTO>>> GetPagedByActiveAsync(PagerParams pagerDTO);

        Task<ResponseViewModel<ResponseList<ApplicationUserInfoListDTO>>> GetTopTenAsync();
    }
}

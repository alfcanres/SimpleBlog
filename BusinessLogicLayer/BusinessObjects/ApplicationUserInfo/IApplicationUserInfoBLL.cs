using BusinessLogicLayer.Interfaces;
using DataTransferObjects;
using DataTransferObjects.DTO;

namespace BusinessLogicLayer.BusinessObjects
{
    public interface IApplicationUserInfoBLL : IBLL<ApplicationUserInfoCreateDTO, ApplicationUserInfoReadDTO, ApplicationUserInfoUpdateDTO>
    {
        Task<ResponsePagedList<ApplicationUserInfoListDTO>> GetAllActivePagedAsync(PagerParams pagerDTO);
        Task<ResponseList<ApplicationUserInfoListDTO>> GetTopWithPostsAsync(int top);
        Task<ResponsePagedList<ApplicationUserInfoListDTO>> GetAllPagedAsync(PagerParams pagerDTO);
        Task<ApplicationUserInfoReadDTO> GetByUserEmailAsync(string email);

    }
}

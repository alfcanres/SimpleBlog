using BusinessLogicLayer.Interfaces;
using DataTransferObjects;
using DataTransferObjects.DTO;

namespace BusinessLogicLayer.BusinessObjects
{
    public interface IPostTypeBLL : IBLL<PostTypeCreateDTO, PostTypeReadDTO, PostTypeUpdateDTO>
    {
        Task<ResponsePagedList<PostTypeReadDTO>> GetAllPagedAsync(PagerParams pagerDTO);
        Task<int> CountAllAsync();
        Task<ResponseList<PostTypeReadDTO>> GetAllAsync();
        Task<ResponseList<PostTypeReadDTO>> GetAllByIsAvailableAsync(bool isAvailable);
        Task<ResponseList<PostTypeReadDTO>> GetTopWithPostsAsync(int top);


    }
}
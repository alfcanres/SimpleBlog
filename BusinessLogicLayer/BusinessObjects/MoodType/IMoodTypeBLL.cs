using BusinessLogicLayer.Interfaces;
using DataTransferObjects;
using DataTransferObjects.DTO;


namespace BusinessLogicLayer.BusinessObjects
{
    public interface IMoodTypeBLL : IBLL<MoodTypeCreateDTO, MoodTypeReadDTO, MoodTypeUpdateDTO>
    {
        Task<int> CountAllAsync();
        Task<ResponseList<MoodTypeReadDTO>> GetAllAsync();
        Task<ResponseList<MoodTypeReadDTO>> GetAllByIsAvailableAsync(bool isAvailable);
        Task<ResponsePagedList<MoodTypeReadDTO>> GetAllPagedAsync(PagerParams pagerDTO);
        Task<ResponseList<MoodTypeReadDTO>> GetTopWithPostsAsync(int top);
    }
}
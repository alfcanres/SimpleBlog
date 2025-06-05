using DataTransferObjects;
using DataTransferObjects.DTO;
using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Repository.MoodType
{
    public interface IMoodTypeRepository 
    {
        Task<ResponseViewModel<MoodTypeReadDTO>> CreateAsync(MoodTypeCreateDTO createModel);
        Task<ResponseViewModel<bool>> DeleteAsync(int id);
        Task<ResponseViewModel<MoodTypeReadDTO>> GetByIdAsync(int id);
        Task<ResponseViewModel<MoodTypeReadDTO>> UpdateAsync(MoodTypeUpdateDTO updateModel);

        Task<ResponseViewModel<ResponsePagedList<MoodTypeReadDTO>>> GetPagedAsync(PagerParams pagerDTO);

        Task<ResponseViewModel<ResponseList<MoodTypeReadDTO>>> GetIsAvailableAsync(bool isAvailable);

        Task<ResponseViewModel<ResponseList<MoodTypeReadDTO>>> GetTopTenAsync();

    }
}

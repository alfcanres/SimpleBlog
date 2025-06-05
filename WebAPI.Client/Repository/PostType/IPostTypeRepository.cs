using DataTransferObjects;
using DataTransferObjects.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Repository.PostType
{
    public interface IPostTypeRepository 
    {
        Task<ResponseViewModel<PostTypeReadDTO>> CreateAsync(PostTypeCreateDTO createModel);
        Task<ResponseViewModel<bool>> DeleteAsync(int id);
        Task<ResponseViewModel<PostTypeReadDTO>> GetByIdAsync(int id);
        Task<ResponseViewModel<PostTypeReadDTO>> UpdateAsync(PostTypeUpdateDTO updateModel);
        Task<ResponseViewModel<ResponsePagedList<PostTypeReadDTO>>> GetPagedAsync(PagerParams pagerDTO);
        Task<ResponseViewModel<int>> CountAllAsync();
        Task<ResponseViewModel<ResponseList<PostTypeReadDTO>>> GetAllByIsAvailableAsync(bool isAvailable);
        Task<ResponseViewModel<ResponseList<PostTypeReadDTO>>> GetTopTenAsync(int top);

    }
}

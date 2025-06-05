using DataTransferObjects.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Repository.PostVote
{
    public interface IPostVoteRepository 
    {
        Task<ResponseViewModel<PostVoteResultDTO>> CreateAsync(PostVoteCreateDTO createModel);
        Task<ResponseViewModel<bool>> DeleteAsync(int id);
        Task<ResponseViewModel<PostVoteResultDTO>> UpdateAsync(PostVoteUpdateDTO updateModel);
    }
}

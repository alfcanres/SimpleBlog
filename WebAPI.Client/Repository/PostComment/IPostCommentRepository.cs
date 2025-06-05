using DataTransferObjects;
using DataTransferObjects.DTO;
using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Repository.PostComment
{
    public interface IPostCommentRepository 
    {
        Task<ResponseViewModel<PostCommentReadDTO>> CreateAsync(PostCommentCreateDTO createModel);
        Task<ResponseViewModel<bool>> DeleteAsync(int id);
        Task<ResponseViewModel<PostCommentReadDTO>> GetByIdAsync(int id);
        Task<ResponseViewModel<ResponseList<PostCommentReadDTO>>> GetByPostIdAsync(int postId);
    }
}

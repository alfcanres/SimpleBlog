using DataTransferObjects;
using DataTransferObjects.DTO;
using DataTransferObjects.DTO.PostVote;
using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Repository.Post
{
    public interface IPostRepository 
    {
        Task<ResponseViewModel<PostReadDTO>> CreateAsync(PostCreateDTO createModel);
        Task<ResponseViewModel<bool>> DeleteAsync(int id);
        Task<ResponseViewModel<PostReadDTO>> GetByIdAsync(int id);
        Task<ResponseViewModel<PostReadDTO>> UpdateAsync(PostUpdateDTO updateModel);

        Task<ResponseViewModel<bool>> ApproveAsync(int id);
        Task<ResponseViewModel<ResponsePagedList<PostListDTO>>> GetPublishedPagedByUserAsync(int id, PagerParams pagerDTO);
        Task<ResponseViewModel<ResponsePagedList<PostListDTO>>> GetAllPagedByUserAsync(int id, PagerParams pagerDTO);

        Task<ResponseViewModel<ResponsePagedList<PostListDTO>>> GetPublishedPagedAsync(PagerParams pagerDTO);

        Task<ResponseViewModel<ResponsePagedList<PostListDTO>>> GetPendingPublishPagedAsync(PagerParams pagerDTO);

        Task<ResponseViewModel<ResponsePagedList<PostCommentReadDTO>>> GetPostCommentsPageddAsync(int postId, PagerParams pagerDTO);

        Task<ResponseViewModel<PostVoteViewDTO>> GetPostVotesAsync(int postId);

        Task<ResponseViewModel<ResponseList<PostCommentReadDTO>>> GetPostCommentsAsync(int postId);

        Task<ResponseViewModel<PostVoteViewDTO>> VoteAsync(SubmitVoteDTO voteModel);

        Task<ResponseViewModel<PostCommentReadDTO>> CreateCommentAsync(PostCommentCreateDTO postCommentCreate);

    }
}

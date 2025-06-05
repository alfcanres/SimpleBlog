using BusinessLogicLayer.Interfaces;
using DataTransferObjects;
using DataTransferObjects.DTO;

namespace BusinessLogicLayer.BusinessObjects
{
    public interface IPostBLL : IBLL<PostCreateDTO, PostReadDTO, PostUpdateDTO>
    {

        Task<ValidatorResponse> ValidateApprovePostPublishAsync(int postId);
        Task ApprovePostPublishAsync(int postId);
        Task<ResponsePagedList<PostPendingToPublishDTO>> GetAllPostsNotPublishedAsync(PagerParams pagerDTO);
        Task<ResponsePagedList<PostListDTO>> GetPostsPublishedPagedAsync(PagerParams pagerDTO);
        Task<ResponsePagedList<PostListDTO>> GetPostsPublishedByUserPagedAsync(int UserID, PagerParams pagerDTO);
        Task<ResponsePagedList<PostListDTO>> GetAllPostByUserPagedAsync(int UserID, PagerParams pagerDTO);


    }
}
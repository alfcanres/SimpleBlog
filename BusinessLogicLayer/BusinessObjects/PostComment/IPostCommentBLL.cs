using DataTransferObjects;
using DataTransferObjects.DTO;

namespace BusinessLogicLayer.BusinessObjects
{
    public interface IPostCommentBLL 
    {
        Task<ValidatorResponse> ValidateDeleteAsync(int id);
        Task DeleteAsync(int id);
        Task<PostCommentReadDTO> InsertAsync(PostCommentCreateDTO createDTO);
        Task<ValidatorResponse> ValidateInsertAsync(PostCommentCreateDTO createDTO);        
        Task<PostCommentReadDTO> GetByIdAsync(int id);
        Task<ResponsePagedList<PostCommentReadDTO>> GetPagedByPostIdAsync(int postID, PagerParams pagerDTO);
        Task<ResponseList<PostCommentReadDTO>> GetByPostIdAsync(int postID);

    }
}

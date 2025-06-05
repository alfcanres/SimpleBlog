using DataTransferObjects;
using DataTransferObjects.DTO;
using WebAPI.Client.Helpers;
using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Repository.PostComment
{
    public class PostCommentRepository :
        BaseRepository<PostCommentCreateDTO, PostCommentReadDTO, PostCommentUpdateDTO>,
        IPostCommentRepository
    {
        public PostCommentRepository(IHttpClientHelper httpClientHelper) 
            :base("api/postcomment",httpClientHelper)
        {

        }

        public async Task<ResponseViewModel<ResponseList<PostCommentReadDTO>>> GetByPostIdAsync(int postId)
        {
            return await HttpClientHelper.GetResponse<ResponseList<PostCommentReadDTO>, int>(postId, HttpVerbsEnum.GET, $"{BaseEndPoint}/GetByPostIdAsync/{postId}");

        }
    }
}

using DataTransferObjects.DTO;

namespace WebApp.Models
{
    public class PostReadViewModel
    {
        public PostListDTO Post { get; set; }
        public IEnumerable<PostCommentReadDTO> Comments { get; set; }   
        public PostVoteViewDTO Votes { get; set; }
    }
}

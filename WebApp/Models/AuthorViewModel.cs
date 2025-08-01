using DataTransferObjects;
using DataTransferObjects.DTO;

namespace WebApp.Models
{
    public class AuthorViewModel
    {
        public ApplicationUserInfoReadDTO Author { get; set; }
        public ResponsePagedList<PostListDTO> PostsPagedList { get; set; }

    }
}

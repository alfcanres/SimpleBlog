using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.DTO
{
    public record PostCommentReadDTO
    {
        public int Id { get; init; }
        public int PostId { get; init; }
        public int ApplicationUserInfoId { init; get; }
        public string UserName { get; init; }
        public string ProfilePicture { get; init; }
        public DateTime CommentDate { init; get; }
        public string CommentText { init; get; }
    }
}

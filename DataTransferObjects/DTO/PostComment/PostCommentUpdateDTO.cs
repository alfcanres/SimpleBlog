using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.DTO
{
    public record PostCommentUpdateDTO
    {
        public int Id { get; init; }
        [Required]
        public int PostId { get; init; }
        [Required]
        public int ApplicationUserInfoId { init; get; }

        [Required]
        [MaxLength(255)]
        public string CommentText { init; get; }
    }
}

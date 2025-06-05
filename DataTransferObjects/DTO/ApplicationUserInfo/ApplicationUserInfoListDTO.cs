using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.DTO
{
    public record ApplicationUserInfoListDTO
    {
        public int Id { init; get; }
        public string UserName { init; get; }
        public string? ProfilePicture { init; get; }
        public string FullName { init; get; }
        public int PostsCount { init; get; }
        public int PostsPedingForPublish { init; get; }
        public int CommentsCount { init; get; }

    }
}

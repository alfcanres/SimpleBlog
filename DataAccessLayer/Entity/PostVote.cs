using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entity
{
    public class PostVote
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        [Required]
        public Post Post { get; set; }
        public int ApplicationUserInfoId { set; get; }
        [Required]
        public ApplicationUserInfo ApplicationUserInfo { get; set; }
        [Required]
        public bool ILikedThis { get; set; }
        [Required]
        public DateTime VoteDate { get; set; }
    }
}

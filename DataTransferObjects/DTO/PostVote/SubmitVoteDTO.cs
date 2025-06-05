using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.DTO.PostVote
{
    public class SubmitVoteDTO
    {
        public int PostId { get; set; }
        public int ApplicationUserInfoId { get; set; }
        public bool ILikedThis { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.DTO
{
    public record PostVoteViewDTO
    {
        public int PostId { set; get; }
        public int Like { get; init; }
        public int Dislike { get; init; }
    }
}

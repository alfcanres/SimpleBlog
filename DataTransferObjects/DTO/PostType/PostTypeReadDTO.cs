using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.DTO
{
    public record PostTypeReadDTO
    {
        public int Id { init; get; }
        public string Description { init; get; }
        public bool IsAvailable { init; get; }
        public int PostCount { init; get; }
    }

}

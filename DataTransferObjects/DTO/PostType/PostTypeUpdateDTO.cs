using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.DTO
{
    public record PostTypeUpdateDTO
    {
        public int Id { init; get; }

        [MaxLength(255)]
        [Required(ErrorMessage = "Type of post must have a description")]
        public string Description { init; get; }
        public bool IsAvailable { init; get; }
    }


}

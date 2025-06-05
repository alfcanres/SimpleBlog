using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.DTO
{
    public record MoodTypeCreateDTO
    {
        [Required(ErrorMessage = "Mood must have a description")]
        public string Mood { init; get; }

        public bool IsAvailable { init; get; } 
    }
}

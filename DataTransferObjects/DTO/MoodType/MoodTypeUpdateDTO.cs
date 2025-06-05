using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.DTO
{
    public record MoodTypeUpdateDTO
    {
        public int Id { get; init; }

        [Required(ErrorMessage = "Mood must have a description")]
        public string Mood { get; init; }

        public bool IsAvailable { get; init; }
    }
}

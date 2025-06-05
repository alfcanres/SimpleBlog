using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.DTO
{
    public record PostCreateDTO
    {
        public int ApplicationUserInfoId { init; get; }
        [Required(ErrorMessage = "Please select a type for your post")]
        public int PostTypeId { get; init; }
        [Required(ErrorMessage = "Please select a mood for your post")]
        public int MoodTypeId { get; init; }

        [MaxLength(255, ErrorMessage = "Max chars for your title is 255!")]
        [Required(ErrorMessage = "Please type a title for your post")]
        public string Title { init; get; }

        [MaxLength(1024, ErrorMessage = "Avoid TLDR!!! Max chars 1024")]
        [Required(ErrorMessage = "Need some text here!")]
        public string Text { get; init; }
    }
}

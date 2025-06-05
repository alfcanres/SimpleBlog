using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.DTO
{
    public record PostUpdateDTO
    {
        public int Id { get; init; }
        public int ApplicationUserInfoId { init; get; }
        public int PostTypeId { get; init; }
        public int MoodTypeId { get; init; }

        [MaxLength(255, ErrorMessage = "Max chars for your title is 255!")]
        [Required(ErrorMessage = "Please type a title for your post")]
        public string Title { init; get; }

        [MaxLength(1024, ErrorMessage = "Avoid TDL!!! Max chars 1024")]
        [Required(ErrorMessage = "Need some text here!")]
        public string Text { get; init; }

    }
}

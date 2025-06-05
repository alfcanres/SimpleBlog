using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.DTO
{
    public record ApplicationUserInfoUpdateDTO
    {
        public int Id { init; get; }
        [MaxLength(255, ErrorMessage = "Max chars 255")]
        [Required(ErrorMessage = "Please type your username")]
        public string UserName { init; get; }
        public string? ProfilePicture { init; get; }

        [MaxLength(255, ErrorMessage = "Max chars 255")]
        [Required(ErrorMessage = "Please type your first name")]
        public string FirstName { init; get; }
        [MaxLength(255, ErrorMessage = "Max chars 255")]
        [Required(ErrorMessage = "Please type your last name")]
        public string LastName { init; get; }

    }
}

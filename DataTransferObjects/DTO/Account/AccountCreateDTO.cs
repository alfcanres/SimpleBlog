
using System.ComponentModel.DataAnnotations;


namespace DataTransferObjects.DTO
{
    public class AccountCreateDTO
    {
        [MaxLength(255)]
        [Required(ErrorMessage = "Type a valid user name")]
        public string UserName { init; get; }

        [EmailAddress]
        [Required(ErrorMessage = "Type a valid email")]
        public string Email { init; get; }

        [MaxLength(255)]
        [Required(ErrorMessage = "Type a valid fist name")]
        public string FirstName { init; get; }

        [MaxLength(255)]
        [Required(ErrorMessage = "Type a valid last name")]
        public string LastName { init; get; }

        [MinLength(5)]
        [Required(ErrorMessage = "Type a valid password")]
        [Compare("ComfirmPassword", ErrorMessage = "Password does not match")]
        [DataType(DataType.Password)]
        public string Password { init; get; }


        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        public string ComfirmPassword { init; get; }
    }
}

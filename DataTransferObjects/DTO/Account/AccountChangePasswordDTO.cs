
using System.ComponentModel.DataAnnotations;


namespace DataTransferObjects.DTO
{
    public class AccountChangePasswordDTO
    {
        [MaxLength(255)]
        [Required(ErrorMessage = "Type a valid user name")]
        public string UserName { init; get; }

        [EmailAddress]
        [Required(ErrorMessage = "Type a valid email")]
        public string Email { init; get; }

        [MinLength(150)]
        [Required(ErrorMessage = "Type a valid password")]
        [Compare("ComfirmPassword", ErrorMessage = "Password does not match")]
        [DataType(DataType.Password)]
        public string OldPassword { init; get; }

        [MinLength(150)]
        [Required(ErrorMessage = "Type a valid password")]
        [Compare("ComfirmNewPassword", ErrorMessage = "Password does not match")]
        [DataType(DataType.Password)]
        public string NewPassword { init; get; }


        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        public string ComfirmNewPassword { init; get; }
    }
}

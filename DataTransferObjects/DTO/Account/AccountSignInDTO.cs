
using System.ComponentModel.DataAnnotations;


namespace DataTransferObjects.DTO
{
    public class AccountSignInDTO
    {
        [EmailAddress]
        [Required(ErrorMessage = "Type a valid email")]
        public string Email { init; get; }

        [Required(ErrorMessage = "Type a valid password")]
        [DataType(DataType.Password)]
        public string Password { init; get; }
    }
}

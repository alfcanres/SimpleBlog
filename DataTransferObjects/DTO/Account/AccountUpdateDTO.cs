
using System.ComponentModel.DataAnnotations;


namespace DataTransferObjects.DTO
{
    public class AccountUpdateDTO
    {
        [MaxLength(255)]
        [Required(ErrorMessage = "Type a valid user name")]
        public string UserName { init; get; }

        [MaxLength(255)]
        [Required(ErrorMessage = "Type a valid email")]
        public string FirstName { init; get; }

        [MaxLength(255)]
        [Required(ErrorMessage = "Type a valid email")]
        public string LastName { init; get; }

        [EmailAddress]
        [Required(ErrorMessage = "Type a valid email")]
        public string Email { init; get; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.DTO
{
    public record ApplicationUserInfoReadDTO
    {
        public int Id { init; get; }

        public string UserID { init; get; }
        public string UserName { init; get; }
        public string? ProfilePicture { init; get; }
        public string FirstName { init; get; }
        public string LastName { init; get; }
        public DateTime? DateOfBirth { init; get; }
        public bool IsActive { init; get; }
    }
}



namespace DataTransferObjects.DTO
{
    public record AccountReadDTO
    {
        public string UserName { init; get; }
        public string? UserID { init; get; }

        public int ApplicationUserInfoId { init; get; }

        public string ProfilePicture { init; get; }

        public string FullName { init; get; }
        public string Email { init; get; }
    }
}

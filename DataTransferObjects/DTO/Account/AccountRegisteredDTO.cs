
namespace DataTransferObjects.DTO
{
    public record AccountRegisteredDTO
    {
        public string UserName { set; get; }
        public string Email { set; get; }

        public ValidatorResponse ValidateDTO { set; get; }
    }
}



namespace DataTransferObjects.DTO
{
    public record PostListDTO
    {
        public int Id { get; init; }
        public int ApplicationUserInfoId { init; get; }
        public string UserName { init; get; }
        public string ProfilePic { init; get; }
        public string PostType { get; init; }
        public string MoodType { get; init; }
        public string Title { init; get; } 
        public DateTime CreationDate { get; set; }
        public Nullable<DateTime> PublishDate { get; set; }
        public bool IsPublished { get; set; }
        public int Votes { init; get; }
        public int Comments { init; get; }

    }
}

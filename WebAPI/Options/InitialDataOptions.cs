namespace WebAPI.Options
{
    public class InitialDataOptions
    {
        public AdminUserOptions AdminUser { get; set; }
        public MoodTypesOptions MoodTypes { get; set; }
        public PostTypesOptions PostTypes { get; set; }
        public DummyUsersOptions DummyUsers { get; set; }
        public DummyPostsOptions DummyPosts { get; set; }
    }

    public class AdminUserOptions
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Role { get; set; }
    }

    public class MoodTypesOptions
    {
        public bool Create { get; set; }
        public List<string> MoodTypes { get; set; }
    }

    public class PostTypesOptions
    {
        public bool Create { get; set; }
        public List<string> PostTypes { get; set; }
    }

    public class DummyUsersOptions
    {
        public bool Create { get; set; }
        public int Count { get; set; }

        public string DefaultPassword { set; get; }
        public List<string> DummyNames { get; set; }
        public List<string> DummyLastNames { get; set; }
    }

    public class DummyPostsOptions
    {
        public bool Create { get; set; }
        public int Count { get; set; }
        public List<string> DummyPostTitles { get; set; }
        public List<string> DummyPostContents { get; set; }
    }
}

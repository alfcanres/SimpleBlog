using DataAccessLayer.Entity;

namespace BusinessLogicLayer.Interfaces
{
    public interface IUnitOfWork
    {
        public IRepository<Post> Posts { get; }
        public IRepository<PostType> PostTypes { get; }
        public IRepository<PostVote> PostVotes { get; }
        public IRepository<PostComment> PostComments { get; }
        public IRepository<MoodType> MoodTypes { get; }
        public IRepository<ApplicationUserInfo> UsersInfo { get; }
        Task SaveAsync();
    }
}

using DataAccessLayer.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace DataAccessLayer
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {


        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Post>()
                .HasMany(t => t.Comments)
                .WithOne(t => t.Post)
                .HasForeignKey(t => t.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Post>()
                .HasMany(t => t.Votes)
                .WithOne(t => t.Post)
                .HasForeignKey(t => t.PostId)
                .OnDelete(DeleteBehavior.NoAction);



            base.OnModelCreating(builder);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Enabling lazy loading by proxies requires three steps:
            //add package Microsoft.EntityFrameworkCore.Proxies
            //optionsBuilder.UseLazyLoadingProxies();

            base.OnConfiguring(optionsBuilder);

        }

        public DbSet<MoodType> MoodTypes => Set<MoodType>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<PostType> PostTypes => Set<PostType>();
        public DbSet<PostVote> PostVotes => Set<PostVote>();
        public DbSet<PostComment> PostComments => Set<PostComment>();
        public DbSet<ApplicationUserInfo> ApplicationUsers => Set<ApplicationUserInfo>();
    }
}

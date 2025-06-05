using DataAccessLayer.Entity;
using DataAccessLayer;

using AutoMapper;
using BusinessLogicLayer.BusinessObjects;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using DataTransferObjects;

namespace BusinessLogicLayerTest
{
    public class PostBLLTest
    {
        [Fact]
        public async Task GetAllPublishedPostsPaged_Should_Return_3_Pages()
        {
            //Arrange

            var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;

            var dbContext = new AppDbContext(dbOptions);

            await CreateTestDataAsync(dbContext);

            int totalPagesExpected = 3;

            var pagerDTO = new PagerParams()
            {
                RecordsPerPage = 10,
                CurrentPage = 1,
                SearchKeyWord = ""
            };


            var unitOfWork = new UnitOfWork(dbContext);

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfiles>();
            });

            var mapper = mapperConfiguration.CreateMapper();
            IDataAnnotationsValidator validator = new DataAnnotationsValidatorHelper();
            var logger = new Mock<ILogger<PostBLL>>();

            PostBLL postBLL = new PostBLL(
                unitOfWork,
                mapper,
                logger.Object,
                validator
                );

            //Act
            var response = await postBLL.GetPostsPublishedPagedAsync(pagerDTO);


            int totalPagesActual = response.PageCount;


            //Assert
            Assert.Equal(totalPagesExpected, totalPagesActual);

        }

        [Fact]
        public async Task GetAllPublishedPostsPaged_Current_Page_Should_Be_Last_Page()
        {
            //Arrange

            var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;

            var dbContext = new AppDbContext(dbOptions);

            await CreateTestDataAsync(dbContext);

            int currentPageExpected = 2;

            var pagerDTO = new PagerParams()
            {
                RecordsPerPage = 10,
                CurrentPage = 2,
                SearchKeyWord = ""
            };


            var unitOfWork = new UnitOfWork(dbContext);

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfiles>();
            });

            var mapper = mapperConfiguration.CreateMapper();
            IDataAnnotationsValidator validator = new DataAnnotationsValidatorHelper();
            var logger = new Mock<ILogger<PostBLL>>();

            PostBLL postBLL = new PostBLL(
                unitOfWork,
                mapper,
                logger.Object,
                validator
                );

            //Act
            var response = await postBLL.GetPostsPublishedPagedAsync(pagerDTO);


            int currentPageActual = response.CurrentPage;


            //Assert
            Assert.Equal(currentPageExpected, currentPageActual);

        }

        [Fact]
        public async Task GetAllPublishedPostsPaged_With_Search_Filter_Should_Return_1_Page_With_10_Records()
        {
            //Arrange

            var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;

            var dbContext = new AppDbContext(dbOptions);

            await CreateTestDataAsync(dbContext);

            int currentPageExpected = 1;
            int totalRecordsExpected = 10;
            int totalPagesExpected = 1;

            var pagerDTO = new PagerParams()
            {
                RecordsPerPage = 10,
                CurrentPage = 1,
                SearchKeyWord = "Happy"
            };


            var unitOfWork = new UnitOfWork(dbContext);

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfiles>();
            });

            var mapper = mapperConfiguration.CreateMapper();
            IDataAnnotationsValidator validator = new DataAnnotationsValidatorHelper();
            var logger = new Mock<ILogger<PostBLL>>();

            PostBLL postBLL = new PostBLL(
                unitOfWork,
                mapper,
                logger.Object,
                validator
                );

            //Act
            var response = await postBLL.GetPostsPublishedPagedAsync(pagerDTO);


            int currentPageActual = response.CurrentPage;
            int totalRecordsActual = response.RecordCount;
            int totalPagesActual = response.PageCount;


            //Assert
            Assert.Equal(currentPageExpected, currentPageActual);
            Assert.Equal(totalRecordsExpected, totalRecordsActual);
            Assert.Equal(totalPagesExpected, totalPagesActual);

        }


        [Fact]
        public async Task GetAllPublishedPostsPaged_First_Post_Should_Have_3_Comments_And_3_Votes()
        {
            //Arrange

            var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;

            var dbContext = new AppDbContext(dbOptions);

            await CreateTestDataAsync(dbContext);



            var pagerDTO = new PagerParams()
            {
                RecordsPerPage = 10,
                CurrentPage = 1,
                SearchKeyWord = ""
            };


            int totalCommentsExpected = 3;
            int totalVotesExpected = 3;


            var unitOfWork = new UnitOfWork(dbContext);

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfiles>();
            });

            var mapper = mapperConfiguration.CreateMapper();
            IDataAnnotationsValidator validator = new DataAnnotationsValidatorHelper();
            var logger = new Mock<ILogger<PostBLL>>();

            PostBLL postBLL = new PostBLL(
                unitOfWork,
                mapper,
                logger.Object,
                validator
                );

            //Act
            var response = await postBLL.GetPostsPublishedPagedAsync(pagerDTO);

            var firstPost = response.List.FirstOrDefault(t => t.Title == "Happy 1");

            int totalCommentsActual = firstPost.Comments;
            int totalVotesActual = firstPost.Votes;


            //Assert

            Assert.Equal(totalCommentsExpected, totalCommentsActual);
            Assert.Equal(totalVotesExpected, totalVotesActual);

        }

        [Fact]
        public async Task GetById_Should_Retrieve_Post()
        {
            //Arrange

            var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;

            var dbContext = new AppDbContext(dbOptions);

            await CreateTestDataAsync(dbContext);

            int postId = 1;
            string postTitleExpected = "Happy 1";
            string postTextExpected = "3 Comments 3 Votes";
            string postTypeExpected = "Any";
            string moodTypeExpected = "Happy";
            string postUserNameExpected = "TEST USER TEST USER";



            var unitOfWork = new UnitOfWork(dbContext);

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfiles>();
            });

            var mapper = mapperConfiguration.CreateMapper();
            IDataAnnotationsValidator validator = new DataAnnotationsValidatorHelper();
            var logger = new Mock<ILogger<PostBLL>>();

            PostBLL postBLL = new PostBLL(
                unitOfWork,
                mapper,
                logger.Object,
                validator
                );

            //Act
            var response = await postBLL.GetByIdAsync(postId);

            //Assert


            Assert.Equal(postTitleExpected, response.Title);
            Assert.Equal(postTextExpected, response.Text);
            Assert.Equal(postTypeExpected, response.PostType);
            Assert.Equal(moodTypeExpected, response.MoodType);
            Assert.Equal(postUserNameExpected, response.UserFullName);

        }

        [Fact]
        public async Task Approve_Post_Should_Be_Published()
        {
            //Arrange

            var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;

            var dbContext = new AppDbContext(dbOptions);

            await CreateTestDataAsync(dbContext);


            var postNotPublished = dbContext.Posts.Where(t => t.IsPublished == false).FirstOrDefault();

            int postId = postNotPublished.Id;
            bool publishedStatusExpected = true;
            string publishedDateExpected = DateTime.Now.ToShortDateString();


            var unitOfWork = new UnitOfWork(dbContext);

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfiles>();
            });

            var mapper = mapperConfiguration.CreateMapper();
            IDataAnnotationsValidator validator = new DataAnnotationsValidatorHelper();
            var logger = new Mock<ILogger<PostBLL>>();

            PostBLL postBLL = new PostBLL(
                unitOfWork,
                mapper,
                logger.Object,
                validator
                );

            //Act
            await postBLL.ApprovePostPublishAsync(postId);

            var response = await postBLL.GetByIdAsync(postId);

            string publishedDateActual = response.PublishDate.HasValue ? response.PublishDate.Value.ToShortDateString() : "";


            //Assert

            Assert.Equal(publishedStatusExpected, response.IsPublished);
            Assert.Equal(publishedDateExpected, publishedDateActual);


        }



        private async Task CreateTestDataAsync(AppDbContext dbContext)
        {
            ApplicationUserInfo userInfo = new ApplicationUserInfo();
            userInfo.UserID = Guid.NewGuid().ToString();
            userInfo.DateOfBirth = DateTime.Now;
            userInfo.FirstName = "TEST USER";
            userInfo.LastName = "TEST USER";
            userInfo.UserName = "test@test.com";

            dbContext.ApplicationUsers.Add(userInfo);

            PostType anyPostType = new PostType()
            {
                Description = "Any",
                IsAvailable = true
            };


            var moodHappy = new MoodType() { IsAvailable = true, Mood = "Happy", Posts = new List<Post>() };
            dbContext.MoodTypes.Add(moodHappy);

            var moodSad = new MoodType() { IsAvailable = true, Mood = "Sad", Posts = new List<Post>() };
            dbContext.MoodTypes.Add(moodSad);

            var moodlove = new MoodType() { IsAvailable = false, Mood = "Love", Posts = new List<Post>() };
            dbContext.MoodTypes.Add(moodlove);

            var moodBrave = new MoodType() { IsAvailable = true, Mood = "Brave", Posts = new List<Post>() };
            dbContext.MoodTypes.Add(moodBrave);

            var happyPost1WithCommentsAndVotes = new Post { MoodType = moodHappy, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Happy 1", Text = "3 Comments 3 Votes", CreationDate = DateTime.Now };




            var happyPost2WithCommentsAndVotes = new Post { MoodType = moodHappy, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Happy 2", Text = "No comments 4 Votes", CreationDate = DateTime.Now };





            var happyPost3WithCommentsAndVotes = new Post { MoodType = moodHappy, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Happy 3", Text = "2 Comments No votes", CreationDate = DateTime.Now };


            dbContext.Posts.AddRange(new List<Post>()
            {
                happyPost1WithCommentsAndVotes,
                happyPost2WithCommentsAndVotes,
                happyPost3WithCommentsAndVotes,
                new Post { MoodType = moodHappy, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Happy 4", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodHappy, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Happy 5", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodHappy, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Happy 6", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodHappy, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Happy 7", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodHappy, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Happy 8", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodHappy, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Happy 9", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodHappy, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Happy 10", Text = "Test", CreationDate = DateTime.Now },


                new Post { MoodType = moodSad, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Sad 1", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodSad, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Sad 2", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodSad, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Sad 3", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodSad, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Sad 4", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodSad, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Sad 5", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodSad, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Sad 6", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodSad, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Sad 7", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodSad, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Sad 8", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodSad, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Sad 9", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodSad, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Sad 10", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodSad, IsPublished = false, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Sad 11 [NP]", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodSad, IsPublished = false, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Sad 12 [NP]", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodSad, IsPublished = false, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Sad 13 [NP]", Text = "Test", CreationDate = DateTime.Now },

                new Post { MoodType = moodlove, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Love 1 [Mood disabled]", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodlove, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Love 2 [Mood disabled]", Text = "Test", CreationDate = DateTime.Now },


                new Post { MoodType = moodBrave, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Brave 1", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodBrave, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Brave 2", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodBrave, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Brave 3", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodBrave, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Brave 4", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodBrave, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Brave 5", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodBrave, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Brave 6", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodBrave, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Brave 7", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodBrave, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Brave 8", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodBrave, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Brave 9", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodBrave, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Brave 10", Text = "Test", CreationDate = DateTime.Now }
            });


            dbContext.PostComments.AddRange(new List<PostComment>()
            {
                new PostComment()
                {
                ApplicationUserInfo = userInfo,
                CommentDate = DateTime.Now,
                Post = happyPost1WithCommentsAndVotes,
                CommentText = ""
                },
                new PostComment()
                {
                ApplicationUserInfo = userInfo,
                CommentDate = DateTime.Now,
                Post = happyPost1WithCommentsAndVotes,
                CommentText = ""
                },
                new PostComment()
                {
                ApplicationUserInfo = userInfo,
                CommentDate = DateTime.Now,
                Post = happyPost1WithCommentsAndVotes,
                CommentText = ""
                },


                new PostComment()
                {
                ApplicationUserInfo = userInfo,
                CommentDate = DateTime.Now,
                Post = happyPost3WithCommentsAndVotes,
                CommentText = ""
                },

                new PostComment()
                {
                ApplicationUserInfo = userInfo,
                CommentDate = DateTime.Now,
                Post = happyPost3WithCommentsAndVotes,
                CommentText = ""
                },
            });


            dbContext.PostVotes.AddRange(new List<PostVote>()
            {



            new PostVote()
            {
                Post = happyPost1WithCommentsAndVotes,
                ILikedThis = true,
                VoteDate = DateTime.Now,
                ApplicationUserInfo = userInfo,
            },

            new PostVote()
            {
                Post = happyPost1WithCommentsAndVotes,
                ILikedThis = true,
                VoteDate = DateTime.Now,
                ApplicationUserInfo = userInfo,
            },

            new PostVote()
            {
                Post = happyPost1WithCommentsAndVotes,
                ILikedThis = true,
                VoteDate = DateTime.Now,
                ApplicationUserInfo = userInfo,
            },

            new PostVote()
            {
                Post = happyPost2WithCommentsAndVotes,
                ILikedThis = true,
                VoteDate = DateTime.Now,
                ApplicationUserInfo = userInfo,
            },

            new PostVote()
            {
                Post = happyPost2WithCommentsAndVotes,
                ILikedThis = true,
                VoteDate = DateTime.Now,
                ApplicationUserInfo = userInfo,
            },

            new PostVote()
            {
                Post = happyPost2WithCommentsAndVotes,
                ILikedThis = true,
                VoteDate = DateTime.Now,
                ApplicationUserInfo = userInfo,
            },


            new PostVote()
            {
                Post = happyPost2WithCommentsAndVotes,
                ILikedThis = true,
                VoteDate = DateTime.Now,
                ApplicationUserInfo = userInfo,
            }
        });



            await dbContext.SaveChangesAsync();

        }
    }
}

using AutoMapper;
using BusinessLogicLayer;
using BusinessLogicLayer.BusinessObjects;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace BusinessLogicLayerTest
{
    public class MoodTypeBLLTest
    {

        [Fact]
        public async Task GetMoodTypesTopWithPosts_Should_Return_Only_Two_Records_For_Top5()
        {
            //Arrange

            var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;

            var dbContext = new AppDbContext(dbOptions);

            await CreateTestDataAsync(dbContext);

            int expectedCount = 2;
            int top = 5;

            var unitOfWork = new UnitOfWork(dbContext);

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfiles>();
            });

            var mapper = mapperConfiguration.CreateMapper();
            IDataAnnotationsValidator validator = new DataAnnotationsValidatorHelper();
            var logger = new Mock<ILogger<MoodTypeBLL>>();

            MoodTypeBLL moodTypeBLL = new MoodTypeBLL(
                unitOfWork,
                mapper,
                logger.Object,
                validator
                );

            //Act
            var response = await moodTypeBLL.GetTopWithPostsAsync(top);


            //Assert

            int actualCount = response.List.Count();


            Assert.Equal(actualCount, expectedCount);
        }

        [Fact]
        public async Task GetMoodTypesTopWithPosts_Should_Return_Only_One_Record_For_Top1()
        {
            //Arrange

            var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;

            var dbContext = new AppDbContext(dbOptions);

            await CreateTestDataAsync(dbContext);

            int expectedCount = 1;
            int top = 1;



            var unitOfWork = new UnitOfWork(dbContext);

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfiles>();
            });

            var mapper = mapperConfiguration.CreateMapper();
            IDataAnnotationsValidator validator = new DataAnnotationsValidatorHelper();
            var logger = new Mock<ILogger<MoodTypeBLL>>();

            MoodTypeBLL moodTypeBLL = new MoodTypeBLL(
                unitOfWork,
                mapper,
                logger.Object,
                validator
                );

            //Act
            var response = await moodTypeBLL.GetTopWithPostsAsync(top);


            //Assert

            int actualCount = response.List.Count();


            Assert.Equal(actualCount, expectedCount);
        }

        [Fact]
        public async Task GetMoodTypesTopWithPosts_Return_Happy_And_Sad_MoodTypes()
        {
            //Arrange

            var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;

            var dbContext = new AppDbContext(dbOptions);

            await CreateTestDataAsync(dbContext);


            string happyMoodTypeExpected = "Happy";
            string sadMoodTypeExpected = "Sad";

            int happyMoodTypeExpectedPostCount = 4;
            int sadMoodTypeExpectedPostCount = 2;

            int top = 5;


            var unitOfWork = new UnitOfWork(dbContext);

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfiles>();
            });

            var mapper = mapperConfiguration.CreateMapper();
            IDataAnnotationsValidator validator = new DataAnnotationsValidatorHelper();
            var logger = new Mock<ILogger<MoodTypeBLL>>();

            MoodTypeBLL moodTypeBLL = new MoodTypeBLL(
                unitOfWork,
                mapper,
                logger.Object,
                validator
                );

            //Act
            var response = await moodTypeBLL.GetTopWithPostsAsync(top);

            var actualHappyMoodType = response.List.ToList()[0];
            var actualSadMoodType = response.List.ToList()[1];


            //Assert

            Assert.Equal(actualHappyMoodType.Mood, happyMoodTypeExpected);
            Assert.Equal(actualHappyMoodType.PostCount, happyMoodTypeExpectedPostCount);

            Assert.Equal(actualSadMoodType.Mood, sadMoodTypeExpected);
            Assert.Equal(actualSadMoodType.PostCount, sadMoodTypeExpectedPostCount);
        }

        [Fact]
        public async Task Insert_Should_Not_Pass_Validation()
        {
            //Arrange

            var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;

            var dbContext = new AppDbContext(dbOptions);

            var moodTypeDTO = new MoodTypeCreateDTO()
            {
                Mood = "Happy",
                IsAvailable = true,
            };

            await CreateTestDataAsync(dbContext);

            string expectedErrorMessage = ValidationErrorMessages.OnInsertAnItemAlreadyExists;

            bool isValidExpected = false;

            var unitOfWork = new UnitOfWork(dbContext);

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfiles>();
            });

            var mapper = mapperConfiguration.CreateMapper();
            IDataAnnotationsValidator validator = new DataAnnotationsValidatorHelper();
            var logger = new Mock<ILogger<MoodTypeBLL>>();

            MoodTypeBLL moodTypeBLL = new MoodTypeBLL(
                unitOfWork,
                mapper,
                logger.Object,
                validator
                );

            //Act
            var response = await moodTypeBLL.ValidateInsertAsync(moodTypeDTO);


            //Assert

            Assert.Equal(response.IsValid, isValidExpected);

            Assert.True(response.MessageList.Contains(expectedErrorMessage));
        }


        [Fact]
        public async Task Insert_Should_Pass_Validation_And_Insert_New()
        {
            //Arrange

            var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;

            var dbContext = new AppDbContext(dbOptions);

            var moodTypeDTO = new MoodTypeCreateDTO()
            {
                Mood = "New Record",
                IsAvailable = true,
            };

            await CreateTestDataAsync(dbContext);


            bool isValidExpected = true;



            var unitOfWork = new UnitOfWork(dbContext);

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfiles>();
            });

            var mapper = mapperConfiguration.CreateMapper();
            IDataAnnotationsValidator validator = new DataAnnotationsValidatorHelper();
            var logger = new Mock<ILogger<MoodTypeBLL>>();

            MoodTypeBLL moodTypeBLL = new MoodTypeBLL(
                unitOfWork,
                mapper,
                logger.Object,
                validator
                );

            //Act
            var validation = await moodTypeBLL.ValidateInsertAsync(moodTypeDTO);

            var response = await moodTypeBLL.InsertAsync(moodTypeDTO);

            int insertedRecordId = response.Id;

            var responseGetInserted = await moodTypeBLL.GetByIdAsync(insertedRecordId);


            //Assert

            Assert.Equal(validation.IsValid, isValidExpected);

            Assert.Equal(responseGetInserted.Id, insertedRecordId);

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


            var moodBrave = new MoodType() { IsAvailable = false, Mood = "Brave", Posts = new List<Post>() };
            dbContext.MoodTypes.Add(moodBrave);

            var moodBlue = new MoodType() { IsAvailable = false, Mood = "Blue", Posts = new List<Post>() };
            dbContext.MoodTypes.Add(moodBlue);

            var moodScared = new MoodType() { IsAvailable = false, Mood = "Scared", Posts = new List<Post>() };
            dbContext.MoodTypes.Add(moodScared);

            var moodHappy = new MoodType() { IsAvailable = true, Mood = "Happy", Posts = new List<Post>() };
            dbContext.MoodTypes.Add(moodHappy);

            var moodSad = new MoodType() { IsAvailable = true, Mood = "Sad", Posts = new List<Post>() };
            dbContext.MoodTypes.Add(moodSad);

            var moodlove = new MoodType() { IsAvailable = false, Mood = "Love", Posts = new List<Post>() };
            dbContext.MoodTypes.Add(moodlove);


            dbContext.Posts.AddRange(new List<Post>()
            {
                new Post { MoodType = moodHappy, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Happy 1", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodHappy, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Happy 2", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodHappy, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Happy 3", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodHappy, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Happy 4", Text = "Test", CreationDate = DateTime.Now },


                new Post { MoodType = moodSad, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Sad 1", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodSad, IsPublished = true, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Sad 2", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodSad, IsPublished = false, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Sad 3 [NP]", Text = "Test", CreationDate = DateTime.Now },
                new Post { MoodType = moodSad, IsPublished = false, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Sad 4 [NP]", Text = "Test", CreationDate = DateTime.Now },


                new Post { MoodType = moodlove, IsPublished = false, PostType = anyPostType, ApplicationUserInfo = userInfo, Title = "Love 1", Text = "Test", CreationDate = DateTime.Now }
            });


            await dbContext.SaveChangesAsync();

        }


    }
}

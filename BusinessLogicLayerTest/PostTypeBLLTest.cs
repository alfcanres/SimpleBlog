using AutoMapper;
using BusinessLogicLayer.BusinessObjects;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entity;

namespace BusinessLogicLayerTest
{
    public class PostTypeBLLTest
    {
        [Fact]
        public async Task GetByIsAvailable_Count_Should_Be_Greater_Than_Zero()
        {

            //Arrange

            var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;

            var dbContext = new AppDbContext(dbOptions);

            await CreateTestDataAsync(dbContext);


            var unitOfWork = new UnitOfWork(dbContext);

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfiles>();
            });

            var mapper = mapperConfiguration.CreateMapper();
            IDataAnnotationsValidator validator = new DataAnnotationsValidatorHelper();
            var logger = new Mock<ILogger<PostTypeBLL>>();

            PostTypeBLL postTypeBLL = new PostTypeBLL(
                unitOfWork,
                mapper,
                logger.Object,
                validator
                );

            //Act
            var response = await postTypeBLL.GetAllByIsAvailableAsync(true);


            //Assert

            int actualCount = response.List.Count();


            Assert.NotEqual(actualCount, 0);
        }

        private async Task CreateTestDataAsync(AppDbContext dbContext)
        {

            var postType1 = new PostType() { IsAvailable = false, Description = "postType1", Posts = new List<Post>() };
            dbContext.PostTypes.Add(postType1);

            var postType2 = new PostType() { IsAvailable = false, Description = "postType2", Posts = new List<Post>() };
            dbContext.PostTypes.Add(postType2);

            var postType3 = new PostType() { IsAvailable = false, Description = "postType3", Posts = new List<Post>() };
            dbContext.PostTypes.Add(postType3);

            var postType4 = new PostType() { IsAvailable = true, Description = "postType4", Posts = new List<Post>() };
            dbContext.PostTypes.Add(postType4);

            var postType5 = new PostType() { IsAvailable = true, Description = "postType5", Posts = new List<Post>() };
            dbContext.PostTypes.Add(postType5);

            var postType6 = new PostType() { IsAvailable = false, Description = "postType6", Posts = new List<Post>() };
            dbContext.PostTypes.Add(postType6);


            await dbContext.SaveChangesAsync();

        }
    }
}

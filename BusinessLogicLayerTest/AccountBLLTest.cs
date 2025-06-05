using BusinessLogicLayer.BusinessObjects;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace BusinessLogicLayerTest
{
    public class AccountBLLTest
    {
        [Fact]
        public async Task CreateAsync_With_Empty_Data_Should_Not_Be_Valid()
        {

            //Arrange
            int expectedErrorsCount = 5;

            IdentityResult identityResult = IdentityResult.Failed(new[] { new IdentityError() { Description = "Just a test" } });

            IQueryable<ApplicationUserInfo> userInfos = new List<ApplicationUserInfo>().AsQueryable();


            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(s => s.UsersInfo.Query()).Returns(userInfos);

            var userManager = new Mock<IUserManagerWrapper>();
            userManager.Setup(s => s.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<String>())).Returns(Task.FromResult(identityResult));

            IDataAnnotationsValidator validator = new DataAnnotationsValidatorHelper();
            var logger = new Mock<ILogger<AccountBLL>>();

            AccountBLL accountBLL = new AccountBLL(userManager.Object, logger.Object, validator, unitOfWork.Object);

            //Act

            var result = await accountBLL.ValidateInsertAsync(new AccountCreateDTO()
            {
                UserName = "",
                ComfirmPassword = "",
                Password = "",
                Email = "",
                FirstName = "",
                LastName = ""
            });

            string errorMessage = "Type a valid";

            //Assert
            int actualErrorsCount = result.MessageList.Where(t => t.Contains(errorMessage)).Count();

            Assert.Equal(expectedErrorsCount, actualErrorsCount);

        }

        [Fact]
        public async Task CreateAsync_With_Valid_DataShould_Be_Valid()
        {


            //Arrange
            int expectedErrorsCount = 0;
            bool expectedIsValid = true;

            IdentityResult identityResult = IdentityResult.Success;

            IQueryable<ApplicationUserInfo> userInfos = new List<ApplicationUserInfo>().AsQueryable();


            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(s => s.UsersInfo.Query()).Returns(userInfos);

            var userManager = new Mock<IUserManagerWrapper>();
            userManager.Setup(s => s.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<String>())).Returns(Task.FromResult(identityResult));

            IDataAnnotationsValidator validator = new DataAnnotationsValidatorHelper();
            var logger = new Mock<ILogger<AccountBLL>>();

            AccountBLL accountBLL = new AccountBLL(userManager.Object, logger.Object, validator, unitOfWork.Object);

            //Act

            var result = await accountBLL.ValidateInsertAsync(new AccountCreateDTO()
            {
                UserName = "alfcanres@gmail.com",
                ComfirmPassword = "Alfredo79#",
                Password = "Alfredo79#",
                Email = "alfcanres@gmail.com",
                FirstName = "Alfredo",
                LastName = "Can"
            });



            //Assert
            int actualErrorsCount = result.MessageList.Count();
            bool actualIsValid = result.IsValid;

            Assert.Equal(actualIsValid, expectedIsValid);
            Assert.Equal(expectedErrorsCount, actualErrorsCount);
        }

        [Fact]
        public async Task CreateAsync_Should_Pass()
        {


            //Arrange
            int expectedErrorsCount = 0;
            bool expectedIsValid = true;

            IdentityResult identityResult = IdentityResult.Success;

            IQueryable<ApplicationUserInfo> userInfos = new List<ApplicationUserInfo>().AsQueryable();


            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(s => s.UsersInfo.Query()).Returns(userInfos);

            var userManager = new Mock<IUserManagerWrapper>();
            userManager.Setup(s => s.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<String>())).Returns(Task.FromResult(identityResult));

            IDataAnnotationsValidator validator = new DataAnnotationsValidatorHelper();
            var logger = new Mock<ILogger<AccountBLL>>();

            AccountBLL accountBLL = new AccountBLL(userManager.Object, logger.Object, validator, unitOfWork.Object);

            //Act

            var result = await accountBLL.InsertAsync(new AccountCreateDTO()
            {
                UserName = "alfcanres@gmail.com",
                ComfirmPassword = "Alfredo79#",
                Password = "Alfredo79#",
                Email = "alfcanres@gmail.com",
                FirstName = "Alfredo",
                LastName = "Can"
            });



            //Assert
            Assert.NotNull(result);
        }

    }
}
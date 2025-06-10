using DataTransferObjects.DTO;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using WebAPI.Options;
using Xunit;


namespace WebAPI.Test
{
    public class InitialDataOptionsValidatorTest
    {
        [Fact]
        public void Should_pass_validation()
        {
            var initialDataOptions = new InitialDataOptions
            {
                AdminUser = new AdminUserOptions
                {
                    UserName = "test@test.com",
                    Email = "test@test.com",
                    Password = "test",
                    Role = "test"
                },
                MoodTypes = new MoodTypesOptions
                {
                    Create = true,
                    MoodTypes = new List<string>
                    {
                        "Happy",
                        "Sad",
                        "Angry",
                        "Excited",
                        "Bored"
                    }
                }
            };



            var services = new ServiceCollection();

            services.AddSingleton<
                IValidateOptions<InitialDataOptions>,
                InitialDataOptionsValidator>(); // <-- Register the validator for InitialDataOptions

            services.AddOptions<InitialDataOptions>()
                .Configure(o=> o.AdminUser = initialDataOptions.AdminUser)
                .ValidateOnStart() // eager validation 
            ;
            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetRequiredService<IOptionsMonitor<InitialDataOptions>>();
            Assert.Equal("test@test.com", options.CurrentValue.AdminUser.UserName);
        }

        [Fact]
        public void Should_fail_validation()
        {
            string expectedErrorMessage = "Admin user options are not properly configured.";

            var initialDataOptions = new InitialDataOptions
            {
                AdminUser = new AdminUserOptions
                {
                    UserName = "",
                    Email = "",
                    Password = "",
                    Role = ""
                },
                MoodTypes = new MoodTypesOptions
                {
                    Create = false,
                    MoodTypes = new List<string>()
                },
                PostTypes = new PostTypesOptions
                {
                    Create = true,
                    PostTypes = new List<string>()                    
                },
                DummyUsers = new DummyUsersOptions
                {
                    Create = true,
                    Count = 15,
                    DefaultPassword = "",
                    DummyNames = new List<string>(),
                    DummyLastNames = new List<string>()
                },
                DummyPosts = new DummyPostsOptions
                {
                    Create = true,
                    Count = 500,
                    DummyPostTitles = new List<string>(),
                    DummyPostContents = new List<string>()
                }
            };

            var services = new ServiceCollection();
            services.AddSingleton<
                IValidateOptions<InitialDataOptions>,
                InitialDataOptionsValidator>(); // <-- Register the validator for InitialDataOptions

            services.AddOptions<InitialDataOptions>()
                 .Configure(o => o = initialDataOptions)
                 .ValidateOnStart() // eager validation 
             ;
            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetRequiredService<IOptionsMonitor<InitialDataOptions>>();
            var error = Assert.Throws<OptionsValidationException>(() => options.CurrentValue);

            Assert.Equal(error.Message, expectedErrorMessage);

        }
    }
}

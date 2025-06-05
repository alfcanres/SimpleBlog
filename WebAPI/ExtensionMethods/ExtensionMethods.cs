using DataAccessLayer;
using DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using WebAPI.Options;

namespace WebAPI.ExtensionMethods
{
    public static class ExtensionMethods
    {
        public static ValidatorResponse ToValidate(this ModelStateDictionary modelState)
        {
            ValidatorResponse validate = new ValidatorResponse();
            if (!modelState.IsValid)
            {
                validate.IsValid = false;
                foreach (var key in modelState.Keys)
                {
                    var _modelState = modelState[key];
                    foreach (var error in _modelState.Errors)
                    {
                        validate.MessageList.Add(error.ErrorMessage);
                    }
                }
            }

            return validate;

        }

        public static IApplicationBuilder UseDataInitializer(this IApplicationBuilder app)
        {

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var appDContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var initialDataOptions = scope.ServiceProvider.GetRequiredService<IOptions<InitialDataOptions>>();
                var initialData = initialDataOptions.Value;


                DbInitializer.CreateAdminUser(
                    initialData.AdminUser.UserName,
                    initialData.AdminUser.Email,
                    initialData.AdminUser.Password,
                    initialData.AdminUser.Role,
                    userManager,
                    roleManager).
                    GetAwaiter().
                    GetResult();



                DbInitializer.CreateMoodTypesAndPostTypes(
                    appDContext,
                    initialData.MoodTypes.Create,
                    initialData.MoodTypes.MoodTypes,
                    initialData.PostTypes.Create,
                    initialData.PostTypes.PostTypes
                    ).
                    GetAwaiter().
                    GetResult();


                if (initialData.DummyUsers.Create)
                    DbInitializer.CreateDummyUsers(
                        appDContext,
                        userManager,
                        initialData.DummyUsers.Count,
                        initialData.DummyUsers.DummyNames,
                        initialData.DummyUsers.DummyLastNames,
                        initialData.DummyUsers.DefaultPassword
                        ).
                        GetAwaiter().
                        GetResult();

                if (initialData.DummyPosts.Create)
                    DbInitializer.CreateDummyPosts(
                        appDContext,
                        initialData.DummyPosts.Count,
                        initialData.DummyPosts.DummyPostTitles,
                        initialData.DummyPosts.DummyPostContents
                        ).
                        GetAwaiter().
                        GetResult();

            }



            return app;
        }


    }

}

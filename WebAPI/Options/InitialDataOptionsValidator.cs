using DataAccessLayer.Entity;
using Microsoft.Extensions.Options;

namespace WebAPI.Options
{
    public class InitialDataOptionsValidator : IValidateOptions<InitialDataOptions>
    {
        public ValidateOptionsResult Validate(string name, InitialDataOptions options)
        {
            string adminUser = options.AdminUser?.UserName;
            string adminEmail = options.AdminUser?.Email;
            string adminPassword   = options.AdminUser?.Password;
            string adminRole = options.AdminUser?.Role;

            if (string.IsNullOrWhiteSpace(adminUser) || string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword) || string.IsNullOrWhiteSpace(adminRole))
            {
                return ValidateOptionsResult.Fail("Admin user options are not properly configured.");
            }

            bool createmoodTypes = options.MoodTypes?.Create ?? false;
            IEnumerable<string> moodTypes = options.MoodTypes?.MoodTypes ?? Enumerable.Empty<string>();
            bool createPostTypes = options.PostTypes?.Create ?? false;
            IEnumerable<string> postTypes = options.PostTypes?.PostTypes ?? Enumerable.Empty<string>();

            if (createmoodTypes && !moodTypes.Any())
            {
                return ValidateOptionsResult.Fail("Mood types are not provided for creation.");
            }

            if (createPostTypes && !postTypes.Any())
            {
                return ValidateOptionsResult.Fail("Post types are not provided for creation.");
            }


            int dummyUserCount = options.DummyUsers?.Count ?? 0;
            IEnumerable<string> dummyNames = options.DummyUsers?.DummyNames ?? Enumerable.Empty<string>();
            IEnumerable<string> dummyLastNames = options.DummyUsers?.DummyLastNames ?? Enumerable.Empty<string>();
            string defaultPasswordForDummyUsers = options.DummyUsers?.DefaultPassword ?? string.Empty;

            if(dummyUserCount > 0 && (dummyNames == null || !dummyNames.Any() || dummyLastNames == null || !dummyLastNames.Any()))
            {
                return ValidateOptionsResult.Fail("Dummy names and last names cannot be null or empty when creating dummy users.");
            }

            if (dummyUserCount > 0 && string.IsNullOrWhiteSpace(defaultPasswordForDummyUsers))
            {
                return ValidateOptionsResult.Fail("Default password for dummy users cannot be null or empty when creating dummy users.");
            }

            int dummyPostCount = options.DummyPosts?.Count ?? 0;
            IEnumerable<string> dummyPostTitles = options.DummyPosts?.DummyPostTitles ?? Enumerable.Empty<string>();
            IEnumerable<string> dummyPostContents = options.DummyPosts?.DummyPostContents ?? Enumerable.Empty<string>();

            if(dummyPostCount > 0 && (!createmoodTypes || !createPostTypes || dummyUserCount <= 0))
            {
                return ValidateOptionsResult.Fail("Dummy post can only be created if you previously configured dummy users, mood types and post types");
            }
            else
            {
                if (dummyPostCount > 0 && (dummyPostTitles == null || !dummyPostTitles.Any() || dummyPostContents == null || !dummyPostContents.Any()))
                {
                    return ValidateOptionsResult.Fail("Dummy post titles and contents cannot be null or empty when creating dummy posts.");
                }

            }


            return ValidateOptionsResult.Success;

        }
    }
}

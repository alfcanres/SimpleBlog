using Microsoft.AspNetCore.Identity;

namespace BusinessLogicLayer.Interfaces
{
    public interface IUserManagerWrapper
    {
        Task<IdentityUser> FindByEmailAsync(string email);

        Task<IdentityUser> FindByNameAsync(string userName);

        Task<IdentityUser> FindByIdAsync(string userName);

        Task<IdentityResult> CreateAsync(IdentityUser appUser, string password);

        Task<bool> CheckPasswordAsync(IdentityUser appUser, string password);

        Task<IdentityResult> DeleteAsync(IdentityUser appUser);

        Task<IdentityResult> ChangePasswordAsync(IdentityUser appUser, string oldPassword, string newPassword);
    }
}

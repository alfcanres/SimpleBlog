using DataTransferObjects;
using DataTransferObjects.DTO;

namespace BusinessLogicLayer.BusinessObjects
{
    public interface IAccountBLL
    {
        Task<AccountRegisteredDTO> InsertAsync(AccountCreateDTO createDTO);
        Task<AccountReadDTO> GetByUserNameOrEmail(string userNameOrEmail);
        public Task<ValidatorResponse> SignInAsync(AccountSignInDTO userSignInDTO);
        Task UpdatePasswordAsync(AccountChangePasswordDTO updateDTO);
        Task<ValidatorResponse> ValidateInsertAsync(AccountCreateDTO createDTO);
        Task<ValidatorResponse> ValidateUpdatePasswordAsync(AccountChangePasswordDTO updateDTO);
        

    }
}

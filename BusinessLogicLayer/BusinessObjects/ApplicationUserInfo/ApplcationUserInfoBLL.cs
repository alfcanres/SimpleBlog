using AutoMapper;
using BusinessLogicLayer.BusinessObject;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects;
using DataTransferObjects.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.BusinessObjects
{
    public class ApplicationUserInfoBLL : BaseBLL<ApplicationUserInfoCreateDTO, ApplicationUserInfoReadDTO, ApplicationUserInfoUpdateDTO>, IApplicationUserInfoBLL
    {
        private readonly IUserManagerWrapper _userManagerWrapper;
        public ApplicationUserInfoBLL(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<ApplicationUserInfoBLL> logger,
            IDataAnnotationsValidator dataAnnotationsValidator,
            IUserManagerWrapper userManagerWrapper
            ) : base(unitOfWork, mapper, logger, dataAnnotationsValidator)
        {
            _userManagerWrapper = userManagerWrapper;
        }

        public async Task<ResponsePagedList<ApplicationUserInfoListDTO>> GetAllActivePagedAsync(PagerParams pagerDTO)
        {

            return await ExecutePagedListAsync(new GetAllUsersInfoActivePaged(UnitOfWork, Mapper, pagerDTO), pagerDTO);
        }

        public async Task<ResponsePagedList<ApplicationUserInfoListDTO>> GetAllPagedAsync(PagerParams pagerDTO)
        {
            return await ExecutePagedListAsync(new GetAllUsersInfoPaged(UnitOfWork, Mapper, pagerDTO), pagerDTO);
        }

        public async Task<ResponseList<ApplicationUserInfoListDTO>> GetTopWithPostsAsync(int top)
        {
            return await ExecuteListAsync(new GetTopUsersInfoWithPosts(UnitOfWork, Mapper, top));
        }

        protected override async Task ExecuteDeleteAsync(int id)
        {
            await UnitOfWork.UsersInfo.DeleteAsync(id);
        }

        protected override async Task<ApplicationUserInfoReadDTO> ExecuteGetByIdAsync(int id)
        {

            var entity = await UnitOfWork.UsersInfo.GetByIdAsync(id);


            return await MapApplicationUserInfoReadDTO(entity);
        }

        protected override async Task<ApplicationUserInfoReadDTO> ExecuteInsertAsync(ApplicationUserInfoCreateDTO createDTO)
        {
            ApplicationUserInfo entity = Mapper.Map<ApplicationUserInfo>(createDTO);

            await UnitOfWork.UsersInfo.InsertAsync(entity);

            return await MapApplicationUserInfoReadDTO(entity);
        }

        protected override async Task<ApplicationUserInfoReadDTO> ExecuteUpdateAsync(ApplicationUserInfoUpdateDTO updateDTO)
        {
            var entity = await UnitOfWork.UsersInfo.GetByIdAsync(updateDTO.Id);
            Mapper.Map(updateDTO, entity);
            await UnitOfWork.UsersInfo.UpdateAsync(entity);

            return await MapApplicationUserInfoReadDTO(entity);
        }

        protected override async Task ExecValidateDeleteAsync(int id)
        {
            bool exists = await UnitOfWork.UsersInfo.Query().Where(t => t.Id == id).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnDeleteNoRecordWasFound);
            }
        }

        protected override async Task ExecValidateInsertAsync(ApplicationUserInfoCreateDTO createDTO)
        {
            bool exists = await UnitOfWork.UsersInfo.Query().Where(t => t.UserName == createDTO.UserName).AnyAsync();
            if (exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnInsertAnItemAlreadyExists);
            }
        }

        protected override async Task ExecValidateUpdateAsync(int id, ApplicationUserInfoUpdateDTO updateDTO)
        {
            bool exists = await UnitOfWork.UsersInfo.Query().Where(t => t.Id == updateDTO.Id).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnUpdateNoRecordWasFound);
            }

            //TODO apply a business rule for those elements trying to change it's username to an already existent user name
        }

        public async Task<ApplicationUserInfoReadDTO> GetByUserEmailAsync(string email)
        {
            var user = await _userManagerWrapper.FindByEmailAsync(email);

            var entity = await UnitOfWork.UsersInfo.Query().Where(t => t.UserID == user.Id).FirstOrDefaultAsync();

            return await MapApplicationUserInfoReadDTO(entity);
        }

        private async Task<ApplicationUserInfoReadDTO> MapApplicationUserInfoReadDTO(ApplicationUserInfo entity)
        {
            var identityUser = await _userManagerWrapper.FindByIdAsync(entity.UserID);

            var dto = new ApplicationUserInfoReadDTO()
            {
                DateOfBirth = entity.DateOfBirth,
                FirstName = entity.FirstName,
                Id = entity.Id,
                IsActive = entity.IsActive,
                LastName = entity.LastName,
                ProfilePicture = entity.ProfilePicture,
                UserName = entity.UserName,
                UserID = entity.UserID
            };
            return dto;
        }
    }
}

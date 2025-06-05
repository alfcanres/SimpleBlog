using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects;
using DataTransferObjects.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace BusinessLogicLayer.BusinessObjects
{
    public class PostTypeBLL : CRUDBaseBLL<PostType, PostTypeCreateDTO, PostTypeReadDTO, PostTypeUpdateDTO>, IPostTypeBLL
    {
        private readonly IUnitOfWork _unitOfWork;
        public PostTypeBLL(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PostTypeBLL> logger, IDataAnnotationsValidator dataAnnotationsValidator) : base(unitOfWork.PostTypes, mapper, logger, dataAnnotationsValidator)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CountAllAsync()
        {
            return await _unitOfWork.PostTypes.Query().CountAsync();
        }

        public async Task<ResponseList<PostTypeReadDTO>> GetAllAsync()
        {
            return await ExecuteListAsync(new GetAllPostTypes(_unitOfWork, Mapper));
        }

        public async Task<ResponsePagedList<PostTypeReadDTO>> GetAllPagedAsync(PagerParams pagerDTO)
        {
            return await ExecutePagedListAsync(new GetAllPostTypePaged(_unitOfWork, Mapper, pagerDTO), pagerDTO);
        }

        public async Task<ResponseList<PostTypeReadDTO>> GetTopWithPostsAsync(int top)
        {
            return await ExecuteListAsync(new GetPostTypeTopWithPosts(_unitOfWork, Mapper, top));
        }

        public async Task<ResponseList<PostTypeReadDTO>> GetAllByIsAvailableAsync(bool isAvailable)
        {
            return await ExecuteListAsync(new GetAllPostTypeByIsAvailable(_unitOfWork, Mapper, isAvailable));
        }


        protected override async Task ExecValidateDeleteAsync(int id)
        {
            bool exists = await _unitOfWork.PostTypes.Query().Where(t => t.Id == id).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnDeleteNoRecordWasFound);
            }
        }

        protected override async Task ExecValidateInsertAsync(PostTypeCreateDTO createDTO)
        {
            bool exists = await _unitOfWork.PostTypes.Query().Where(t => t.Description == createDTO.Description).AnyAsync();
            
               if (exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnInsertAnItemAlreadyExists);
            }
        }

        protected override async Task ExecValidateUpdateAsync(int id, PostTypeUpdateDTO updateDTO)
        {
            bool exists = await _unitOfWork.PostTypes.Query().Where(t => t.Id == updateDTO.Id).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnUpdateNoRecordWasFound);
            }
        }

    }
}

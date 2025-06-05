using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects;
using DataTransferObjects.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.BusinessObjects
{
    public class PostCommentBLL : CRUDBaseBLL<PostComment, PostCommentCreateDTO, PostCommentReadDTO, PostCommentUpdateDTO>, IPostCommentBLL
    {
        IUnitOfWork _unitOfWork;
        public PostCommentBLL(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PostCommentBLL> logger, IDataAnnotationsValidator dataAnnotationsValidator) : base(unitOfWork.PostComments, mapper, logger, dataAnnotationsValidator)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<ResponsePagedList<PostCommentReadDTO>> GetPagedByPostIdAsync(int postID, PagerParams pagerDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseList<PostCommentReadDTO>> GetByPostIdAsync(int postID)
        {
            var query = _unitOfWork.PostComments.Query().Where(t => t.PostId == postID);

            var list = await query.Select(t => new PostCommentReadDTO
            {
                Id = t.Id,
                CommentText = t.CommentText,
                ApplicationUserInfoId = t.ApplicationUserInfoId,
                PostId = t.PostId,
                CommentDate = t.CommentDate,
                ProfilePicture = t.ApplicationUserInfo.ProfilePicture,
                UserName = t.ApplicationUserInfo.UserName                
            }).ToListAsync();

            return new ResponseList<PostCommentReadDTO>(list);
        }

        protected override async Task ExecValidateInsertAsync(PostCommentCreateDTO createDTO)
        {
            bool exists = await _unitOfWork.PostComments.Query().
                Where(
                t =>
                t.CommentText == createDTO.CommentText
                &&
                t.ApplicationUserInfoId == createDTO.ApplicationUserInfoId
                &&
                t.PostId == createDTO.PostId
                ).AnyAsync();

            if (exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnInsertAnItemAlreadyExists);
            }
        }

        protected override async Task ExecValidateUpdateAsync(int id, PostCommentUpdateDTO updateDTO)
        {
            bool exists = await _unitOfWork.PostComments.Query().Where(t => t.Id == updateDTO.Id).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnUpdateNoRecordWasFound);
            }
        }

        protected override async Task ExecValidateDeleteAsync(int id)
        {
            bool exists = await _unitOfWork.PostComments.Query().Where(t => t.Id == id).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnDeleteNoRecordWasFound);
            }
        }
    }
}

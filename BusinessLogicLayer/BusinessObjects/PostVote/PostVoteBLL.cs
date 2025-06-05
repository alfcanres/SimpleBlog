using AutoMapper;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects;
using DataTransferObjects.DTO;
using DataTransferObjects.DTO.PostVote;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.BusinessObjects
{
    public class PostVoteBLL : IPostVoteBLL
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected readonly ValidatorResponse _validate = new ValidatorResponse();
        protected readonly ILogger _logger;
        public PostVoteBLL(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PostVoteBLL> logger, IDataAnnotationsValidator dataAnnotationsValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PostVoteViewDTO> GetVotesByPostIdAsync(int PostId)
        {
            _validate.Clear();

            try
            {
                var likes = await _unitOfWork.PostVotes.Query().Where(t => t.PostId == PostId && t.ILikedThis).CountAsync();
                var dislikes = await _unitOfWork.PostVotes.Query().Where(t => t.PostId == PostId && !t.ILikedThis).CountAsync();

                PostVoteViewDTO postview = new PostVoteViewDTO()
                {
                    PostId = PostId,
                    Dislike = dislikes,
                    Like = likes
                };



                return postview;
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validate.IsValid = false;
                _validate.AddError(friendlyError);
                _logger.LogError(ex, "EXECUTE LIST ERROR GetVotesByPostIdAsync");
                return null;
            }

        }

        public async Task<PostVoteViewDTO> SubmitVoteAsync(SubmitVoteDTO submitVoteDTO)
        {
            var result = await _unitOfWork.PostVotes.Query().
                 Where(t =>
                     t.ApplicationUserInfoId == submitVoteDTO.ApplicationUserInfoId
                     &&
                     t.PostId == submitVoteDTO.PostId).
                     FirstOrDefaultAsync();

            if (result != null)
            {
                result.ILikedThis = submitVoteDTO.ILikedThis;
                result.VoteDate = DateTime.Now;
                await _unitOfWork.PostVotes.UpdateAsync(result);
                
            }
            else
            {
                PostVote entity = new PostVote()
                {
                    ApplicationUserInfoId = submitVoteDTO.ApplicationUserInfoId,
                    PostId = submitVoteDTO.PostId,
                    ILikedThis = submitVoteDTO.ILikedThis,
                    VoteDate = DateTime.Now,
                };
                await _unitOfWork.PostVotes.InsertAsync(entity);

            }

            return await GetVotesByPostIdAsync(submitVoteDTO.PostId);

        }

        public async Task<ValidatorResponse> ValidateSubmitVoteAsync(SubmitVoteDTO submitVoteDTO)
        {
           var result = await _unitOfWork.PostVotes.Query().
                Where(t => 
                    t.ApplicationUserInfoId == submitVoteDTO.ApplicationUserInfoId 
                    && 
                    t.PostId == submitVoteDTO.PostId).
                    FirstOrDefaultAsync();

            if (result != null)
            {
                if(result.ILikedThis == submitVoteDTO.ILikedThis)
                {
                    _validate.AddError(Helpers.ValidationPostErrorMessages.AlreadyVotedForThisPost);
                }   
            }

            return _validate;
        }


    }
}

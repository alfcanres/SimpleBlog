using DataTransferObjects;
using DataTransferObjects.DTO;
using DataTransferObjects.DTO.PostVote;



namespace BusinessLogicLayer.BusinessObjects
{
    public interface IPostVoteBLL 
    {

        Task<PostVoteViewDTO> GetVotesByPostIdAsync(int PostId);
        Task<PostVoteViewDTO> SubmitVoteAsync(SubmitVoteDTO submitVoteDTO);
        Task<ValidatorResponse> ValidateSubmitVoteAsync(SubmitVoteDTO submitVoteDTO);
    }
}

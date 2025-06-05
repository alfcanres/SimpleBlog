using BusinessLogicLayer.BusinessObjects;
using BusinessLogicLayer.Helpers;
using DataTransferObjects;
using DataTransferObjects.DTO;
using DataTransferObjects.DTO.PostVote;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostController : BaseController<PostCreateDTO, PostReadDTO, PostUpdateDTO>
    {
        private readonly IPostBLL _BLL;
        private readonly IPostCommentBLL _postCommentBLL;
        private readonly IPostVoteBLL _postVoteBLL;
        private readonly ILogger<PostController> _logger;

        public PostController(IPostBLL BLL, IPostCommentBLL postCommentBLL, IPostVoteBLL postVoteBLL, ILogger<PostController> logger) : base(BLL, logger)
        {
            _BLL = BLL;
            _logger = logger;
            _postCommentBLL = postCommentBLL;
            _postVoteBLL = postVoteBLL;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PostCreateDTO createModel)
        {
            return await CreateAsync(createModel);
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] PostUpdateDTO updateModel)
        {
            return await UpdateAsync(updateModel.Id, updateModel);
        }


        [HttpPut("Approve/{id}")]
        public async Task<ActionResult> Put(int id)
        {

            try
            {
                var validate = await _BLL.ValidateApprovePostPublishAsync(id);
                if (!validate.IsValid)
                {
                    return BadRequest(validate);
                }

                await _BLL.ApprovePostPublishAsync(id);

                return Ok();
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "Approve OPERATION : {id}", id);

                return StatusCode(500, _validateDTO);
            }

        }



        [ResponseCache(Duration = 10)]
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            return await GetByIdAsync(id);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await DeleteAsync(id);
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 10)]
        [HttpGet("PublishedPaged")]
        public async Task<ActionResult> GetPublishedPaged([FromQuery] PagerParams pagerDTO)
        {
            try
            {
                var response = await _BLL.GetPostsPublishedPagedAsync(pagerDTO);

                return Ok(response);

            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "GET PublishedPaged : {pagerDTO}", pagerDTO);

                return StatusCode(500, _validateDTO);
            }

        }


        [ResponseCache(Duration = 10)]
        [HttpGet("PublishedPagedByUser/{id}")]
        public async Task<ActionResult> GetPaged(int id, [FromQuery] PagerParams pagerDTO)
        {
            try
            {
                var response = await _BLL.GetPostsPublishedByUserPagedAsync(id, pagerDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "GET PublishedPagedByUser : {pagerDTO}", pagerDTO);

                return StatusCode(500, _validateDTO);
            }

        }

        [ResponseCache(Duration = 10)]
        [HttpGet("PageByUser/{id}")]
        public async Task<ActionResult> GetPagedByUser(int id, [FromQuery] PagerParams pagerDTO)
        {
            try
            {
                var response = await _BLL.GetAllPostByUserPagedAsync(id, pagerDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "GET GetAllPostByUserPagedAsync : {pagerDTO}", pagerDTO);

                return StatusCode(500, _validateDTO);
            }

        }



        [ResponseCache(Duration = 10)]
        [HttpGet("PendingPublishPaged")]
        public async Task<ActionResult> GetNotPublished([FromQuery] PagerParams pagerDTO)
        {
            try
            {
                var response = await _BLL.GetAllPostsNotPublishedAsync(pagerDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "GET PendingPublishPaged : {pagerDTO}", pagerDTO);

                return StatusCode(500, _validateDTO);
            }
        }


        [HttpGet("{id}/Comments")]
        public async Task<ActionResult> GetComments(int id)
        {
            try
            {
                var response = await _postCommentBLL.GetByPostIdAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "GET Comments : {id}", id);

                return StatusCode(500, _validateDTO);
            }

        }

        [HttpGet("{id}/Votes")]
        public async Task<ActionResult> GetVotes(int id)
        {
            try
            {
                var response = await _postVoteBLL.GetVotesByPostIdAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "GET Votes : {id}", id);

                return StatusCode(500, _validateDTO);
            }

        }

        [HttpPost("{id}/SubmitVote")]
        public async Task<ActionResult> SubmitVote(SubmitVoteDTO submitVoteDTO)
        {
            try
            {
                var validate = await _postVoteBLL.ValidateSubmitVoteAsync(submitVoteDTO);
                if (!validate.IsValid)
                {
                    return BadRequest(validate);
                }
                var response = await _postVoteBLL.SubmitVoteAsync(submitVoteDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "INSERT OPERATION : {submitVoteDTO}", submitVoteDTO);

                return StatusCode(500, _validateDTO);
            }

        }


    }
}

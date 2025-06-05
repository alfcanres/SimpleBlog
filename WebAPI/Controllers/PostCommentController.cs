using BusinessLogicLayer.BusinessObjects;
using BusinessLogicLayer.Helpers;
using DataTransferObjects;
using DataTransferObjects.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostCommentController : Controller
    {
        private readonly IPostCommentBLL _BLL;
        private readonly ILogger<PostCommentController> _logger;
        private readonly ValidatorResponse _validateDTO;

        public PostCommentController(IPostCommentBLL BLL, ILogger<PostCommentController> logger)
        {
            _BLL = BLL;
            _logger = logger;
            _validateDTO = new ValidatorResponse();   
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PostCommentCreateDTO createModel)
        {

            try
            {
                var validate = await _BLL.ValidateInsertAsync(createModel);
                if (!validate.IsValid)
                {
                    return BadRequest(validate);
                }
                var response = await _BLL.InsertAsync(createModel);
                return Ok(response);
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "INSERT OPERATION : {createDTO}", createModel);

                return StatusCode(500, _validateDTO);
            }
        }

        [ResponseCache(Duration = 10)]
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var response = await _BLL.GetByIdAsync(id);
                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "GET BY ID OPERATION : {id}", id);

                return StatusCode(500, _validateDTO);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var validate = await _BLL.ValidateDeleteAsync(id);
                if (!validate.IsValid)
                {
                    return BadRequest(validate);
                }

                await _BLL.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "DELETE OPERATION : {ID}", id);

                return StatusCode(500, _validateDTO);
            }
        }


        [ResponseCache(Duration = 10)]
        [HttpGet("GetByPostIdAsync/{id}")]
        public async Task<ActionResult> GetByPostIdAsync(int id)
        {
            try
            {
                var response = await _BLL.GetByPostIdAsync(id);
                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "GET BY ID OPERATION : {id}", id);

                return StatusCode(500, _validateDTO);
            }
        }


    }
}

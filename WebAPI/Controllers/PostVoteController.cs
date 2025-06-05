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
    public class PostVoteController : Controller
    {
        private readonly IPostVoteBLL _BLL;
        private readonly ILogger<PostVoteController> _logger;
        private readonly ValidatorResponse _validateDTO;

        public PostVoteController(IPostVoteBLL BLL, ILogger<PostVoteController> logger)
        {
            _BLL = BLL;
            _logger = logger;
            _validateDTO = new ValidatorResponse();
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PostVoteCreateDTO createModel)
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

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] PostVoteUpdateDTO updateModel)
        {
            try
            {
                var validate = await _BLL.ValidateUpdateAsync(updateModel.Id, updateModel);
                if (!validate.IsValid)
                {
                    return BadRequest(validate);
                }

                var response = await _BLL.UpdateAsync(updateModel.Id, updateModel);

                return Ok(response);
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "UPDATE OPERATION : {updateDTO}", updateModel);

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


    }
}

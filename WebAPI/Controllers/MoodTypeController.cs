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
    public class MoodTypeController : BaseController<MoodTypeCreateDTO, MoodTypeReadDTO, MoodTypeUpdateDTO>
    {
        private readonly IMoodTypeBLL _BLL;
        private readonly ILogger<MoodTypeController> _logger;

        public MoodTypeController(IMoodTypeBLL BLL, ILogger<MoodTypeController> logger) : base(BLL, logger)
        {
            _BLL = BLL;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MoodTypeCreateDTO createModel)
        {
            return await CreateAsync(createModel);
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] MoodTypeUpdateDTO updateModel)
        {
            return await UpdateAsync(updateModel.Id, updateModel);
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

        [ResponseCache(Duration = 10)]
        [HttpGet("Available/{isAvalable}")]
        public async Task<ActionResult> GetIsAvailable(bool isAvalable)
        {

            try
            {
                var response = await _BLL.GetAllByIsAvailableAsync(isAvalable);

                return Ok(response);

            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "GET BY Available : {isAvalable}", isAvalable);

                return StatusCode(500, _validateDTO);
            }
        }


        [HttpGet("Paged")]
        public async Task<ActionResult> GetPaged([FromQuery] PagerParams pagerDTO)
        {

            try
            {
                var response = await _BLL.GetAllPagedAsync(pagerDTO);

                return Ok(response);

            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "GET PAGED : {pagerDTO}", pagerDTO);

                return StatusCode(500, _validateDTO);
            }

        }


        [ResponseCache(Duration = 10)]
        [HttpGet("Top/{top}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetTop(int top)
        {
            try
            {
                var response = await _BLL.GetTopWithPostsAsync(top);

                return Ok(response);

            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "GET TOP : {pagerDTO}", top);

                return StatusCode(500, _validateDTO);
            }

        }


    }
}

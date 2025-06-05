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
    public class PostTypeController : BaseController<PostTypeCreateDTO, PostTypeReadDTO, PostTypeUpdateDTO>
    {
        private readonly IPostTypeBLL _BLL;
        private readonly ILogger<PostTypeController> _logger;

        public PostTypeController(IPostTypeBLL BLL, ILogger<PostTypeController> logger) : base(BLL, logger)
        {
            _BLL = BLL;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PostTypeCreateDTO createModel)
        {
            return await CreateAsync(createModel);
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] PostTypeUpdateDTO updateModel)
        {
            return await UpdateAsync(updateModel.Id, updateModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await DeleteAsync(id);
        }

  
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
                _logger.LogError(ex, "GET Available : {isAvalable}", isAvalable);

                return StatusCode(500, _validateDTO);
            }
        }


        [HttpGet("CountAll")]
        public async Task<ActionResult> CountAll()
        {

            try
            {
                var response = await _BLL.CountAllAsync();

                return Ok(response);

            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "COUNT ALL ");

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
        [HttpGet("{id}")]
        public async Task<ActionResult<PostTypeReadDTO>> Get(int id)
        {
            return await GetByIdAsync(id);
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
                _logger.LogError(ex, "GET Top : {top}", top);

                return StatusCode(500, _validateDTO);
            }
        }


    }
}

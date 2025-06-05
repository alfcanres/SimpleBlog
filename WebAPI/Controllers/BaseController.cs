using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public abstract class BaseController<CreateDTO, ReadDTO, UpdateDTO> : ControllerBase
        where CreateDTO : class
        where ReadDTO : class
        where UpdateDTO : class
    {
        protected readonly IBLL<CreateDTO, ReadDTO, UpdateDTO> _BLL;
        private readonly ILogger _logger;
        protected readonly ValidatorResponse _validateDTO;

        protected BaseController(IBLL<CreateDTO, ReadDTO, UpdateDTO> BLL, ILogger logger)
        {
            _BLL = BLL;
            _logger = logger;
            _validateDTO = new ValidatorResponse();
        }

        protected async Task<ActionResult> GetByIdAsync(int id)
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

        protected async Task<ActionResult> CreateAsync(CreateDTO createDTO)
        {
            try
            {
                var validate = await _BLL.ValidateInsertAsync(createDTO);
                if (!validate.IsValid)
                {
                    return BadRequest(validate);
                }
                var response = await _BLL.InsertAsync(createDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "INSERT OPERATION : {createDTO}", createDTO);

                return StatusCode(500, _validateDTO);
            }
        }

        protected async Task<ActionResult> UpdateAsync(int id, UpdateDTO updateDTO)
        {

            try
            {
                var validate = await _BLL.ValidateUpdateAsync(id, updateDTO);
                if (!validate.IsValid)
                {
                    return BadRequest(validate);
                }

                var response = await _BLL.UpdateAsync(id, updateDTO);

                return Ok(response);
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "UPDATE OPERATION : {updateDTO}", updateDTO);

                return StatusCode(500, _validateDTO);
            }
        }

        protected async Task<ActionResult> DeleteAsync(int id)
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

using BusinessLogicLayer.BusinessObjects;
using BusinessLogicLayer.Helpers;
using DataTransferObjects;
using DataTransferObjects.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Options;


namespace WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : ControllerBase
    {
        private readonly IAccountBLL _accountBLL;
        private readonly ILogger<AccountController> _logger;
        private readonly JwtAppOptions _jwtOptions;
        private readonly ValidatorResponse _validateDTO;

        public AccountController(IAccountBLL accountBLL, ILogger<AccountController> logger, IOptions<JwtAppOptions> jwtOptions)
        {
            _accountBLL = accountBLL;
            _logger = logger;
            _validateDTO = new ValidatorResponse();
            _jwtOptions = jwtOptions.Value;
        }


        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult> Post([FromBody] AccountCreateDTO createModel)
        {
            try
            {
                var validate = await _accountBLL.ValidateInsertAsync(createModel);
                if (!validate.IsValid)
                {
                    return BadRequest(validate);
                }
                var response = await _accountBLL.InsertAsync(createModel);

                if (!response.ValidateDTO.IsValid)
                {
                    return BadRequest(response.ValidateDTO);
                }
                else
                {
                    return Ok(response);
                }


            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "INSERT OPERATION : {createDTO}", createModel);

                return StatusCode(500, _validateDTO);
            }

        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] AccountSignInDTO userSignInDTO)
        {

            try
            {

                var response = await _accountBLL.SignInAsync(userSignInDTO);
                if (response.IsValid)
                {

                    var userResponse = await _accountBLL.GetByUserNameOrEmail(userSignInDTO.Email);

                    TokenResponse tokenResponse = GenerateToken(userResponse);


                    return Ok(tokenResponse);
                }
                else
                {
                    return BadRequest(response);
                }

            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "Login OPERATION : {userSignInDTO}", userSignInDTO);

                return StatusCode(500, _validateDTO);
            }
        }


        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<ActionResult> RefreshToken([FromBody] string expiredToken)
        {
            try
            {
                (bool IsValid, string UserEmail) = ValidateExpiredToken(expiredToken);

                if (IsValid)
                {
                    var userResponse = await _accountBLL.GetByUserNameOrEmail(UserEmail);

                    TokenResponse tokenResponse = GenerateToken(userResponse);

                    return Ok(tokenResponse);
                }
                else
                {
                    return BadRequest(new ValidatorResponse() { IsValid = false, MessageList = new List<string> { "Invalid Token" } });
                }
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "Login OPERATION : {expiredToken}", expiredToken);

                return StatusCode(500, _validateDTO);
            }
        }

        [HttpPut("ChangePassword")]
        public async Task<ActionResult> ChangePassword([FromBody] AccountChangePasswordDTO updateModel)
        {
            try
            {
                var validate = await _accountBLL.ValidateUpdatePasswordAsync(updateModel);
                if (!validate.IsValid)
                {
                    return BadRequest(validate);
                }

                await _accountBLL.UpdatePasswordAsync(updateModel);

                return Ok();
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "ChangePassword OPERATION : {updateDTO}", updateModel);

                return StatusCode(500, _validateDTO);
            }
        }

        [HttpPost("GetUser")]
        public async Task<ActionResult> GetUser([FromBody] string userNameOrEmail)
        {

            try
            {

                var response = await _accountBLL.GetByUserNameOrEmail(userNameOrEmail);
                if (response is AccountReadDTO)
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
                _logger.LogError(ex, "GET USER BY EMAIL OPERATION : {userNameOrEmail}", userNameOrEmail);

                return StatusCode(500, _validateDTO);
            }
        }

        private TokenResponse GenerateToken(AccountReadDTO userReadDTO)
        {

            IEnumerable<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, userReadDTO.Email),
                new Claim("ApplicationUserInfoId", userReadDTO.ApplicationUserInfoId.ToString()),
                new Claim("UserID", userReadDTO.UserID),
                new Claim("FullName", userReadDTO.FullName),
            };

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtOptions.Key)
                );

            SigningCredentials signingCredentials =
                new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256
            );

            DateTime tokenExpire = DateTime.Now.AddDays(Convert.ToInt32(_jwtOptions.ExpireDays));

            var securityToken = new JwtSecurityToken(
            claims: claims,
                expires: tokenExpire,
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                signingCredentials: signingCredentials
                );


            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return new TokenResponse() { Token = token, Expires = tokenExpire };
        }
        private (bool IsValid, string UserEmail) ValidateExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtOptions.Key);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = _jwtOptions.ValidateIssuer,
                ValidateAudience = _jwtOptions.ValidateAudience,
                ValidateLifetime = _jwtOptions.ValidateLifetime,
                ValidateIssuerSigningKey = _jwtOptions.ValidateIssuerSigningKey,
                ValidIssuer = _jwtOptions.Issuer,
                ValidAudience = _jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                var emailClaim = principal.FindFirst(ClaimTypes.Email)?.Value;

                return (true, emailClaim);
            }
            catch (SecurityTokenExpiredException)
            {
                // Token is expired but was valid
                var principal = tokenHandler.ReadToken(token) as JwtSecurityToken;
                var emailClaim = principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                return (true, emailClaim);
            }
            catch (Exception)
            {
                // Token is invalid
                return (false, null);
            }
        }

    }
}

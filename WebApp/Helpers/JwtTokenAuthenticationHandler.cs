using System.Runtime.CompilerServices;
using WebAPI.Client.Repository;
using WebAPI.Client.Repository.Account;
using WebAPI.Client.ViewModels;

namespace WebApp.Helpers
{
    public class JwtTokenAuthenticationHandler : IJwtTokenAuthenticationHandler
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<JwtTokenAuthenticationHandler> _logger;


        public JwtTokenAuthenticationHandler(
            IAccountRepository accountRepository,
            IHttpContextAccessor httpContextAccessor,
            ILogger<JwtTokenAuthenticationHandler> logger)
        {
            _accountRepository = accountRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public string GetToken()
        {

            string token = Convert.ToString(_httpContextAccessor?.HttpContext?.Request?.Cookies["AuthToken"]);

            return token;
        }

        public async Task<string> GetTokenRefreshAsync()
        {
            try
            {
                string currentToken = GetToken();
                var response = await _accountRepository.RefreshToken(currentToken);
                if (response.Status == ResponseStatus.Success)
                {
                    SaveToken(response.Content.Token);
                    return response.Content.Token;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Refesh Token failed");
                return string.Empty;
            }


        }

        public void SaveToken(string token)
        {
            _httpContextAccessor.HttpContext
                .Response
                .Cookies
                .Append("AuthToken", token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true
                }
                );

        }
    }
}

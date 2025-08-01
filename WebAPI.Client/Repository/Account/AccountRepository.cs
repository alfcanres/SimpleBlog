using DataTransferObjects;
using DataTransferObjects.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using WebAPI.Client.Helpers;
using WebAPI.Client.Options;
using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Repository.Account
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string _baseEndpoint = "api/account";
        private readonly string _clientName;
        private readonly HttpClient _client;
        private const string _loginEndpoint = "/login";
        private const string _registerEndpoint = "/register";
        private const string _refreshTokenEndpoint = "/refreshtoken";
        private const string _changePasswordEndPoint = "/changepassword";
        private const string _getUserEndPoint = "/getuser";
        private readonly ILogger<AccountRepository> _logger;


        public AccountRepository(
            IHttpClientFactory httpClientFactory,
            IOptions<HttpClientOptions> httpClientOptions,
            ILogger<AccountRepository> logger)
        {
            _httpClientFactory = httpClientFactory;
            _clientName = httpClientOptions.Value.ClientName;
            _logger = logger;
            _client = _httpClientFactory.CreateClient(_clientName);

        }

        public async Task<ResponseViewModel<TokenResponse>> LoginAsync(AccountSignInDTO loginViewModel)
        {
            var content = new StringContent(JsonSerializer.Serialize(loginViewModel), Encoding.UTF8, "application/json");
            ResponseViewModel<TokenResponse> responseView = new ResponseViewModel<TokenResponse>();

            try
            {
                var response = await _client.PostAsync($"{_baseEndpoint}{_loginEndpoint}", content);


                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = await JsonSerializer.DeserializeAsync<TokenResponse>(
                        await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );
                    responseView.Content = tokenResponse;
                    responseView.Status = ResponseStatus.Success;
                }
                else
                {
                    var validateResponse = await JsonSerializer.DeserializeAsync<ValidatorResponse>(
                        await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );

                    responseView.MessageList = validateResponse.MessageList;
                    responseView.Status = ResponseStatus.Unauthorized;
                }
            }
            catch (Exception ex)
            {

                responseView.MessageList = new List<string>() { ex.Message };
                responseView.Status = ResponseStatus.Error;
                _logger.LogError(ex, "An error occurred while processing the request.");
            }

            return responseView;

        }
        public async Task<ResponseViewModel<bool>> RegisterAsync(AccountCreateDTO createModel)
        {
            var content = new StringContent(JsonSerializer.Serialize(createModel), Encoding.UTF8, "application/json");
            var responseViewModel = new ResponseViewModel<bool>();

            try
            {
                var response = await _client.PostAsync($"{_baseEndpoint}{_registerEndpoint}", content);

                if (response.IsSuccessStatusCode)
                {
                    var userRegistered = await JsonSerializer.DeserializeAsync<AccountRegisteredDTO>(
                        await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );

                    responseViewModel.Content = true;
                    responseViewModel.Status = ResponseStatus.Success;

                }
                else
                {

                    var validateDTO = await JsonSerializer.DeserializeAsync<ValidatorResponse>(
                        await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );
                    responseViewModel.Content = false;
                    responseViewModel.MessageList = validateDTO.MessageList;
                    responseViewModel.Status = ResponseStatus.Error;
                }

            }
            catch (Exception ex)
            {
                responseViewModel.Content = false;
                responseViewModel.MessageList = new List<string>() { ex.Message };
                responseViewModel.Status = ResponseStatus.Error;
                _logger.LogError(ex, "An error occurred while processing the request.");
            }

            return responseViewModel;
        }
        public async Task<ResponseViewModel<TokenResponse>> RefreshToken(string expiredToken)
        {
            var content = new StringContent(JsonSerializer.Serialize(expiredToken), Encoding.UTF8, "application/json");
            ResponseViewModel<TokenResponse> responseView = new ResponseViewModel<TokenResponse>();

            try
            {
                var response = await _client.PostAsync($"{_baseEndpoint}{_refreshTokenEndpoint}", content);


                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = await JsonSerializer.DeserializeAsync<TokenResponse>(
                        await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );
                    responseView.Content = tokenResponse;
                    responseView.Status = ResponseStatus.Success;
                }
                else
                {
                    var validateResponse = await JsonSerializer.DeserializeAsync<ValidatorResponse>(
                        await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );

                    responseView.MessageList = validateResponse.MessageList;
                    responseView.Status = ResponseStatus.Unauthorized;
                }
            }
            catch (Exception ex)
            {
                responseView.MessageList = new List<string>() { ex.Message };
                responseView.Status = ResponseStatus.Error;
                _logger.LogError(ex, "An error occurred while processing the request.");
            }

            return responseView;
        }

        public Task<ResponseViewModel<bool>> UpdatePasswordAsync(AccountChangePasswordDTO model)
        {
            throw new NotImplementedException("UpdatePasswordAsync method is not implemented yet.");
        }
    }
}



namespace WebAPI.Client.Repository
{
    public interface IJwtTokenAuthenticationHandler
    {
        /// <summary>
        /// Will return the token from storage and refresh it if it is expired
        /// </summary>
        /// <returns></returns>
        Task<string> GetTokenRefreshAsync();

        /// <summary>
        /// Get the token from the storage (cookie, local storage, etc) 
        /// </summary>
        /// <returns></returns>
        string GetToken();

        /// <summary>
        /// Save the token to the storage (cookie, local storage, etc)
        /// </summary>
        /// <param name="token"></param>
        void SaveToken(string token);
    }
}

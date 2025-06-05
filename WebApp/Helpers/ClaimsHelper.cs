using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApp.Helpers
{
    public static class ClaimsHelper
    {
        public static int GetApplicationUserInfoId(string _authToken)
        {

            var claimValue = GetClaimValue("ApplicationUserInfoId", _authToken);

            return Convert.ToInt32(claimValue);
        }

        public static string GetUserID(string _authToken)
        {
            return GetClaimValue("UserID", _authToken);
        }


        public static string GetUserFullName(string _authToken)
        {
            return GetClaimValue("FullName", _authToken);
        }

        public static string GetUserEmail(string _authToken)
        {
            return GetClaimValue(ClaimTypes.Email, _authToken);
        }

        public static string GetClaimValue(string key, string _authToken)
        {
            if(string.IsNullOrEmpty(key) || string.IsNullOrEmpty(_authToken))
            {
                return string.Empty;
            }

            var jwtToken = new JwtSecurityToken(_authToken);
            var claim = jwtToken.Claims.Where(t => t.Type == key).FirstOrDefault();

            return claim.Value;
        }


    }
}

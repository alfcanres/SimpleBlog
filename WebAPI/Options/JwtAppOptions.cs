namespace WebAPI.Options
{
    public class JwtAppOptions
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpireDays { get; set; } = 1;
        public bool ValidateIssuer { get; set; } = true;
        public bool ValidateAudience { get; set; } = true;
        public bool ValidateLifetime { get; set; } = false;
        public bool ValidateIssuerSigningKey { get; set; } = true;
    }
}

namespace DataTransferObjects
{
    public record TokenResponse
    {
        public string Token { init; get; }
        public DateTime Expires { init; get; }

    }
}

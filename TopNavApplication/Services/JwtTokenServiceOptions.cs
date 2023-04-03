namespace TopNavApplication.Services
{
    public class JwtTokenServiceOptions
    {
        public string MagicKey { get; set; } = string.Empty;

        public string Id { get; set; } = string.Empty;

        public string Issuer { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;

        public int DurationInMinutes { get; set; }
    }
}

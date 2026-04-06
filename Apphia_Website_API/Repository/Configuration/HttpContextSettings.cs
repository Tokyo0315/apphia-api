namespace Apphia_Website_API.Repository.Configuration {
    public class HttpContextSettings {
        public required bool DevelopmentMode { get; set; }
        public required bool HttpOnly { get; set; }
        public required bool Secure { get; set; }
        public required string SameSite { get; set; }
        public required int ExpiresInMinutes { get; set; }
    }

    public class JwtSettings {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpirationMinutes { get; set; } = 720;
    }

    public class DestinationPath {
        public string? SuccessPath { get; set; }
        public string? ErrorPath { get; set; }
    }
}

namespace Apphia_Website_API.Repository.Configuration {
    public class HttpContextSettings {
        public required bool DevelopmentMode { get; set; }
        public required bool HttpOnly { get; set; }
        public required bool Secure { get; set; }
        public required string SameSite { get; set; }
        public required int ExpiresInMinutes { get; set; }
    }
    // JwtSettings → Repository/Configuration/JwtSettings.cs
    // DestinationPath → Repository/Configuration/Helper/DestinationPath.cs
}

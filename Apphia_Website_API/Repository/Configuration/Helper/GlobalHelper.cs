namespace Apphia_Website_API.Repository.Configuration.Helper
{
    public class GlobalHelper
    {
        public static string? ConvertThumbnailToUrl(byte[]? thumbnail)
        {
            if (thumbnail == null || thumbnail.Length == 0) return null;
            var base64 = Convert.ToBase64String(thumbnail);
            return $"data:image/png;base64,{base64}";
        }
    }
}

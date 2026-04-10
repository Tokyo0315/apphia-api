using System.Text;

namespace Apphia_Website_API
{
    public class LogHelper
    {
        public static void WriteLogToFile(string message, string destination, string endpoint, string status)
        {
            var dateStr = DateTime.Now.ToString("yyyyMMdd");
            var fileName = $"{dateStr}_ActionLogs.txt";
            var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{status}] [{endpoint}] {message}";

            if (!Directory.Exists(destination)) Directory.CreateDirectory(destination);
            var filePath = Path.Combine(destination, fileName);
            using var stream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            using var writer = new StreamWriter(stream);
            writer.WriteLine(logEntry);
        }
    }
}

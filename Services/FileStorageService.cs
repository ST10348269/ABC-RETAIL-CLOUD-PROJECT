using Azure.Storage.Files.Shares;
using System.Text;

namespace ABCRetail.Services
{
    public class FileStorageService
    {
        private readonly ShareClient _shareClient;
        private readonly string _directoryName = "app-logs";

        public FileStorageService(IConfiguration configuration)
        {
            string connectionString = configuration["AzureStorage:ConnectionString"];
            _shareClient = new ShareClient(connectionString, "logs");
            _shareClient.CreateIfNotExists();
        }

        public async Task WriteLogAsync(string message)
        {
            var directoryClient = _shareClient.GetDirectoryClient(_directoryName);
            await directoryClient.CreateIfNotExistsAsync();

            string fileName = $"log-{DateTime.Now:yyyyMMdd-HHmmss}.txt";
            var fileClient = directoryClient.GetFileClient(fileName);

            string fullMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}\n";
            byte[] bytes = Encoding.UTF8.GetBytes(fullMessage);

            using (var stream = new MemoryStream(bytes))
            {
                await fileClient.CreateAsync(stream.Length);
                await fileClient.UploadRangeAsync(new Azure.HttpRange(0, stream.Length), stream);
            }
        }

        public async Task<List<string>> GetLogFilesAsync()
        {
            var directoryClient = _shareClient.GetDirectoryClient(_directoryName);
            await directoryClient.CreateIfNotExistsAsync();

            var files = new List<string>();

            await foreach (var item in directoryClient.GetFilesAndDirectoriesAsync())
            {
                if (!item.IsDirectory)
                {
                    files.Add(item.Name);
                }
            }

            return files;
        }
    }
}
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO.Compression;

namespace FlowBill.Services
{
    public interface IBackupService
    {
        Task<string> CreateBackup();
        Task<bool> RestoreBackup(string backupPath);
        Task<List<BackupInfo>> GetBackupList();
        Task<bool> DeleteOldBackups(int daysToKeep = 30);
    }

    public class BackupService : IBackupService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<BackupService> _logger;
        private readonly string _backupPath;

        public BackupService(IConfiguration configuration, IWebHostEnvironment environment, ILogger<BackupService> logger)
        {
            _configuration = configuration;
            _environment = environment;
            _logger = logger;
            _backupPath = Path.Combine(_environment.ContentRootPath, "Backups");

            if (!Directory.Exists(_backupPath))
            {
                Directory.CreateDirectory(_backupPath);
            }
        }

        public async Task<string> CreateBackup()
        {
            try
            {
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var fileName = $"FlowBill_Backup_{timestamp}.bak";
                var fullPath = Path.Combine(_backupPath, fileName);

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    var commandText = $@"
                        BACKUP DATABASE [FlowBillDB] 
                        TO DISK = '{fullPath}'
                        WITH FORMAT, INIT, 
                        NAME = 'FlowBill Full Backup',
                        SKIP, NOREWIND, NOUNLOAD, STATS = 10";

                    using (var command = new SqlCommand(commandText, connection))
                    {
                        command.CommandTimeout = 600; // 10 minuten timeout
                        await command.ExecuteNonQueryAsync();
                    }
                }

                // Comprimeer de backup
                var zipFileName = $"{fileName}.zip";
                var zipPath = Path.Combine(_backupPath, zipFileName);

                using (var zip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
                {
                    zip.CreateEntryFromFile(fullPath, fileName);
                }

                // Verwijder het ongecomprimeerde bestand
                File.Delete(fullPath);

                _logger.LogInformation($"Backup succesvol aangemaakt: {zipFileName}");
                return zipFileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het maken van backup");
                throw;
            }
        }

        public async Task<bool> RestoreBackup(string backupPath)
        {
            try
            {
                // Extract zip if needed
                if (Path.GetExtension(backupPath).ToLower() == ".zip")
                {
                    var extractPath = Path.Combine(_backupPath, "temp");
                    if (!Directory.Exists(extractPath))
                    {
                        Directory.CreateDirectory(extractPath);
                    }

                    ZipFile.ExtractToDirectory(backupPath, extractPath);
                    backupPath = Directory.GetFiles(extractPath, "*.bak").FirstOrDefault();

                    if (string.IsNullOrEmpty(backupPath))
                    {
                        throw new FileNotFoundException("Geen backup bestand gevonden in zip");
                    }
                }

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    // Zet database in single user mode
                    var setSingleUser = "ALTER DATABASE [FlowBillDB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                    using (var command = new SqlCommand(setSingleUser, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }

                    // Restore database
                    var restoreCommand = $@"
                        RESTORE DATABASE [FlowBillDB] 
                        FROM DISK = '{backupPath}'
                        WITH REPLACE, NOUNLOAD, STATS = 10";

                    using (var command = new SqlCommand(restoreCommand, connection))
                    {
                        command.CommandTimeout = 600;
                        await command.ExecuteNonQueryAsync();
                    }

                    // Zet database terug in multi user mode
                    var setMultiUser = "ALTER DATABASE [FlowBillDB] SET MULTI_USER";
                    using (var command = new SqlCommand(setMultiUser, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }

                _logger.LogInformation($"Backup succesvol hersteld: {backupPath}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het herstellen van backup");
                return false;
            }
        }

        public async Task<List<BackupInfo>> GetBackupList()
        {
            return await Task.Run(() =>
            {
                var backups = new List<BackupInfo>();

                if (Directory.Exists(_backupPath))
                {
                    var files = Directory.GetFiles(_backupPath, "*.zip");

                    foreach (var file in files)
                    {
                        var fileInfo = new FileInfo(file);
                        backups.Add(new BackupInfo
                        {
                            FileName = fileInfo.Name,
                            FullPath = fileInfo.FullName,
                            CreatedDate = fileInfo.CreationTime,
                            Size = fileInfo.Length
                        });
                    }
                }

                return backups.OrderByDescending(b => b.CreatedDate).ToList();
            });
        }

        public async Task<bool> DeleteOldBackups(int daysToKeep = 30)
        {
            try
            {
                var cutoffDate = DateTime.Now.AddDays(-daysToKeep);
                var backups = await GetBackupList();

                foreach (var backup in backups.Where(b => b.CreatedDate < cutoffDate))
                {
                    File.Delete(backup.FullPath);
                    _logger.LogInformation($"Oude backup verwijderd: {backup.FileName}");
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het verwijderen van oude backups");
                return false;
            }
        }
    }

    public class BackupInfo
    {
        public string FileName { get; set; }
        public string FullPath { get; set; }
        public DateTime CreatedDate { get; set; }
        public long Size { get; set; }

        public string FormattedSize
        {
            get
            {
                string[] sizes = { "B", "KB", "MB", "GB" };
                int order = 0;
                double size = Size;

                while (size >= 1024 && order < sizes.Length - 1)
                {
                    order++;
                    size /= 1024;
                }

                return $"{size:0.##} {sizes[order]}";
            }
        }
    }
}
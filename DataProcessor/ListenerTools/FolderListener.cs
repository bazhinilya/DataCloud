using DbBinder.Conext;
using DbBinder.Models;
using DataProcessor.AppSettings;
using Microsoft.EntityFrameworkCore;

namespace DataProcessor.FolderListener
{
    internal class FolderListener
    {
        private static readonly string _listeningPath = new AppConfiguration().ListeningPath;
        private static readonly DbContextOptions<DbBinderContext> _connectionOptions = new AppConfiguration().ConnectionOptions;
        private static readonly FileSystemWatcher _fileSystemWatcher = new(_listeningPath)
        {
            NotifyFilter = NotifyFilters.Attributes
                           | NotifyFilters.CreationTime
                           | NotifyFilters.DirectoryName
                           | NotifyFilters.FileName
                           | NotifyFilters.LastAccess
                           | NotifyFilters.LastWrite
                           | NotifyFilters.Security
                           | NotifyFilters.Size,
            Filter = "",
            EnableRaisingEvents = true
        };

        public FolderListener() => _fileSystemWatcher.Changed += OnChangedDirectory;

        private static void OnChangedDirectory(object sender, FileSystemEventArgs eventArgs)
        {
            if (eventArgs.ChangeType != WatcherChangeTypes.Changed) return;
            DirectoryInfo directoryInfo = new(_listeningPath);
            foreach (var file in directoryInfo.GetFiles("*", SearchOption.AllDirectories))
            {
                Console.WriteLine(DateTime.UtcNow + $": New file found: {file.Name}, {file.Length / 1000} mb\n\tI'm working . . .");
                var byteArray = DataConverter.ConvertData.ConvertFile(file.FullName);
                AddArrayByteToDb(file, byteArray);
                File.Delete(file.FullName);
                Console.WriteLine("\tHas been successfully converted!");
            }
        }

        private static void AddArrayByteToDb(FileInfo file, byte[] arrayByte)
        {
            using var db = new DbBinderContext(_connectionOptions);
            var fileExtension = file.Extension[1..].ToLower();
            var dbExtensions = db.Extensions.FirstOrDefault(item => item.ExtensionValue == fileExtension);
            var extensionId = dbExtensions != null ? dbExtensions.Id : Guid.NewGuid();
            if (dbExtensions == null)
            {
                Console.WriteLine(DateTime.UtcNow + ": " + "Adding a new extension, please wait . . .");
                db.Extensions.Add(new Extension { Id = extensionId, ExtensionValue = fileExtension });
                db.SaveChanges();
                Console.WriteLine(DateTime.UtcNow + ": " + "New extension added, continue!");
            }
            db.Add(new FileData { Id = Guid.NewGuid(), Name = Path.GetFileNameWithoutExtension(file.Name), FileDataArray = arrayByte, ExtensionId = extensionId, });
            db.SaveChanges();
        }
    }
}
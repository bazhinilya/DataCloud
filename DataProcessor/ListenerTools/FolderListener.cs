using DataProcessor.AppSettings;
using DbLayer.Conext;
using DbLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataProcessor.FolderListener
{
    internal class FolderListener
    {
        private static readonly string _listeningPath = new AppConfiguration().ListeningPath;
        private static readonly DbContextOptions<DbLayerContext> _connectionOptions = new AppConfiguration().ConnectionOptions;
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

        private void OnChangedDirectory(object sender, FileSystemEventArgs eventArgs)
        {
            Console.WriteLine($"{DateTime.UtcNow}: New file found: {eventArgs.Name}\n\tI'm working . . .");
            new Thread(() => { AddArrayByteToDb(eventArgs); }).Start();
        }
        private void AddArrayByteToDb(FileSystemEventArgs processedFile)
        {
            using var db = new DbLayerContext(_connectionOptions);
            var byteArray = DataConverter.ConvertData.GetConvertFile(processedFile.FullPath);
            var fileExtension = Path.GetExtension(processedFile.Name)![1..].ToLower();
            var dbExtensions = db.Extensions.FirstOrDefault(item => item.ExtensionValue == fileExtension);
            var extensionId = dbExtensions != null ? dbExtensions.Id : Guid.NewGuid();
            if (dbExtensions == null)
            {
                db.Extensions.Add(new Extension { Id = extensionId, ExtensionValue = fileExtension });
                db.SaveChanges();
            }
            db.Add(new FileData { Id = Guid.NewGuid(), Name = Path.GetFileNameWithoutExtension(processedFile.Name)!, FileDataArray = byteArray, ExtensionId = extensionId, });
            db.SaveChanges();
            File.Delete(processedFile.FullPath);
            Console.WriteLine($"\t{processedFile.Name}: Has been successfully converted!");
        }
    }
}
using Microsoft.EntityFrameworkCore;

namespace MiddleWareLogic
{
    public static class DataConvertation
    {
        public static (string name, string extension, byte[] data) GetDataFileFromDb(DbLayer.Conext.DbLayerContext context, string fileName)
        {
            var dataFileFromDb = context.FileDatas
                .Join(context.Extensions,
                      fd => fd.ExtensionId,
                      ex => ex.Id,
                      (fd, ex) => new { fd.Name, ex.ExtensionValue, fd.FileDataArray })
                .FirstOrDefault(file => file.Name == Path.GetFileNameWithoutExtension(fileName))
                    ?? throw new Exception($"File with this fileName = {fileName} not exists");
            return (dataFileFromDb.Name, dataFileFromDb.ExtensionValue, dataFileFromDb.FileDataArray);
        }

        public static async Task<string> SetDataFileFromDb(DbLayer.Conext.DbLayerContext context, object uploadedFile)
        {
            uploadedFile.
            var extensionsFromDb = await context.Extensions.ToListAsync();
            var setedFiles = new List<DbLayer.Models.FileData>();
            var setedExtension = new List<DbLayer.Models.Extension>();
            foreach (var upladedFile in upladedFiles) 
            {
                if (!await context.FileDatas.AnyAsync(file => file.Name == upladedFile.Key.Name)) 
                {
                    var extension = extensionsFromDb.FirstOrDefault(extension => extension.ExtensionValue == upladedFile.Value.ExtensionValue);
                    if (extension == null)
                    {
                        setedExtension.Add(new DbLayer.Models.Extension { Id = Guid.NewGuid(), ExtensionValue})
                    } 
                    //var ext = extension == null? null : extension.ExtensionValue;
                    setedFiles.Add(new DbLayer.Models.FileData
                    {
                        Id = Guid.NewGuid(),
                        Name = upladedFile.Key.Name,
                        FileDataArray = upladedFile.Key.FileDataArray,
                        ExtensionId = 
                    });
                    //var createdFile = new DbLayer.Models.FileData 
                    //{
                    //    Id = Guid.NewGuid(),
                    //    Name = upladedFile.Name,
                    //    FileDataArray = upladedFile.FileDataArray
                    //}
                }
                
            }
            await context.FileDatas.AddRangeAsync();
            await context.SaveChangesAsync();

            return new object();
        }
    }
}
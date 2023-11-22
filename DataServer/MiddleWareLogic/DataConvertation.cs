using Microsoft.EntityFrameworkCore;

namespace MiddleWareLogic
{
    public static class DataConvertation
    {
        public static (string name, string extension, byte[] data) GetDataFileFromDb(DbLayer.Conext.DbLayerContext context, string fileName)
        {
            //TODO: проблема с поиском файлов, проверить
            try
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
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public static async Task<string> SetDataFileFromDb(DbLayer.Conext.DbLayerContext context, IFormFileCollection uploadedFiles)
        {
            foreach (var uploadedFile in uploadedFiles) 
                await Task.Run(async () => await AddUploadedFile(context, uploadedFile));
            return System.Net.HttpStatusCode.OK.ToString();
        }

        private static async Task AddUploadedFile(DbLayer.Conext.DbLayerContext context, IFormFile uploadedFile)
        {
            try
            {
                (var fileNameWithoutExtension, var fileExtension) = (Path.GetFileNameWithoutExtension(uploadedFile.FileName), Path.GetExtension(uploadedFile.FileName).ToLower());
                using var dataStream = new MemoryStream();
                await uploadedFile.CopyToAsync(dataStream);
                var extensionFromDb = await context.Extensions.FirstOrDefaultAsync(ex => ex.ExtensionValue == fileExtension);
                var extensionId = extensionFromDb == null ? Guid.NewGuid() : extensionFromDb.Id;
                if (extensionFromDb == null) await context.AddAsync(new DbLayer.Models.Extension { Id = extensionId, ExtensionValue = fileExtension });
                await context.FileDatas.AddAsync(new DbLayer.Models.FileData { Id = Guid.NewGuid(), ExtensionId = extensionId, Name = uploadedFile.FileName, FileDataArray = dataStream.ToArray() });
                await context.SaveChangesAsync();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
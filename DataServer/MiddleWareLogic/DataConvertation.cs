using DbLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MiddleWareLogic
{
    public static class DataConvertation
    {
        public static (string name, string extension, byte[] data) GetDataFileFromDb(DbLayer.Conext.DbLayerContext context, string fileName)
        {
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

        public static async Task<string> SetDataFileFromDb(DbLayer.Conext.DbLayerContext context, IFormFile uploadedFile)
        {
            try
            {
                (var fileNameWithoutExtension, var fileExtension) = (Path.GetFileNameWithoutExtension(uploadedFile.FileName), Path.GetExtension(uploadedFile.FileName).ToLower());
                using var dataStream = new MemoryStream();
                await uploadedFile.CopyToAsync(dataStream);
                var extensionFromDb = await context.Extensions.FirstOrDefaultAsync(ex => ex.ExtensionValue == fileExtension);
                var extensionId = extensionFromDb == null ? Guid.NewGuid() : extensionFromDb.Id;
                if (extensionFromDb == null)
                {
                    await context.AddAsync(new Extension { Id = extensionId, ExtensionValue = fileExtension });
                }
                await context.FileDatas.AddAsync(new FileData { Id = Guid.NewGuid(), ExtensionId = extensionId, Name = uploadedFile.Name, FileDataArray = dataStream.ToArray() });
                await context.SaveChangesAsync();
                return HttpStatusCode.OK.ToString();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
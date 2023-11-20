namespace DataConverter
{
    public static class DeconvertData
    {
        //TODO: проверка если расширение некорректно
        public static byte[] GetDeconvertedData(DbLayer.Conext.DbLayerContext context, string fileName)
        {
            var fileFromDb = context.FileDatas.Join(context.Extensions,
                fd => fd.ExtensionId,
                ex => ex.Id,
                (fd, ex) => new { fd.FileDataArray, fd.Name, ex.ExtensionValue }) 
                     ?? throw new Exception("");

            //DbLayer.Models.FileData? fileFromDb = context.FileDatas.FirstOrDefault(file => file.Name == fileNameWithoutExtension)
            //        ?? throw new Exception();
            //DbLayer.Models.Extension? fileExtensionFromDb = context.Extensions.FirstOrDefault(extension => extension.ExtensionValue == fileExtension)
            //        ?? throw new Exception();

            //Task.Run(async () =>
            //{
            //    fileFromDb = await context.FileDatas.FirstOrDefaultAsync(file => file.Name.Contains(fileNameWithoutExtension, StringComparison.OrdinalIgnoreCase))
            //        ?? throw new Exception();
            //    fileExtensionFromDb = await context.Extensions.FirstOrDefaultAsync(extension => extension.ExtensionValue == fileExtension)
            //        ?? throw new Exception();
            //}).Start();

            return fileFromDb.First().FileDataArray;
            //var contentType = $"{fileNameWithoutExtension}/{fileExtension}";
            //return File()
            //new FileStreamResult(fileDataArrayStream, "file/png");
        }
    }
}
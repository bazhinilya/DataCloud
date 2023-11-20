namespace MiddleWareLogic
{
    public static class DataConvertation
    {
        public static (string name, string extension, byte[] data) GetDataFileFromDb(DbLayer.Conext.DbLayerContext context, string fileName)
        {
            var somethingObject = context.FileDatas
                .Join(context.Extensions,
                      fd => fd.ExtensionId,
                      ex => ex.Id,
                      (fd, ex) => new { fd.Name, ex.ExtensionValue, fd.FileDataArray })
                .FirstOrDefault(file => file.Name == Path.GetFileNameWithoutExtension(fileName))
                    ?? throw new Exception($"File with this fileName = {fileName} not exists");
            return (somethingObject.Name, somethingObject.ExtensionValue, somethingObject.FileDataArray);
        }
    }
}

namespace DataConverter
{
    public static class ConvertData
    {
        public static byte[] GetConvertFile(string filePath)
        {
            using FileStream fileStream = new(filePath, FileMode.Open);
            var arrayByte = new byte[fileStream.Length];
            fileStream.Read(arrayByte, 0, arrayByte.Length);
            //fileStream.Close();
            return arrayByte;
        }
    }
}
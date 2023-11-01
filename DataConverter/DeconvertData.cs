namespace DataConverter
{
    public static class DeconvertData
    {
        public static void ConvertArrayByte(byte[] array, string filePath)
        {
            using FileStream fileStream = new(filePath, FileMode.OpenOrCreate);
            fileStream.Write(array, 0, array.Length);
        }
    }
}
namespace DbLayer.Models
{
    public class FileData
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required byte[] FileDataArray { get; set; }
        public required Guid ExtensionId { get; set; }
    }
}
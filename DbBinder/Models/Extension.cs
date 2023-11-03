namespace DbLayer.Models
{
    public class Extension
    {
        public required Guid Id { get; set; }
        public required string ExtensionValue { get; set; }
        public virtual ICollection<FileData> FileDatas { get; set; }
    }
}
namespace Application.System.Dtos
{
    public class FileStorageDto
    {
        public Guid FileStorageId { get; set; }
        public string? FileStorageType { get; set; }
        public string? Name { get; set; }
        public string? Desc { get; set; }
        public string? AzureStorageAccountConnectionString { get; set; }
        public string? AzureStorageAccountContainerName { get; set; }
        public string? FileSystemBasePath { get; set; }
        public UserBase? CreateByUser { get; set; }
        public DateTime? CreateTime { get; set; }
        public UserBase? UpdateByUser { get; set; }
        public DateTime? UpdateTime { get; set; }
    }

    public class FileStorageNameDto
    {
        public Guid FileStorageId { get; set; }
        public string? FileStorageType { get; set; }
        public string? Name { get; set; }
    }
}

using Domain.EntityFramework;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.System.Entities
{
    [Table("FileStorage")]
    [PrimaryKey(nameof(FileStorageId))]
    public class FileStorage : BaseEntityWithCreateUpdate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FileStorageId { get; private set; }
        public string FileStorageType { get; private set; }
        public string Name { get; private set; }
        public string? Desc { get; private set; }

        [NotMapped]
        public string? AzureStorageAccountConnectionString { get; private set; }

        [Column("AzureStorageAccountConnectionString")]
        public string? AzureStorageAccountConnectionStringEncrypted
        {
            get { return EncryptUtil.AESEncrypt(AzureStorageAccountConnectionString); }
            set { AzureStorageAccountConnectionString = EncryptUtil.AESDecrypt(value); }
        }
        public string? AzureStorageAccountContainerName { get; private set; }
        public string? FileSystemBasePath { get; private set; }

        private FileStorage() { }

        public FileStorage(string fileStorageType, string name, string? desc, string? azureStorageAccountConnectionString, string? azureStorageAccountContainerName, string? fileSystemBasePath)
        {
            FileStorageType = fileStorageType.Trim();
            Name = name.Trim();
            Desc = desc?.Trim();
            AzureStorageAccountConnectionString = azureStorageAccountConnectionString?.Trim();
            AzureStorageAccountContainerName = azureStorageAccountContainerName?.Trim();
            FileSystemBasePath = fileSystemBasePath?.Trim();
        }
    }
}

namespace Domain.ValueObjests
{
    public class FileStorageType
    {
        public const string FileSystem = "FileSystem";  //文件系统
        public const string AzureStorageAccountGen2 = "AzureStorageAccountGen2";   //Azure存储账户Gen1
        public const string AzureStorageAccountGen1 = "AzureStorageAccountGen1";   //Azure存储账户Gen2
    }

    public class SerilogStorageType
    {
        public const string Azure = "Azure";
        public const string File = "File";
    }
}

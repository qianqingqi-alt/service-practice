namespace Infrastructure.FileStorage
{
    public class PathItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public long Length { get; set; }
        public bool IsDirectory { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public PathItem(string Name, string Path, long Length, bool IsDirectory, DateTime CreateTime, DateTime UpdateTime)
        {
            this.Name = Name;
            this.Path = Path;
            this.Length = Length;
            this.IsDirectory = IsDirectory;
            this.CreateTime = CreateTime;
            this.UpdateTime = UpdateTime;
        }
    }
}

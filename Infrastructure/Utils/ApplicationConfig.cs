namespace Infrastructure.Utils
{
    public class Ports
    {
        public int Nginx { get; set; }
        public int WinPbi { get; set; }
        public int Main { get; set; }
        public int Scheduler { get; set; }
        public int WinModel { get; set; }
    }

    /// <summary>
    /// 应用程序配置初始化
    /// </summary>
    public class ApplicationConfig
    {
        public Ports Ports = new Ports();
        public string? PublishHost { get; set; }
        public string? EncryptionKey { get; set; }
    }
}

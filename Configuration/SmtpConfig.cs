namespace ASP_MVC.Configuration
{
    public class SmtpConfig
    {
#pragma warning disable CS8618 //отключаем null-ворнинги
        public string Host { get; set; }        
        public int Port { get; set; }
        public bool EnableLogging { get; set; }
        public string FromAddress { get; set; }
    }
}

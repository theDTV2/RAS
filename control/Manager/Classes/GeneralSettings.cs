namespace control.Manager.Classes
{
    public class GeneralSettings
    {
        public TimeSpan UserLoginTimeout { get; set; } = TimeSpan.FromMinutes(15);
        public string EMailSuffix { get; set; } = "";

        public string Hostname { get; set; } = "";
    }
}

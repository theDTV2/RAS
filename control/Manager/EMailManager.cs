namespace control.Manager
{
    public static class EMailManager
    {
        public static string MailSMTPAdress { get; } = string.Empty;

        public static int MailPort { get; } = 465;

        public static string MailUserName { get; } = string.Empty;

        public static string MailPassword { get; } = string.Empty;


        public static bool LoadMailSettingsFromConfig()
        {
            //TODO: LoadMailSettingsFromConfig
            return true; 
        }

        public static bool SetMailParameters(string newMailSMTPAdress, int newMailPort, string MailUserName, string MailPassword)
        {
            //TODO: SetMailParameters
            return true;
        }


    }
}

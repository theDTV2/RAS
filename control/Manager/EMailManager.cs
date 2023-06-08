using System.Collections.Concurrent;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using control.Manager.Classes;

namespace control.Manager
{
    public static class EMailManager
    {
        public static EMailSettings EMailSettings { get; private set; } = new EMailSettings();

        private static ConcurrentStack<Email> EmailQueueNormal { get; set; } = new ConcurrentStack<Email>();
        private static ConcurrentStack<Email> EmailQueueHighPriority { get; set; } = new ConcurrentStack<Email>();


        public static bool LoadMailSettingsFromConfig()
        {
            //TODO: Make Config location changable
            //TODO: Catch errors
            string read = System.IO.File.ReadAllText("MailSettings.cfg");

            if (read is null)
                return false;

            var _localEMailSettings = JsonSerializer.Deserialize<EMailSettings>(read);

            if (_localEMailSettings == null)
                return false;

            EMailSettings = _localEMailSettings;

            //TODO: LoadMailSettingsFromConfig
            return true;
        }

        public static bool SaveMailSettingsToConfig()
        {
            string _toWrite = JsonSerializer.Serialize<EMailSettings>(EMailSettings);

            //TODO: Make Config location changable
            //TODO: Catch errors
            System.IO.File.WriteAllText("MailSettings.cfg", _toWrite);
            //TODO: LoadMailSettingsFromConfig
            return true;
        }


        public static bool SetMailParameters(string newMailSMTPAdress, int newMailPort, string newMailUserName, string newMailPassword)
        {
            EMailSettings.MailSMTPAdress = newMailSMTPAdress;
            EMailSettings.MailPort = newMailPort;
            EMailSettings.MailUserName = newMailUserName;
            EMailSettings.MailPassword = newMailPassword;

            return SaveMailSettingsToConfig();
        }

        public static void AddLoginMailToQueue(HttpContext context, string adressToSendTo, int loginKey, string loginCode)
        {
            string message = @"Click the link or use the login code:
" + loginCode + @"
" + context.Request.Host + @"/Account/LoginCode/" + loginKey + @"
If you did not request this message, you can ignore this message";

            EmailQueueHighPriority.Push(new Email(adressToSendTo, message));
        }

    }


    public class EMailSettings
    {
        public string MailSMTPAdress { get; set; } = String.Empty;
        public int MailPort { get; set; } = 0;
        public string MailUserName { get; set; } = String.Empty;
        public string MailPassword { get; set; } = String.Empty;
    }


}

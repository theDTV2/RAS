using System.Collections.Concurrent;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using control.Manager.Classes;
using MailKit.Net.Smtp;
using MimeKit;
using static control.Manager.EMailManager;

namespace control.Manager
{
    public static class EMailManager
    {
        public static EMailSettings EMailSettingObj { get; private set; } = new EMailSettings();

        private static bool Tested { get; set; } = false;

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

            EMailSettingObj = _localEMailSettings;

            return true;
        }

        public static bool SaveMailSettingsToConfig()
        {
            string _toWrite = JsonSerializer.Serialize<EMailSettings>(EMailSettingObj);

            //TODO: Make Config location changable
            //TODO: Catch errors
            System.IO.File.WriteAllText("MailSettings.cfg", _toWrite);

            return true;
        }


        public static bool SetMailParameters(string newMailSMTPAdress, int newMailPort, string newMailUserName, string newMailPassword)
        {
            EMailSettingObj.MailSMTPAdress = newMailSMTPAdress;
            EMailSettingObj.MailPort = newMailPort;
            EMailSettingObj.MailUserName = newMailUserName;
            EMailSettingObj.MailPassword = newMailPassword;

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

        public static bool TestMailSettings()
        {
            if (EMailSettingObj.Tested)
                return true;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("RAS System", EMailSettingObj.MailUserName + "@htw-berlin.de"));
            message.To.Add(new MailboxAddress("RAS System", EMailSettingObj.MailUserName + "@htw-berlin.de"));
            message.Subject = "Hello World";

            message.Body = new TextPart("plain")
            {
                Text = @"Test Message, Hello."
            };

            using (var client = new SmtpClient())
            {
             
                try
                {
                    client.Connect(EMailSettingObj.MailSMTPAdress, EMailSettingObj.MailPort, true);
                    client.Authenticate(EMailSettingObj.MailUserName, EMailSettingObj.MailPassword);
                    client.Send(message);

                }
                catch (Exception)
                {
                    client.Disconnect(true);
                    return false;
                }

                client.Disconnect(true);
                EMailSettingObj.Tested = true;
                return true;

            }
        }


        public static async void SendMailsAsync()
        {
            //We wait, until an config file is written
            while (EMailSettingObj.IsAnyEmpty() || !EMailSettingObj.Tested)
            {
                Thread.Sleep(10000);
                LoadMailSettingsFromConfig();
            }

            while (true)
            {
                //Retreive Email from List




                if (EmailQueueHighPriority.IsEmpty && EmailQueueNormal.IsEmpty)
                    Thread.Sleep(3000);
            }


        }
    }


    public class EMailSettings
    {
        public string MailSMTPAdress { get; set; } = String.Empty;
        public int MailPort { get; set; } = 0;
        public string MailUserName { get; set; } = String.Empty;
        public string MailPassword { get; set; } = String.Empty;

        public bool Tested { get; set; } = false;

        public bool IsAnyEmpty()
        {
            if (MailSMTPAdress == string.Empty || MailPort == 0
                || MailUserName == string.Empty || MailPassword == string.Empty)
            {
                return true;
            }
            return false;
        }
    }
}




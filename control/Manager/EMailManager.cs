using System.Collections.Concurrent;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;
using control.Manager.Classes;
using MailKit.Net.Smtp;
using MimeKit;
using static System.Net.WebRequestMethods;
using static control.Manager.EMailManager;

namespace control.Manager
{
    public static class EMailManager
    {
        public static EMailSettings EMailSettingObj { get; private set; } = new EMailSettings();

        private static SmtpClient EmailClient { get; set; } = new();

        private static ConcurrentStack<Email> EmailQueueNormal { get; set; } = new ConcurrentStack<Email>();
        private static ConcurrentStack<Email> EmailQueueHighPriority { get; set; } = new ConcurrentStack<Email>();


        public static bool LoadMailSettingsFromConfig()
        {
            if (!System.IO.File.Exists("MailSettings.cfg"))
                return false;
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

            if (System.IO.File.Exists("MailSettings.cfg"))
                System.IO.File.Delete("MailSettings.cfg");

            System.IO.File.WriteAllText("MailSettings.cfg", _toWrite);

            return true;
        }


        public static bool SetMailParameters(string newMailSMTPAdress, int newMailPort, string newMailUserName, string newMailPassword)
        {
            EMailSettingObj.MailSMTPAdress = newMailSMTPAdress;
            EMailSettingObj.MailPort = newMailPort;
            EMailSettingObj.MailUserName = newMailUserName;
            EMailSettingObj.MailPassword = newMailPassword;

            SaveMailSettingsToConfig();

            return LoadMailSettingsFromConfig();
        }

        public static void AddLoginMailToQueue(HttpContext httpContext, string adressToSendTo, string loginCode, string loginKey)
        {
            string _message = "<p>Click the following link or use the login code to sign in.</p>"
+ "<a href = https://" + GeneralSettingsManager.GetHostName() + @"/Account/LoginCode/" + HttpUtility.UrlEncode(loginKey) + ">Click here</a> <br> "
+ loginCode + "<br> If you did not request this message, you can ignore it.";

            EmailQueueHighPriority.Push(new Email(adressToSendTo, "RAS Login", _message));
        }

        public static bool TestMailSettings()
        {
            if (EMailSettingObj.Tested)
                return true;
            if (EMailSettingObj.IsAnyEmpty())
                return false;

            //Due to this being a test function, we ignore the Async properties of the function
            if (ConnectToMailServer())
            {
                EMailSettingObj.Tested = true;
                return true;
            }
            return false;
        }


        public static async Task SendQueuedMailsAsync(CancellationToken stopToken)
        {
            //We wait, until an config file is written
            while (EMailSettingObj.IsAnyEmpty() || !EMailSettingObj.Tested)
            {
                await Task.Delay(10000, stopToken);
                LoadMailSettingsFromConfig();
                TestMailSettings();
            }

            while (!stopToken.IsCancellationRequested)
            {
                //Retreive Email from List

                if (!EmailClient.IsConnected)
                {
                    if (ConnectToMailServer())
                    {
                        //TODO: Log error
                        await Task.Delay(10000, stopToken);
                        continue;
                    }
                }

                Email? _nextMail = GetNextEmail();

                while (_nextMail is not null)
                {
                    MimeMessage _message = CreateMailObject(_nextMail);

                    await SendMailAsync(_message);
                    _nextMail = GetNextEmail();

                }
                if (EmailClient.IsConnected)
                    DisconnectFromMailServer();

            }


        }

        private static Email? GetNextEmail()
        {
            Email? _email;
            if (!EmailQueueHighPriority.TryPop(out _email))
                if (!EmailQueueNormal.TryPop(out _email))
                    return null;
            return _email;
        }

        private static bool ConnectToMailServer()
        {
            try
            {
                EmailClient.Connect(EMailSettingObj.MailSMTPAdress, EMailSettingObj.MailPort, true);
                EmailClient.Authenticate(EMailSettingObj.MailUserName, EMailSettingObj.MailPassword);
            }
            catch (Exception)
            {
                return false;
            }
            return true;

        }

        private static void DisconnectFromMailServer()
        {
            EmailClient.Disconnect(true);
            return;
        }

        private static MimeMessage CreateMailObject(Email email)
        {
            var name = email.DestinationAddress.Split("@")[0];

            MimeMessage _message = new MimeMessage();
            _message.From.Add(new MailboxAddress("RAS System", EMailSettingObj.MailUserName + "@htw-berlin.de"));
            _message.To.Add(new MailboxAddress(name, email.DestinationAddress));
            _message.Subject = email.Title;
            _message.Body = new TextPart("html")
            {
                Text = email.Text
            };
            return _message;
        }

        private static async Task<bool> SendMailAsync(MimeMessage emailObject)
        {
            try
            {
                await EmailClient.SendAsync(emailObject);
            }
            catch (Exception e)
            {
                return false;
            }

            return false;
        }
    }
}


   


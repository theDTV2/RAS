namespace control.Manager.Classes
{
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



using control.Manager;

namespace control.Helper
{
    public class EmailHelper
    {
        //This function adds an mail, that the email routine will send
        public static void AddAccessMailToBeSent(HttpContext context, string emailAdress,int loginKey, string loginCode)
        {
            EMailManager.AddLoginMailToQueue(context, emailAdress, loginKey, loginCode);

            Console.WriteLine(emailAdress + Environment.NewLine + loginCode + Environment.NewLine);
            Console.WriteLine(loginKey + Environment.NewLine);
        }

        //This function adds an reminder email to be send
        public static void AccessReminderEmailToBeSend(string emailAdress)
        {
            throw new NotImplementedException();
        }

        //This mail sends a specific mail
        //TODO

    }
}

namespace control.Helper
{
    public class EmailHelper
    {
       //TODO: Mail handling

        //This function adds an mail, that the email routine will send
        public static void AddAccessMailToBeSent(string accessCode, string emailAdress)
        {
            Console.WriteLine(emailAdress + Environment.NewLine + accessCode + Environment.NewLine);
        }

        //This function adds an reminder email to be send
        public static void AccessReminderEmailToBeSend(string emailAdress)
        {
            throw new NotImplementedException();
        }


        //This function perdiodically sends out any pending reminder emails (sometime in the night)
        public static void SendAllPendingReminderMails()
        {
            throw new NotImplementedException();
        }


        //This mail sends a specific mail
        //TODO

    }
}

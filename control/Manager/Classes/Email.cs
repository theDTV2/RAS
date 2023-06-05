namespace control.Manager.Classes
{
    public class Email
    {
        public string DestinationAdress { get; private set; }
        public string Text { get; private set; }
        public Email(string destination, string text)
        { 
            DestinationAdress = destination;
            Text = text;
        }

    }
}

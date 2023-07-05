namespace control.Manager.Classes
{
    public class Email
    {
        public string DestinationAddress { get; private set; }  

        public string Title { get; private set; }
        public string Text { get; private set; }
        public Email(string destination,string title, string text)
        { 
            DestinationAddress = destination;
            Title = title;
            Text = text;
        }

    }
}

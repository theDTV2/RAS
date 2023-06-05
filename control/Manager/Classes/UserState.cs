namespace control.Manager.Classes
{
    public class UserState
    {
        public string UserName { get; private set; }

        public DateTime LoginTime { get; private set; } = DateTime.Now;

        public DateTime LastAction { get; private set; } = DateTime.Now;

        public bool Valid { get; private set; } = true;

        public UserState(string username)
        {
            UserName = username;
        }

        public void SetInvalid()
        {
            Valid = false;
        }
    }
}

using control.Models;

namespace control.Helper
{
    public class LogHelper
    {
        public static void AddSuccessfulUserEntryToLog(control.Data.controlContext dataContext, Door door, User user)
        {
            Log _newEntry = new()
            {
                Door = door, 
                User = user,
                EntryTime = DateTime.Now,
                EntryResult = EntryResult.AccessAllowed};


           dataContext.Log.Add(_newEntry);
           dataContext.SaveChanges();
        }
    }
}

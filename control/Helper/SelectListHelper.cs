using control.Models;

namespace control.Helper
{
    public static class SelectListHelper
    {
        public static IList<Door> ConvertDoorIdStringsToReferences(control.Data.controlContext dataContext, string[] doors)
        {
            return dataContext.Door.Where(e => doors.Contains(e.Id)).ToList();
        }

        public static IList<User> ConvertUserIdStringsToReferences(control.Data.controlContext dataContext, string[] users)
        {
            return dataContext.User.Where(e => users.Contains(e.UserName)).ToList();
        }

    }
}

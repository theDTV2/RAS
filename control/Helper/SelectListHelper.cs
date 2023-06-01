using control.Models;

namespace control.Helper
{
    public static class SelectListHelper
    {
        public static IList<Door> ConvertDoorIdStringsToReferences(control.Data.controlContext dataContext, string[] doors)
        {
            return dataContext.Door.Where(e => doors.Contains(e.Id)).ToList();
        }

    }
}

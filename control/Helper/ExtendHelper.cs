using control.Models;

namespace control.Helper
{
    public static class ExtendHelper
    {
        public static bool LoggedInUserIsAllowedToExtend(control.Data.controlContext dataContext, HttpContext httpContext)
        {
            DateTime _now = DateTime.Now;
            DateTime _ssSemester = new DateTime(DateTime.Now.Year, 08, 31);
            DateTime _wsSemester = new DateTime(DateTime.Now.Year, 04, 30);
             
            if (_now < _ssSemester && (_ssSemester - _now) < TimeSpan.FromDays(31))
                return true;

            if (_now < _wsSemester && (_wsSemester - _now) < TimeSpan.FromDays(31))
                return true;

            return true;
        }

        private static DateTime GenerateNewExpiryDate()
        {
            //TODO: Calculate expiry date
           // if (DateTime.Now < )
           return DateTime.Now;
        }

    }
}

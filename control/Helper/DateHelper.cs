using control.Models;

namespace control.Helper
{
    public static class DateHelper
    {
        public static bool LoggedInUserIsAllowedToExtend(control.Data.controlContext dataContext, HttpContext httpContext)
        {
            DateTime _now = DateTime.Now;
            DateTime _ssSemester = new DateTime(DateTime.Now.Year, 08, 31);
            DateTime _wsSemester = new DateTime(DateTime.Now.Year, 03, 31);
             
            if (_now < _ssSemester && (_ssSemester - _now) < TimeSpan.FromDays(31))
                return true;

            if (_now < _wsSemester && (_wsSemester - _now) < TimeSpan.FromDays(31))
                return true;

            return false;
        }

        public static DateTime GenerateNextSemesterEnd()
        {
            DateTime _now = DateTime.Now;
            DateTime _ssSemester = new DateTime(DateTime.Now.Year, 09, 30);
            DateTime _wsSemester = new DateTime(DateTime.Now.Year, 04, 30);

            if (_now > new DateTime(DateTime.Now.Year, 1, 1) && _now < _wsSemester)
            {
                return new DateTime(DateTime.Now.Year, 09, 30);

            }
            if (_now > _wsSemester && _now < _ssSemester)
            {
                return new DateTime(DateTime.Now.Year+1, 03, 31);
            }

            return new DateTime(DateTime.Now.Year + 1, 09, 30);

        }

        public static DateTime GenerateCurrentSemesterEnd()
        {
            DateTime _now = DateTime.Now;
            DateTime _ssSemester = new DateTime(DateTime.Now.Year, 09, 30);
            DateTime _wsSemester = new DateTime(DateTime.Now.Year, 04, 30);

            if (_now > new DateTime(DateTime.Now.Year, 1, 1) && _now < _wsSemester)
            {
                return _wsSemester;

            }
            if (_now > _wsSemester && _now < _ssSemester)
            {
                return _ssSemester;
            }

            return _wsSemester.AddYears(1);

        }

    }
}

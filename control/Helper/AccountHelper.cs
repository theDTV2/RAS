using control.Models;

namespace control.Helper
{
    public static class AccountHelper
    {

        public static bool LoginUser(HttpContext httpContext, string username, EAccessLevel level)
        {
            SetLoggedInStatus(httpContext);
            SetUserName(httpContext, username);
            SetUserRole(httpContext, level);


            //TODO: Think, if anything needs to be caught here
            return true;
        }

        public static bool LogoutUser(HttpContext httpContext)
        {
            //TODO: Make this better

            httpContext.Session.Clear();

            //TODO: Think, if anything needs to be caught here
            return true;
            //return false;
        }

        private static void SetLoggedInStatus(HttpContext context, bool statusToSetTo = true)
        {
            SessionHelper.SaveStringInSession(context,"loggedInStatus", statusToSetTo.ToString());
        }

        public static bool SetUserName(HttpContext context, string userName)
        {
            SessionHelper.SaveStringInSession(context, "userName", userName);

            //TODO: What to do when a User Name is already set?
            return true;
        }

        private static bool SetUserRole(HttpContext context, EAccessLevel role)
        {
            SessionHelper.SaveIntInSession(context, "userRole", Convert.ToInt32(role));
            //TODO: What to do when a User Role is already set?
            return true;
        }

        public static bool GetLoggedIn(HttpContext context)
        {
            if (SessionHelper.GetStringFromSession(context, "loggedInStatus") == "")
                return false;
            return true;
        }
        public static string GetUserName(HttpContext context)
        {
            string userName = SessionHelper.GetStringFromSession(context, "userName");

            return userName;
        }

        public static EAccessLevel GetEAccessLevel(HttpContext context)
        {

            if (!GetLoggedIn(context))
                return EAccessLevel.kNone;

            int userRoleRaw = SessionHelper.GetIntFromSession(context, "userRole");

             if (Enum.IsDefined(typeof(EAccessLevel),userRoleRaw))
                return (EAccessLevel)userRoleRaw;

            return EAccessLevel.kNone;
        }

        public static User GetLoggedInUser(control.Data.controlContext dataContext, HttpContext context)
        {
            if (!GetLoggedIn(context))
                return new User();

            string _userName = GetUserName(context);

            return dataContext.User.Where(e => e.UserName == _userName).FirstOrDefault()!;
        }

    }
}

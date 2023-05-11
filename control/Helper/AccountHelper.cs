using control.Models;

namespace control.Helper
{
    public static class AccountHelper
    {
         
        public static void SetLoggedInStatus(HttpContext context, bool statusToSetTo = true)
        {
            SessionHelper.SaveStringInSession(context,"loggedInStatus", statusToSetTo.ToString());
        }

        public static bool SetUserName(HttpContext context, string userName)
        {
            SessionHelper.SaveStringInSession(context, "userName", userName);

            //TODO: What to do when a User Name is already set?
            return true;
        }

        public static bool SetUserRole(HttpContext context, EAccessLevel role)
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

            if (userName == "")
                return "No User Logged in";
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

    }
}

using control.Generator;
using control.Manager;
using control.Models;
using Microsoft.EntityFrameworkCore;

namespace control.Helper
{
    public static class AccountHelper
    {

        public static bool LoginUser(HttpContext httpContext, string username, EAccessLevel level, string language)
        {
            if (UserStateManager.AddUserState(httpContext.Session.Id, GetUserName(httpContext),GetUserLanguage(httpContext)))
                AlertGenerator.AddAlertToSession(httpContext, AlertGenerator.EAlertLevel.kWarning, LanguageManager.GetLocalizedString("ACCOUNT_LOGOUT_PREVIOUS_SESSION_TERMINATED", AccountHelper.GetUserLanguage(httpContext)));

            return RefreshUser(httpContext, username, level, language);
        }

        public static bool LogoutUser(HttpContext httpContext)
        {
            //TODO: Make this better

            UserStateManager.RemoveState(httpContext.Session.Id);

            httpContext.Session.Clear();

            //TODO: Think, if anything needs to be caught here
            return true;
            //return false;
        }

        public static bool RefreshUser(HttpContext httpContext, string username, EAccessLevel level, string language)
        {
            SetLoggedInStatus(httpContext);
            SetUserName(httpContext, username);
            SetUserRole(httpContext, level);
            SetUserLanguage(httpContext, language);

            //TODO: Think, if anything needs to be caught here
            return true;
        }


        private static void SetLoggedInStatus(HttpContext context, bool statusToSetTo = true)
        {
            SessionHelper.SaveStringInSession(context,"loggedInStatus", statusToSetTo.ToString());
        }

        public static void SetUserLanguage(HttpContext context, string language = "en")
        {
            SessionHelper.SaveStringInSession(context, "language", language);
        }

        public static bool SetUserName(HttpContext context, string userName)
        {
            SessionHelper.SaveStringInSession(context, "userName", userName);

            return true;
        }

        private static bool SetUserRole(HttpContext context, EAccessLevel role)
        {
            SessionHelper.SaveIntInSession(context, "userRole", Convert.ToInt32(role));

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

        public static string GetUserLanguage(HttpContext context)
        {
            string language = SessionHelper.GetStringFromSession(context, "language");

            return language;
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

        public static string GetAccessLevelAsString(HttpContext context)
        {
            //As GetEAccessLevel already checks for null, we can ignore the warning with !
            switch (GetEAccessLevel(context))
            {
                case EAccessLevel.kNone:
                    return "USER_LEVEL_GUEST";
                case EAccessLevel.kUser:
                    return "USER_LEVEL_USER";
                case EAccessLevel.kModerator:
                    return "USER_LEVEL_MODERATOR";
                case EAccessLevel.kAdmin:
                    return "USER_LEVEL_ADMIN";
                case EAccessLevel.kSuperAdmin:
                    return "USER_LEVEL_SUPERADMIN";
                default:
                    return "Error";
            }
        }

        public static User GetLoggedInUser(control.Data.controlContext dataContext, HttpContext context)
        {
            if (!GetLoggedIn(context))
                return new User();

            string _userName = GetUserName(context);

            return dataContext.User.Where(e => e.UserName == _userName).FirstOrDefault()!;
        }

        public static string GetExpiryDate(control.Data.controlContext dataContext, HttpContext context)
        {
            if (!GetLoggedIn(context))
                return "";

            return GetLoggedInUser(dataContext, context).ExpiryDate.ToShortDateString();
        }

        public static IList<Door> GetAdminDoorList(control.Data.controlContext dataContext, HttpContext context)
        {   
            string _username = GetUserName(context);
            
            //Admins and Super Admins get all doors
            if (GetEAccessLevel(context) >= EAccessLevel.kAdmin)
                return dataContext.Door.ToList();


            //We need to get the user in another way here, otherwise the foreign list will not be loaded
            var _user = dataContext.User.Where(u => u.UserName == _username).Include(u => u.AdminDoors).First();
                return dataContext.Door.Where(d => _user.AdminDoors.Contains(d)).ToList();

        }

    }
}

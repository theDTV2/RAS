using control.Generator;
using control.Manager;
using control.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace control.Helper
{
    public static class AccountHelper
    {

        public static bool LoginUser(HttpContext httpContext, string userName, EAccessLevel level, string language)
        {
            if (UserStateManager.AddUserState(httpContext.Session.Id, GetUserName(httpContext)))
                AlertGenerator.AddAlertToSession(httpContext, AlertGenerator.EAlertLevel.kWarning, LanguageManager.GetLocalizedString("ACCOUNT_LOGOUT_PREVIOUS_SESSION_TERMINATED", AccountHelper.GetUserLanguage(httpContext)));

            return RefreshUser(httpContext, userName, level, language);
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

        public static bool RefreshUser(HttpContext httpContext, string userName, EAccessLevel level, string language)
        {
            SetLoggedInStatus(httpContext);
            SetUserName(httpContext, userName);
            SetUserRole(httpContext, level);
            SetUserLanguage(httpContext, language);

            //TODO: Think, if anything needs to be caught here
            return true;
        }


        private static void SetLoggedInStatus(HttpContext httpContext, bool statusToSetTo = true)
        {
            SessionHelper.SaveStringInSession(httpContext, "loggedInStatus", statusToSetTo.ToString());
        }

        public static void SetUserLanguage(HttpContext httpContext, string language = "en")
        {
            SessionHelper.SaveStringInSession(httpContext, "language", language);
        }

        public static bool SetUserName(HttpContext httpContext, string userName)
        {
            SessionHelper.SaveStringInSession(httpContext, "userName", userName);

            return true;
        }

        private static bool SetUserRole(HttpContext httpContext, EAccessLevel role)
        {
            SessionHelper.SaveIntInSession(httpContext, "userRole", Convert.ToInt32(role));

            return true;
        }

        public static bool GetLoggedIn(HttpContext httpContext)
        {
            if (SessionHelper.GetStringFromSession(httpContext, "loggedInStatus") == "")
                return false;
            return true;
        }
        public static string GetUserName(HttpContext httpContext)
        {
            string userName = SessionHelper.GetStringFromSession(httpContext, "userName");

            return userName;
        }

        public static string GetUserLanguage(HttpContext httpContext)
        {
            string language = SessionHelper.GetStringFromSession(httpContext, "language");

            return language;
        }

        public static EAccessLevel GetEAccessLevel(HttpContext httpContext)
        {

            if (!GetLoggedIn(httpContext))
                return EAccessLevel.kNone;

            int userRoleRaw = SessionHelper.GetIntFromSession(httpContext, "userRole");

             if (Enum.IsDefined(typeof(EAccessLevel),userRoleRaw))
                return (EAccessLevel)userRoleRaw;

            return EAccessLevel.kNone;
        }  

        public static string GetAccessLevelAsString(HttpContext httpContext)
        {
            //As GetEAccessLevel already checks for null, we can ignore the warning with !
            switch (GetEAccessLevel(httpContext))
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

        public static User GetLoggedInUser(control.Data.controlContext dataContext, HttpContext httpContext)
        {
            if (!GetLoggedIn(httpContext))
                return new User();

            string _userName = GetUserName(httpContext);

            return dataContext.User.Where(e => e.UserName == _userName).FirstOrDefault()!;
        }

        public static string GetExpiryDate(control.Data.controlContext dataContext, HttpContext httpContext)
        {
            if (!GetLoggedIn(httpContext))
                return "";

            return GetLoggedInUser(dataContext, httpContext).ExpiryDate.ToShortDateString();
        }

        public static IList<Door> GetAdminDoorList(control.Data.controlContext dataContext, HttpContext httpContext)
        {   
            string _username = GetUserName(httpContext);
            
            //Admins and Super Admins get all doors
            if (GetEAccessLevel(httpContext) >= EAccessLevel.kAdmin)
                return dataContext.Door.ToList();


            //We need to get the user in another way here, otherwise the foreign list will not be loaded
            var _user = dataContext.User.Where(u => u.UserName == _username).Include(u => u.AdminDoors).First();
                return dataContext.Door.Where(d => _user.AdminDoors.Contains(d)).ToList();

        }

    }
}

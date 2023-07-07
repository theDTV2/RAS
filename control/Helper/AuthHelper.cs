using control.Generator;
using control.Manager;
using control.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using static control.Helper.AccessHelper;

namespace control.Helper
{
    public class AuthHelper
    {
        public static async Task<bool> CreateLoginRequest(control.Data.controlContext dataContext, HttpContext httpContext, string email) 
        {
            var loginCode = CreateAndSetLoginCodeForUserAsync(dataContext, email);

            var loginKey = LoginKeyHelper.RegisterLoginKeyForUserAsync(dataContext, email);

            //TODO: Improve this
            AccountHelper.SetUserName(httpContext, email);


            //TODO: Log Login Request Creation
            await loginCode;
            await loginKey;

            EmailHelper.AddAccessMailToBeSent(httpContext, email, loginCode.Result, loginKey.Result);

            return true;
        }


        public static EAccessReturnValue ChallengeLoginRequestWithLoginCode(control.Data.controlContext dataContext, HttpContext httpContext, string email, string loginCode)
        {
            EAccessReturnValue result = AccessHelper.TryToAuthUserWithLoginCode(dataContext, email, loginCode, out User? user);

          
            if (result  == EAccessReturnValue.kAccessGranted)
            {
                AccountHelper.LoginUser(httpContext, email, user!.AccessLevel, user!.Language);
            }

            return result;
            
        }

        public static EAccessReturnValue ChallengeLoginRequestWithLoginKey(control.Data.controlContext dataContext, HttpContext httpContext, string loginKey)
        {
            EAccessReturnValue result = AccessHelper.TryToAuthUserWithLoginKey(dataContext, loginKey, out User? user);

            
            if (result == EAccessReturnValue.kAccessGranted)
            {
                AccountHelper.LoginUser(httpContext, user!.UserName, user!.AccessLevel, user!.Language);
            }

            return result;
        }


       public static EAccessReturnValue CheckUserPermission(control.Data.controlContext dataContext, HttpContext httpContext, EAccessLevel requiredAccessLevel = EAccessLevel.kUser, Door? doorToOpen = null)
        {
        
            //No User logged in
            if (!AccountHelper.GetLoggedIn(httpContext))
                return EAccessReturnValue.kAccessDenied;

            string userName = AccountHelper.GetUserName(httpContext);
            User user = GetUserAsync(dataContext, userName).Result!;

            //Check, if user permissions need to be refreshed

            if (UserStateManager.UpdatePermissionsRequired(httpContext.Session.Id))
            {
                AccountHelper.RefreshUser(httpContext, userName, user!.AccessLevel, user.Language);
            }

            //User State is not valid anymore (Somebody logged in with the same username)
            if (!UserStateManager.CheckUserState(httpContext.Session.Id))
            {
                AccountHelper.LogoutUser(httpContext);
                UserStateManager.RemoveState(httpContext.Session.Id);

                AlertGenerator.AddAlertToSession(httpContext, AlertGenerator.EAlertLevel.kError, LanguageManager.GetLocalizedString("ACCOUNT_INVALID_USER_STATE", AccountHelper.GetUserLanguage(httpContext)));

                return EAccessReturnValue.kAccessDenied;
            }


            //Check for Account expiry
            if (user.ExpiryDate < DateTime.Now)
                return EAccessReturnValue.kAccountExpired;

            //Agressively redirect users to the privacy page
            if (!user.AcceptedEula)
            {
                return EAccessReturnValue.kAccountEulaNotAccepted;
            }

            if (!user.CompletedRegistration)
            {
                return EAccessReturnValue.kAccountRegistrationNotCompleted;
            }

            if (user.CompletedRegistration == true && user.AccessLevel == EAccessLevel.kNone)
                return EAccessReturnValue.kAccountLocked;
            if (doorToOpen is not null)
            {
                //Check for Admin Rights
                if (user.AdminDoors.Contains(doorToOpen) || user.AccessLevel >= EAccessLevel.kAdmin)
                    return EAccessReturnValue.kAdminGranted;
            }

            if (user!.AccessLevel < requiredAccessLevel)
                return EAccessReturnValue.kPermissionDenied;

            // If we reach this point, the user can access
            return EAccessReturnValue.kAccessGranted;
        }

        //Works same as CheckUserPermission(..), but only returns true/false
        public static bool CheckUserAccess(control.Data.controlContext dataContext, HttpContext httpContext, EAccessLevel requiredAccessLevel = EAccessLevel.kUser, Door? doorToOpen = null)
        {
            EAccessReturnValue perm = CheckUserPermission(dataContext, httpContext, requiredAccessLevel, doorToOpen);
            if (perm != EAccessReturnValue.kAccessGranted && perm != EAccessReturnValue.kAdminGranted)
                return false;

            return true;

        }

        //Works same as CheckUserPermission(..), but retirects users, when they have no permission to open a specific page

        public static bool CheckUserAccessWithRedirect(control.Data.controlContext dataContext, HttpContext httpContext, EAccessLevel requiredAccessLevel = EAccessLevel.kUser, Door? doorToOpen = null, bool IgnoreExpiryDate = false)
        {
            EAccessReturnValue perm = CheckUserPermission(dataContext, httpContext, requiredAccessLevel, doorToOpen);

            if (perm == EAccessReturnValue.kAccountEulaNotAccepted || perm == EAccessReturnValue.kAccountRegistrationNotCompleted)
            {
                AlertGenerator.AddAlertToSession(httpContext, AlertGenerator.EAlertLevel.kWarning, LanguageManager.GetLocalizedString("ACCOUNT_ACCEPT_EULA_REQUIRED", AccountHelper.GetUserLanguage(httpContext)));
               
                httpContext.Response.Redirect("/Account/Login");
                AccountHelper.LogoutUser(httpContext);
                return false;
            }
          

            if (perm != EAccessReturnValue.kAccessGranted && perm != EAccessReturnValue.kAdminGranted)
            {
                //If we visit a "expiry allowed"-Page, we ignore this return here
                if (perm == EAccessReturnValue.kAccountExpired && IgnoreExpiryDate)
                    return true;

                //TODO: Redirect to proper Error page
                httpContext.Response.Redirect("/Index");
                return false;
            }
            return true;
        }

        
    }
}

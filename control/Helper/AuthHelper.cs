using control.Generator;
using control.Manager;
using control.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static control.Helper.AccessHelper;

namespace control.Helper
{
    public class AuthHelper
    {
        public static async Task<bool> CreateLoginRequest(control.Data.controlContext dataContext,HttpContext context, string email) 
        {
            var loginCode = AccessHelper.CreateAndSetLoginCodeForUserAsync(dataContext, email);

            var loginKey = LoginKeyHelper.RegisterLoginKeyForUserAsync(dataContext, email);

            //TODO: Improve this
            AccountHelper.SetUserName(context, email);


            //TODO: Log Login Request Creation
            await loginCode;
            await loginKey;

            EmailHelper.AddAccessMailToBeSent(context, email, loginCode.Result, loginKey.Result);

            return true;
        }


        public static EAccessReturnValue ChallengeLoginRequestWithLoginCode(control.Data.controlContext dataContext,HttpContext context, string email, string loginCode)
        {
            EAccessReturnValue result = AccessHelper.TryToAuthUserWithLoginCode(dataContext, email, loginCode, out User? user);

          
            if (result  == EAccessReturnValue.kAccessGranted)
            {
                AccountHelper.LoginUser(context, email, user!.AccessLevel);
            }

            return result;
            
        }

        public static EAccessReturnValue ChallengeLoginRequestWithLoginKey(control.Data.controlContext dataContext, HttpContext context, string loginKey)
        {
            EAccessReturnValue result = AccessHelper.TryToAuthUserWithLoginKey(dataContext, loginKey, out User? user);

            
            if (result == EAccessReturnValue.kAccessGranted)
            {
                AccountHelper.LoginUser(context, user!.UserName, user!.AccessLevel);
            }

            return result;
        }


       public static EAccessReturnValue CheckUserPermission(control.Data.controlContext dataContext, HttpContext context, EAccessLevel requiredAccessLevel = EAccessLevel.kUser, Door? doorToOpen = null)
        {
        
            //No User logged in
            if (!AccountHelper.GetLoggedIn(context))
                return EAccessReturnValue.kAccessDenied;

            string userName = AccountHelper.GetUserName(context);
            User user = GetUserAsync(dataContext, userName).Result!;

            //Check, if user permissions need to be refreshed

            if (UserStateManager.UpdatePermissionsRequired(context.Session.Id))
            {
                AccountHelper.RefreshUser(context, userName, user!.AccessLevel);
            }

            //User State is not valid anymore (Somebody logged in with the same username)
            if (!UserStateManager.CheckUserState(context.Session.Id))
            {
                AccountHelper.LogoutUser(context);
                UserStateManager.RemoveState(context.Session.Id);

                AlertGenerator.AddAlertToSession(context, AlertGenerator.EAlertLevel.kError, "Invalid User State. You have been logged out!");

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
                //Check for regular Access right
                if (!user.AccessDoors.Contains(doorToOpen))
                    return EAccessReturnValue.kPermissionDenied;

                //Check for Admin Rights
                if (user.AdminDoors.Contains(doorToOpen))
                    return EAccessReturnValue.kAdminGranted;

            }

            if (user!.AccessLevel < requiredAccessLevel)
                return EAccessReturnValue.kPermissionDenied;

            // If we reach this point, the user can access
            return EAccessReturnValue.kAccessGranted;
        }

        //Works same as CheckUserPermission(..), but only returns true/false
        public static bool CheckUserAccess(control.Data.controlContext dataContext, HttpContext context, EAccessLevel requiredAccessLevel = EAccessLevel.kUser, Door? doorToOpen = null)
        {
            EAccessReturnValue perm = CheckUserPermission(dataContext, context, requiredAccessLevel, doorToOpen);
            if (perm != EAccessReturnValue.kAccessGranted && perm != EAccessReturnValue.kAdminGranted)
                return false;

            return true;

        }

        //Works same as CheckUserPermission(..), but retirects users, when they have no permission to open a specific page

        public static bool CheckUserAccessWithRedirect(control.Data.controlContext dataContext, HttpContext context, EAccessLevel requiredAccessLevel = EAccessLevel.kUser, Door? doorToOpen = null)
        {
            EAccessReturnValue perm = CheckUserPermission(dataContext, context, requiredAccessLevel, doorToOpen);

            if (perm == EAccessReturnValue.kAccountEulaNotAccepted || perm == EAccessReturnValue.kAccountRegistrationNotCompleted)
            {
                AlertGenerator.AddAlertToSession(context, AlertGenerator.EAlertLevel.kWarning, "Please accept the EULA");
               
                context.Response.Redirect("/Account/Login");
                AccountHelper.LogoutUser(context);
                return false;
            }
          

            if (perm != EAccessReturnValue.kAccessGranted && perm != EAccessReturnValue.kAdminGranted)
            {
                //TODO: Redirect to proper Error page
                context.Response.Redirect("/Index");
                return false;
            }
            return true;
        }

        
    }
}

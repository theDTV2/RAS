using control.Generator;
using control.Models;
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

            EmailHelper.AddAccessMailToBeSent(loginCode.Result,loginKey.Result,  email);

            return true;
        }


        public static EAccessReturnValue ChallengeLoginRequestWithLoginCode(control.Data.controlContext dataContext,HttpContext context, string email, string loginCode)
        {
            EAccessReturnValue result = AccessHelper.TryToAuthUserWithLoginCode(dataContext, email, loginCode, out User? user);

            //TODO: Move this somewhere else
            if (result  == EAccessReturnValue.kAccessGranted)
            {
                AccountHelper.LoginUser(context, email, user!.AccessLevel);
            }

            return result;
            
        }

        public static EAccessReturnValue ChallengeLoginRequestWithLoginKey(control.Data.controlContext dataContext, HttpContext context, string loginKey)
        {
            EAccessReturnValue result = AccessHelper.TryToAuthUserWithLoginKey(dataContext, loginKey, out User? user);

            //TODO: Move this somewhere else
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

            //Check for Account expiry
            if (user.ExpiryDate < DateTime.Now)
                return EAccessReturnValue.kAccountExpired;

            //Agressively redirect users to the privacy page
            if (!user.AcceptedEula)
            {
                return EAccessReturnValue.kAccountEulaNotAccepted;
            }
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

            if (perm == EAccessReturnValue.kAccountEulaNotAccepted)
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

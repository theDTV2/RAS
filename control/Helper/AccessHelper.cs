using control.Data;
using control.Models;
using Microsoft.EntityFrameworkCore;

namespace control.Helper
{
    public static class AccessHelper
    {
        public enum AccessReturnValue { 
            kAccessGranted,
            kAccessDenied,
            kCodeExpired,
            kNoPermissionDenied,
            kAccountExpired,
            kAccountLocked
        }

        //TODO: Better way then to use system time?
        static System.Random random  = new System.Random();

        public static string GenerateAccessCode(int lenght = 18)
        {
            //We generate a random code with the given lenght but with at least given lenght.
            long new_code = random.NextInt64((long)Math.Pow(10, lenght), (long)Math.Pow(10, lenght+1));

            return Convert.ToString(new_code);
        }
        

        public static AccessReturnValue TryToVerifyUserWithAccessCode(control.Data.controlContext dataContext, HttpContext httpContext, string email, int AccessCode)
        {

            User userToCheck;
            try 
            {
                userToCheck = dataContext.User.Where(d => d.Email == email).Single();      
            }
            catch (InvalidOperationException)
            {
                //This is thrown, then no user with this name exists 
                //TODO: Handle this
                return AccessReturnValue.kAccessDenied;
            }

            catch (Exception)
            {
                //This shouldnt occur
                //TODO: Change this? Remove this?-
                return AccessReturnValue.kAccessDenied;
            }

            //TODO: Add timeout to key check
            if (userToCheck.AccessCode == AccessCode)
            {
                return AccessReturnValue.kAccessGranted;
            }

            return AccessReturnValue.kAccessDenied;

        }

        public static AccessReturnValue TryToVerifyUserWithHash(control.Data.controlContext dataContext, HttpContext httpContext, string username, string AccessCode)
        {
            throw new NotImplementedException();
     
        }


        public static bool LoginUser(HttpContext httpContext, string username)
        {
            throw new NotImplementedException();
            //return false;
        }

        public static bool LogoutUser(HttpContext httpContext, string username)
        {
            throw new NotImplementedException();
            //return false;
        }

    }

}

using control.Data;
using control.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;

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




        public static string CreateLoginCodeForUser(control.Data.controlContext dataContext, HttpContext httpContext, string email)
        {

        }




        public static async Task<AccessReturnValue> TryToVerifyUserWithAccessCode(control.Data.controlContext dataContext, HttpContext httpContext, string email, int AccessCode)
        {

            Task<User?> userToCheck = GetUserAsync(dataContext,email);

            await userToCheck;
            //TODO: Add timeout to key check
            if (userToCheck.Result is null)
            {
                return AccessReturnValue.kAccessDenied;
            }

            if (userToCheck.Result.AccessCode is null)
            {
                return AccessReturnValue.kAccessDenied;
            }

            if (userToCheck.Result.AccessCode.Value == AccessCode)
            {
                return AccessReturnValue.kAccessGranted;
            }


            return AccessReturnValue.kAccessDenied;

        }

        public static async Task<AccessReturnValue> TryToVerifyUserWithHash(control.Data.controlContext dataContext, HttpContext httpContext, string email, string AccessCode)
        {
            Task<User?> userToCheck = GetUserAsync(dataContext, email);
            

            await userToCheck;
            if (userToCheck.Result is null)
            {
                return AccessReturnValue.kAccessDenied;
            }
            if (userToCheck.Result.AccessCode is null)
            {
                return AccessReturnValue.kAccessDenied;
            }


            if (HashHelper.CompareIntToHashString(userToCheck.Result.AccessCode.Value, AccessCode))
                return AccessReturnValue.kAccessGranted;

            return AccessReturnValue.kAccessDenied;


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

   

    private static Task<User?> GetUserAsync(control.Data.controlContext dataContext, string email)
        {
            return dataContext.User.Where(e => e.Email == email).FirstOrDefaultAsync();
        }

        private static string GenerateAccessCode(int lenght = 18)
        {
            //We generate a random code with the given lenght but with at least given lenght.
            long new_code = random.NextInt64((long)Math.Pow(10, lenght), (long)Math.Pow(10, lenght + 1));

            return Convert.ToString(new_code);
        }


    }

}

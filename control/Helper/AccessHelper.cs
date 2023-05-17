using control.Data;
using control.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using System.ComponentModel;
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


        public static async Task<User> CreateUserIfNotExistant(control.Data.controlContext dataContext, string email)
        {
            User? user = await GetUserAsync(dataContext, email);

            //If User is null, we create a new one
            user ??= await CreateUserAsync(dataContext, email);

            return user;
        }

        public static async Task<string> CreateAndSetLoginCodeForUser(control.Data.controlContext dataContext, string email)
        {
            User? user = await CreateUserIfNotExistant(dataContext, email);


            string accessCode =  user.AccessCode = GenerateAccessCode();
            await dataContext.SaveChangesAsync();

            return accessCode;
        }

        public static AccessReturnValue TryToVerifyUserWithAccessCode(control.Data.controlContext dataContext, string email, string AccessCode, out User? user)
        {

            User? userToCheck = GetUserAsync(dataContext,email).Result;
            user = userToCheck;
            //TODO: Add timeout to key check
            if (userToCheck is null)
            {
                return AccessReturnValue.kAccessDenied;
            }

            if (userToCheck.AccessCode is null)
            {
                return AccessReturnValue.kAccessDenied;
            }

            if (userToCheck.AccessCode == AccessCode)
            {
                return AccessReturnValue.kAccessGranted;
            }


            return AccessReturnValue.kAccessDenied;

        }

        public static AccessReturnValue TryToVerifyUserWithHash(control.Data.controlContext dataContext, string email, string AccessCode, out User? user)
        {
            User? userToCheck = GetUserAsync(dataContext, email).Result;
            user = userToCheck;
            if (userToCheck is null)
            {
                return AccessReturnValue.kAccessDenied;
            }
            if (userToCheck.AccessCode is null)
            {
                return AccessReturnValue.kAccessDenied;
            }

            if (HashHelper.CompareStringToHashString(userToCheck.AccessCode, AccessCode))
                return AccessReturnValue.kAccessGranted;

            return AccessReturnValue.kAccessDenied;


        }

        private static Task<User?> GetUserAsync(control.Data.controlContext dataContext, string email)
        {
            return dataContext.User.Where(e => e.Email == email).FirstOrDefaultAsync();
        }

        private static async Task<User> CreateUserAsync(control.Data.controlContext dataContext, string email)
        {
            User newUser = new()
            {
                Email = email,
                AccessLevel = EAccessLevel.kNone
            };
            dataContext.User.Add(newUser);
            await dataContext.SaveChangesAsync();

            return newUser;
        }

        private static string GenerateAccessCode(int lenght = 9)
        {
            //We generate a random code with the given lenght but with at least given lenght.
            long new_code = random.NextInt64((long)Math.Pow(10, lenght), (long)Math.Pow(10, lenght + 1));

            return Convert.ToString(new_code);
        }


    }

}

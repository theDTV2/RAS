using control.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.IdentityModel.Protocols;
using System.ComponentModel;
using System.Security.Cryptography;

namespace control.Helper
{
    public static class LoginKeyHelper
    {

        public static async Task<string> RegisterLoginKeyForUserAsync(Data.controlContext dataContext, string email)
        {
            //1. Check, if user exists. If not -> Create 
            //2. Generate Login Code
            //3. Return True

            var user = AccessHelper.GetOrCreateUserAsync(dataContext, email);

            string loginKey = GenerateLoginCode();

            await user;


            LoginLink loginLink = new LoginLink { Email = email, GenerationTime = DateTime.Now, Key = loginKey };

            dataContext.Add(loginLink);

            dataContext.SaveChanges();

            //TODO: Can this fail?
            return loginKey;
        }

        public static async Task<EAccessReturnValue> VerifyLoginKeyForUserAsync(Data.controlContext dataContext, string key)
        {
            LoginLink? loginLink = await GetLoginLinkAsync(dataContext, key);


            if (loginLink is null)
                return EAccessReturnValue.kAccessDenied;

            //TODO: Add customizable timeout 
            if ((loginLink.GenerationTime - DateTime.Now) > TimeSpan.FromMinutes(60))
                return EAccessReturnValue.kCodeExpired;


            await RemoveUserFromLoginListAsync(dataContext, key);
            return EAccessReturnValue.kAccessGranted;
        }

        public static async Task<User?> RemoveUserFromLoginListAsync(Data.controlContext dataContext, string key)
        {
            //Removes an LoginLink from the LoginList

            LoginLink? loginLink = await GetLoginLinkAsync(dataContext, key);

            if (loginLink is null)
                return null;

            User? user = dataContext.User.Where(u => u.Email == loginLink.Email).FirstOrDefault();

            dataContext.LoginLink.Remove(loginLink);

            dataContext.SaveChanges();

            return user;

        }

        private static string GenerateLoginCode(int Lenght = 64)
        {
            byte[] code = RandomNumberGenerator.GetBytes(Lenght);
            return Convert.ToBase64String(code);
        }


        private static async Task<LoginLink?> GetLoginLinkAsync(Data.controlContext dataContext, string key)
        {
            return await dataContext.LoginLink.Where(l => l.Key == key).FirstOrDefaultAsync();
        }

    }
}

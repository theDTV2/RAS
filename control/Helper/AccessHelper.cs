using control.Data;
using control.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.ComponentModel;
using System.Net.Cache;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;

namespace control.Helper
{
    public static class AccessHelper
    {

        //TODO: Better way then to use system time?
        static System.Random random = new System.Random();


        public static async Task<User> GetOrCreateUserAsync(control.Data.controlContext dataContext, string email)
        {
            User? user = await GetUserAsync(dataContext, email);

            //If User is null, we create a new one
            user ??= await CreateUserAsync(dataContext, email);

            return user;
        }

        public static Task<User?> GetUserAsync(control.Data.controlContext dataContext, string userName)
        {
            return dataContext.User.Where(e => e.UserName == userName).FirstOrDefaultAsync();
        }

        public static async Task<string> CreateAndSetLoginCodeForUserAsync(control.Data.controlContext dataContext, string email)
        {
            User? user = await GetOrCreateUserAsync(dataContext, email);


            string accessCode = user.AccessCode = GenerateAccessCode();
            user.AccessCodeGenerationTime = DateTime.Now;

            await dataContext.SaveChangesAsync();

            return accessCode;
        }

        public static EAccessReturnValue TryToAuthUserWithLoginCode(control.Data.controlContext dataContext, string userName, string loginCode, out User? user)
        {
            User? userToCheck = GetUserAsync(dataContext, userName).Result;
            user = userToCheck;
            //TODO: Add timeout to key check
            if (userToCheck is null)
            {
                return EAccessReturnValue.kAccessDenied;
            }

            if (userToCheck.AccessCode is null)
            {
                return EAccessReturnValue.kAccessDenied;
            }

            if (userToCheck.AccessCode == loginCode)
            {
                //Remove login key after login
                LoginKeyHelper.TryToRemoveLoginLinkByUserName(dataContext, userName);

                //Remove Access Code after login
                userToCheck.AccessCode = String.Empty;
                userToCheck.AccessCodeGenerationTime = DateTime.MinValue;
                user.LastLogin = DateTime.Now;

                dataContext.SaveChanges();

                return EAccessReturnValue.kAccessGranted;
            }

            return EAccessReturnValue.kAccessDenied;
        }

        public static EAccessReturnValue TryToAuthUserWithLoginKey(control.Data.controlContext dataContext, string loginKey, out User? user)
        {
            var rval = LoginKeyHelper.VerifyLoginKeyForUserAsync(dataContext, loginKey).Result;
            user = null;

            if (rval == EAccessReturnValue.kAccessGranted)
            {
                user = LoginKeyHelper.RemoveUserFromLoginListAsync(dataContext, loginKey).Result;

                //Case: User is deleted during login process
                if (user is null)
                    return EAccessReturnValue.kAccessDenied;

                //Remove Access Code after login
                user.AccessCode = String.Empty;
                user.AccessCodeGenerationTime = DateTime.MinValue;
                user.LastLogin = DateTime.Now;

                dataContext.SaveChanges();
            }
            return rval;

        }
        [Obsolete("Not needed, we do not hash the access code anymore")]
        public static EAccessReturnValue TryToVerifyUserWithHash(control.Data.controlContext dataContext, string email, string AccessCode, out User? user)
        {
            User? userToCheck = GetUserAsync(dataContext, email.ToLower()).Result;
            user = userToCheck;
            if (userToCheck is null)
            {
                return EAccessReturnValue.kAccessDenied;
            }
            if (userToCheck.AccessCode is null)
            {
                return EAccessReturnValue.kAccessDenied;
            }

            if (HashHelper.CompareStringToHashString(userToCheck.AccessCode, AccessCode))
                return EAccessReturnValue.kAccessGranted;

            return EAccessReturnValue.kAccessDenied;


        }

        private static async Task<User> CreateUserAsync(control.Data.controlContext dataContext, string userName)
        {
            User newUser = new()
            {
                UserName = userName,
                AccessLevel = EAccessLevel.kNone,
                ExpiryDate = DateHelper.GenerateCurrentSemesterEnd()

            };
            dataContext.User.Add(newUser);
            await dataContext.SaveChangesAsync();

            return newUser;
        }

        public static void UpdateModeratorStatus(control.Data.controlContext dataContext, string[] userNames, string[] oldDoorAdminList)
        {
            User? _user;
            bool _editedSomething = false;


            //Only when a username was added/removed, we need to operate over it
            string[] _userNamesToIterateOver = userNames.Except(oldDoorAdminList).Union(oldDoorAdminList.Except(userNames)).ToArray();

            var a = userNames.Except(oldDoorAdminList);

            var b = oldDoorAdminList.Except(userNames);


            if (_userNamesToIterateOver.IsNullOrEmpty())
                return;

            foreach (string name in _userNamesToIterateOver)
            {
                _user = dataContext.User.Where(u => u.UserName == name).Include(u => u.AdminDoors).FirstOrDefault();

                if (_user is not null)
                {
                    if (_user.AdminDoors.IsNullOrEmpty() && _user.AccessLevel == EAccessLevel.kModerator)
                    {
                        _user.AccessLevel = EAccessLevel.kUser;
                        _editedSomething = true;
                    }

                    if (!_user.AdminDoors.IsNullOrEmpty() && _user.AccessLevel == EAccessLevel.kUser)
                    {
                        _user.AccessLevel = EAccessLevel.kModerator;
                        _editedSomething = true;
                    }
                }
            }

            if (!_editedSomething)
                return;

            dataContext.SaveChanges();

            return;
        }

        private static string GenerateAccessCode(int lenght = 9)
        {
            //We generate a random code with the given lenght but with at least given lenght.
            long new_code = random.NextInt64((long)Math.Pow(10, lenght), (long)Math.Pow(10, lenght + 1));

            return Convert.ToString(new_code);
        }

    }

}

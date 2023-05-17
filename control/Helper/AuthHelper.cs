using control.Models;
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
                AccountHelper.LoginUser(context, user!.Email, user!.AccessLevel);
            }

            return result;
        }
        
    }
}

using control.Models;
using static control.Helper.AccessHelper;

namespace control.Helper
{
    public class AuthHelper
    {
        public static async Task<string> CreateLoginRequest(control.Data.controlContext dataContext,HttpContext context, string email) 
        {
            var loginCode = AccessHelper.CreateAndSetLoginCodeForUser(dataContext, email);

            //TODO: Improve this
            AccountHelper.SetUserName(context, email);
            //TODO: Log Login Request Creation
            await loginCode;
            EmailHelper.AddAccessMailToBeSent(loginCode.Result,email);

            return loginCode.Result;
        }


        public static  EAccessReturnValue ChallengeLoginRequestWithAccessCode(control.Data.controlContext dataContext,HttpContext context, string email, string accessCode)
        {

            EAccessReturnValue result = AccessHelper.TryToVerifyUserWithAccessCode(dataContext, email, accessCode, out User? user);
            if (result  == EAccessReturnValue.kAccessGranted)
            {
                AccountHelper.LoginUser(context, email, user!.AccessLevel);
            }
            return result;
            
        }
    }
}

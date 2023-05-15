using control.Models;
using static control.Helper.AccessHelper;

namespace control.Helper
{
    public class AuthHelper
    {
        public static async Task<string> CreateLoginRequest(control.Data.controlContext dataContext, string email) 
        {
            var loginCode = AccessHelper.CreateAndSetLoginCodeForUser(dataContext, email);

            //TODO: Log Login Request Creation
            await loginCode;
            EmailHelper.AddAccessMailToBeSent(loginCode.Result,email);

            return loginCode.Result;
        }


        public static  AccessReturnValue ChallengeLoginRequestWithAccessCode(control.Data.controlContext dataContext,HttpContext context, string email, string accessCode)
        {

            AccessReturnValue result = AccessHelper.TryToVerifyUserWithAccessCode(dataContext, email, accessCode, out User? user);
            if (result  == AccessReturnValue.kAccessGranted)
            {
                AccountHelper.LoginUser(context, email, user!.AccessLevel);
            }
            return result;
            
        }
    }
}

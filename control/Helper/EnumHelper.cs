using control.Models;

namespace control.Helper
{
    public class EnumHelper
    {

        public static string ConvertEAccessReturnValueToString(EAccessReturnValue value)
        {
            switch (value)
            {
                case EAccessReturnValue.kAccessGranted:
                    return "Access Granted";
                case EAccessReturnValue.kAdminGranted:
                    return "Admin Access Granted";
                case EAccessReturnValue.kAccessDenied:
                    return "Access denied";
                case EAccessReturnValue.kPermissionDenied:
                    return "Access denied: No permission";
                case EAccessReturnValue.kAccountExpired:
                    return "Access denied: Account expired";
                case EAccessReturnValue.kAccountLocked:
                    return "Access denied: Account locked";
                case EAccessReturnValue.kAccountEulaNotAccepted:
                    return "Access denied: Eula not accepted";
                case EAccessReturnValue.kAccountRegistrationNotCompleted:
                    return "Access denied: Registration not completed";
                case EAccessReturnValue.kAccountNotFound:
                    return "Access denied: Account not found";
                default:
                    return "error";
            }
        }
    }
}

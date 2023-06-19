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
                    return "Access denied\nNo permission";
                case EAccessReturnValue.kAccountExpired:
                    return "Access denied\nAccount expired";
                case EAccessReturnValue.kAccountLocked:
                    return "Access denied\nAccount locked";
                case EAccessReturnValue.kAccountEulaNotAccepted:
                    return "Access denied\nEula not accepted";
                case EAccessReturnValue.kAccountRegistrationNotCompleted:
                    return "Access denied\nRegistration not completed";
                case EAccessReturnValue.kAccountNotFound:
                    return "Access denied\nAccount not found";
                default:
                    return "error";
            }
        }
    }
}

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.IdentityModel.Tokens;

namespace control.Helper
{
    public static class SessionHelper
    {
        public static bool SaveStringInSession(HttpContext context, string ToSave, string Key, bool CheckIfOccupied = false)
        {
            if (CheckIfOccupied)
            {
                if (context.Session.GetString(Key) != null)
                    return false;
            }

            context.Session.SetString(Key, ToSave);
            return true;
                }

        public static bool SaveIntInSession(HttpContext context, int ToSave, string Key, bool CheckIfOccupied = false)
        {
            if (CheckIfOccupied)
            {
                if (context.Session.GetInt32(Key) != null)
                    return false;
            }

            context.Session.SetInt32(Key, ToSave);
            return true;
        }

        public static bool SaveDateTimeInSession(HttpContext context, DateTime ToSave, string Key, bool CheckIfOccupied = false)
        {
            string DateTimeAsString = Convert.ToString(ToSave);
            if (CheckIfOccupied)
            {
                if (context.Session.GetString(Key) != null)
                    return false;
            }



            return true;

        }

        public static string GetStringFromSession(HttpContext context, string ToSave, string Key)
        {
            string? toReturn = context.Session.GetString(Key));
            if (string.IsNullOrEmpty(toReturn))
                return "";

            return toReturn;

        }

        public static int GetIntFromSession(HttpContext context, string ToSave, string Key)
        {
            int? ToReturn = context.Session.GetInt32(Key);
            if (!ToReturn.HasValue)
                return 0;

            return ToReturn.Value;
        }

        public static DateTime GetDateTimeFromSession(HttpContext context, string ToSave, string Key)
        {
            string? DateTimeString = context.Session.GetString(Key);

            if (DateTimeString.IsNullOrEmpty())
                return DateTime.Now;

            DateTime ToReturn = Convert.ToDateTime(DateTimeString);
            return ToReturn;
        }


    }
}
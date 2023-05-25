using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace control.Helper
{
    public static class SessionHelper
    {
        public static bool SaveStringInSession(HttpContext context, string key, string toSave, bool checkIfOccupied = false)
        {
            if (checkIfOccupied)
            {
                if (context.Session.GetString(key) != null)
                    return false;
            }

            context.Session.SetString(key, toSave);
            return true;
                }

        public static bool SaveIntInSession(HttpContext context, string key, int toSave, bool checkIfOccupied = false)
        {
            if (checkIfOccupied)
            {
                if (context.Session.GetInt32(key) != null)
                    return false;
            }

            context.Session.SetInt32(key, toSave);
            return true;
        }

        public static bool SaveDateTimeInSession(HttpContext context, string key, DateTime toSave, bool checkIfOccupied = false)
        {
            string DateTimeAsString = Convert.ToString(toSave);
            if (checkIfOccupied)
            {
                if (context.Session.GetString(key) != null)
                    return false;
            }

            return true;

        }

        public static string GetStringFromSession(HttpContext context, string key)
        {
            string? toReturn = context.Session.GetString(key);
            if (string.IsNullOrEmpty(toReturn))
                return "";

            return toReturn;

        }

        public static int GetIntFromSession(HttpContext context, string key)
        {
            int? toReturn = context.Session.GetInt32(key);
            if (!toReturn.HasValue)
                return 0;

            return toReturn.Value;
        }

        public static DateTime GetDateTimeFromSession(HttpContext context, string key)
        {
            string? DateTimeString = context.Session.GetString(key);

            if (String.IsNullOrEmpty(DateTimeString))
                return DateTime.Now;

            DateTime toReturn = Convert.ToDateTime(DateTimeString);
            return toReturn;
        }


    }
}
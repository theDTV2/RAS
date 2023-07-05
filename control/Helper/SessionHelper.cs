using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace control.Helper
{
    public static class SessionHelper
    {
        public static bool SaveStringInSession(HttpContext httpContext, string key, string toSave, bool checkIfOccupied = false)
        {
            if (checkIfOccupied)
            {
                if (httpContext.Session.GetString(key) != null)
                    return false;
            }

            httpContext.Session.SetString(key, toSave);
            return true;
                }

        public static bool SaveIntInSession(HttpContext httpContext, string key, int toSave, bool checkIfOccupied = false)
        {
            if (checkIfOccupied)
            {
                if (httpContext.Session.GetInt32(key) != null)
                    return false;
            }

            httpContext.Session.SetInt32(key, toSave);
            return true;
        }

        public static bool SaveDateTimeInSession(HttpContext httpContext, string key, DateTime toSave, bool checkIfOccupied = false)
        {
            string DateTimeAsString = Convert.ToString(toSave);
            if (checkIfOccupied)
            {
                if (httpContext.Session.GetString(key) != null)
                    return false;
            }

            httpContext.Session.SetString(key, DateTimeAsString);

            return true;

        }

        public static string GetStringFromSession(HttpContext httpContext, string key)
        {
            string? toReturn = httpContext.Session.GetString(key);
            if (string.IsNullOrEmpty(toReturn))
                return "";

            return toReturn;

        }

        public static int GetIntFromSession(HttpContext httpContext, string key)
        {
            int? toReturn = httpContext.Session.GetInt32(key);
            if (!toReturn.HasValue)
                return 0;

            return toReturn.Value;
        }

        public static DateTime GetDateTimeFromSession(HttpContext httpContext, string key)
        {
            string? DateTimeString = httpContext.Session.GetString(key);

            if (String.IsNullOrEmpty(DateTimeString))
                return DateTime.Now;

            DateTime toReturn = Convert.ToDateTime(DateTimeString);
            return toReturn;
        }


    }
}
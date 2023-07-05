using System.Globalization;
using System.Reflection;
using System.Resources;

namespace control.Manager
{
    public static class LanguageManager
    {
        private static readonly ResourceManager ResManager = new("control.Resources.Localization", Assembly.GetExecutingAssembly());

        private static readonly IDictionary<string, CultureInfo> Languages = new Dictionary<string, CultureInfo>();

        static LanguageManager()
        {
            //Adding supported lanuages to dict, if a specific lang is not found we fallback to eng
            Languages.Add("en", new CultureInfo("en"));
            Languages.Add("de", new CultureInfo("de"));
        }

        public static string GetLocalizedString(string stringName, string language)
        {
            if (language == "" || language is null)
                language = "en";

            string? returnValue = ResManager.GetString(stringName, Languages[language]);

            if (returnValue is null || returnValue == string.Empty)
                return stringName;
            return returnValue;
        }

        public static bool IsValidLanguage(string language)
        {
            if (language == null) 
                return false;

            return Languages.ContainsKey(language);  
        }
    }
}

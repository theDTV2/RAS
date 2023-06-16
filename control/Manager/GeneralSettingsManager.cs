using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace control.Manager
{
    public static class GeneralSettingsManager
    {
        public static GeneralSettings GeneralSettingsObj { get; private set; } = new GeneralSettings();

        public static bool LoadGeneralSettingsFromConfig()
        {
            //TODO: Make Config location changable
            //TODO: Catch errors
            string read = System.IO.File.ReadAllText("MailSettings.cfg");

            if (read is null)
                return false;

            GeneralSettings? _localGeneralSettings = JsonSerializer.Deserialize<GeneralSettings>(read);

            if (_localGeneralSettings == null)
                return false;

            GeneralSettingsObj = _localGeneralSettings;

            return true;
        }

        public static bool SaveGeneralSettingsToConfig()
        {
            string _toWrite = JsonSerializer.Serialize<GeneralSettings>(GeneralSettingsObj);

            //TODO: Make Config location changable
            //TODO: Catch errors

            if (System.IO.File.Exists("GeneralSettings.cfg"))
                System.IO.File.Delete("GeneralSettings.cfg");

            System.IO.File.WriteAllText("GeneralSettings.cfg", _toWrite);

            return true;
        }

        public static TimeSpan GetUserLoginTimeout()
        {
            return GeneralSettingsObj.UserLoginTimeout;
        }

    }


    public class GeneralSettings
    {
        public TimeSpan UserLoginTimeout { get; set; } = TimeSpan.FromMinutes(15);


    }
}

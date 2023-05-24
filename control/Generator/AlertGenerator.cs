using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace control.Generator
{
    public static class AlertGenerator
    {
        public enum EAlertLevel
        {
            kInformation,
            kWarning,
            kSuccess,
            kError
        }

        public static string GetAlertsFromSession(HttpContext httpContext)
        {
            string _alertMessages = httpContext.Session.GetString("Alerts") ?? "";
            if (!_alertMessages.IsNullOrEmpty())
                httpContext.Session.SetString("Alerts", "");

            return _alertMessages;
        }


        public static bool AddAlertToSession(HttpContext httpContext, EAlertLevel alertType, string message)
        {
            //If Alerts is empty, we create a new empty string
            string _alertMessages = httpContext.Session.GetString("Alerts") ?? "";
            string _alertTypeString = "";



            switch (alertType)
            {
                case EAlertLevel.kInformation:
                    _alertTypeString = "alert-info";
                    break;
                case EAlertLevel.kWarning:
                    _alertTypeString = "alert-warning";
                    break;
                case EAlertLevel.kSuccess:
                    _alertTypeString = "alert_success";
                    break;
                case EAlertLevel.kError:
                    _alertTypeString = "alert-error";
                    break;
            }
            string _newMessage = "<div class=\"alert alert-dismissible " + _alertTypeString + "\">" +
              "< button type = \"button\" class=\"close\" data-dismiss=\"alert\">×</button> \" " +
              " <strong>" + message + "</strong > </div >";

            _alertMessages += _newMessage;

            httpContext.Session.SetString("Alerts", _alertMessages);

            return true;
        }
    }
}

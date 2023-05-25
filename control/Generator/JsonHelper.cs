using control.Models;
using Microsoft.AspNetCore.DataProtection;
using System.Text.Json.Nodes;

namespace control.Generator
{

    public class JsonHelper
    {
        public static JsonArray GenerateRegistrationResponse(string displayText, EDoorEntryMode entryMode, string secret, DateTime timeStamp)
        {
            return new JsonArray()
            {
                  displayText,
                   secret,
                   timeStamp,
                   timeStamp
            };
        }

        public static JsonArray GenerateAccessRequestResponse(string doorResponse, EDoorAccessResponse accessResponse, string displayText, DateTime timeStamp)
        {
            return new JsonArray()
            {
                  doorResponse,
                   accessResponse,
                   displayText,
                   timeStamp
            };
        }

        public static JsonArray GenerateHeartBeatResponse(EDoorEntryMode entryMode, string displayText, DateTime timeStamp)
        {
            return new JsonArray()
            {
                  entryMode,
                   displayText,
                   timeStamp
            };
        }
    }
}

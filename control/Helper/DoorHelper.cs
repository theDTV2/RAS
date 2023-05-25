using control.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace control.Helper
{
    public static class DoorHelper
    {
        public static async bool AttemptToRegisterDoorAsync(control.Data.controlContext dataContext, string doorID, DateTime timeStamp, out JsonResult returnValue)
        {
            Door? _door = GetDoor(dataContext, doorID);
            returnValue = new JsonResult("");

           //No door with this name found
            if (_door is null)
                return false;

            //This door already is registered
            if (_door.Registered)
                return false;

            //Reject Attempt, if the delay is > 15 seconds
            if (DateTime.Now - timeStamp > TimeSpan.FromSeconds(15))
                return false;

            string _newToken = HashHelper.GenerateRandomBase64String(128);

            _door.Secret = _newToken;
            _door.Registered = true;

            var _saving = dataContext.SaveChangesAsync();

            JsonObject _toReturnJsonRaw = new JsonObject()
            {
                { "doorID" , doorID},
                { "token" , _newToken},
                { "timeStamp" , DateTime.Now},
                { "displayText", _door.DisplayName}

            };

            returnValue = new JsonResult(_toReturnJsonRaw);

            await _saving;
            return true;

        }

        public static EDoorAccessResponse RequestDoorAccess(control.Data.controlContext dataContext, string doorID, DateTime timeStamp, out JsonResult returnValue)
        {
            throw new NotImplementedException();
        }

        public static bool RegisterHeartBeat(control.Data.controlContext dataContext, EDoorEntryMode mode, string displayText,DateTime timeStamp, out JsonResult returnValue)
        {
            throw new NotImplementedException();
        }


        private static Door? GetDoor(control.Data.controlContext dataContext, string doorID)
        {
            return dataContext.Door.Where(e => e.Id == doorID).FirstOrDefault();
        }

    }
}

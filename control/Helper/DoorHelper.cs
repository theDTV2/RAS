using control.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;
using System.Text.Json.Nodes;
using System.Threading;

namespace control.Helper
{
    public static class DoorHelper
    {

        public static bool AttemptToRegisterDoor(control.Data.controlContext dataContext, string doorID, DateTime timeStamp, ref JsonResult returnValue)
        {
            Door? _door = GetDoor(dataContext, doorID);

            //No door with this name found
            if (_door is null)
                return false;

            //This door already is registered
            if (_door.Registered)
                return false;

            if (CheckIfTimedOut(timeStamp))
                return false;

            string _newToken = HashHelper.GenerateRandomBase64String(128);

            _door.Secret = _newToken;
            _door.Registered = true;

            var _saving = dataContext.SaveChanges();

            JsonObject _toReturnJsonRaw = new JsonObject()
            {
                { "doorID" , doorID},
                { "token" , _newToken},
                { "timeStamp" , DateTime.Now},
                { "displayText", _door.DisplayName}

            };

            returnValue = new JsonResult(_toReturnJsonRaw);

            return true;

        }

        public static bool RequestDoorAccess(control.Data.controlContext dataContext, string doorID, string accessCode, DateTime timeStamp, ref JsonResult returnValue)
        {
            Door? _door = GetDoor(dataContext, doorID);
            User? _user = dataContext.User.Where(u => u.SecretCode == accessCode).FirstOrDefault();

            JsonObject _toReturnJsonRaw;


            if (CheckForDoorAccessRestrictions(_door, timeStamp) && CheckForUserAccessRestrictions(_user, _door!, timeStamp))
            {
                _toReturnJsonRaw = new JsonObject()
                {
                    { "doorResponse" , true},
                    { "displayText" ,""},
                    { "timeStamp",  DateTime.Now}
                };

                return true;

            }
            _toReturnJsonRaw = new JsonObject()
            {
                { "doorResponse" , false},
                { "doorStatus" , ""},
                { "displayText" ,""},
                { "timeStamp",  DateTime.Now}

            };

            //If we reached until here, access is allowed :)
            return true;
        }

        public static bool RegisterHeartBeat(control.Data.controlContext dataContext, EDoorEntryMode mode, string displayText, DateTime timeStamp, out JsonResult returnValue)
        {
            throw new NotImplementedException();
        }

        private static bool CheckForDoorAccessRestrictions(Door? door, DateTime timeStamp)
        {
            //No door with this name found
            if (door is null)
                return false;

            //This door not registered yet or locked
            if (!door.Registered || door.EntryStatus == EDoorEntryMode.kNoAccess)
                return false;

            if (CheckIfTimedOut(timeStamp))
                return false;

            return true;
        }
        private static bool CheckForUserAccessRestrictions(User? user, Door door, DateTime timeStamp)
        {
            //No user exists with this access code
            if (user is null)
                return false;

            //User is locked
            if (user.AccessLevel > EAccessLevel.kNone)
                return false;

            //User is expired
            if (CheckIfExpired(user.ExpiryDate))
                return false;

            //User is not allowed to access door
            if (!user.AccessDoors.Contains(door))
                return false;

            return true;
        }


        private static Door? GetDoor(control.Data.controlContext dataContext, string doorID)
        {
            return dataContext.Door.Where(e => e.Id == doorID).FirstOrDefault();
        }

        private static bool CheckIfTimedOut(DateTime timeStamp, TimeSpan? timeOut = null)
        {
            if (timeOut is null)
                timeOut = TimeSpan.FromSeconds(5);

            return (DateTime.Now - timeStamp) < timeOut;
        }

        private static bool CheckIfExpired(DateTime timeStamp)
        {
            return CheckIfTimedOut(timeStamp, TimeSpan.Zero);
        }
    }
}

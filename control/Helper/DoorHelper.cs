using control.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using NuGet.Packaging.Signing;
using System.Text.Json.Nodes;
using System.Threading;

namespace control.Helper
{
    public static class DoorHelper
    {

        public static bool AttemptToRegisterDoor(control.Data.controlContext dataContext, string doorID, string timeStamp, ref JsonResult returnValue)
        {
            Door? _door = GetDoor(dataContext, doorID);

            //No door with this name found
            if (_door is null)
                return false;

            //This door already is registered
            if (_door.Registered)
                return false;

            if (!DateTime.TryParse(timeStamp, out DateTime _timeStamp))
                return false;

            //Request timed out
            if (CheckIfTimedOut(_timeStamp))
                return false;

            string _newToken = HashHelper.GenerateRandomBase64String(128);

            _door.Secret = _newToken;
            _door.Registered = true;
            _door.LastCheckInTime = DateTime.Now;

            dataContext.SaveChanges();

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

        public static bool RequestDoorAccess(control.Data.controlContext dataContext, string doorId, string secret, string cardCode, string timeStamp, ref JsonResult returnValue)
        {
            Door? _door = GetDoor(dataContext, doorId);
            User? _user = dataContext.User.Where(u => u.SecretCode == cardCode).FirstOrDefault();

            if (_door is null) 
                return false;
            if (_user is null) 
                return false;


            JsonObject _toReturnJsonRaw;

            if (!DateTime.TryParse(timeStamp, out DateTime _timeStamp))
                return false;


            if (CheckForDoorRestrictions(_door, secret, _timeStamp) && CheckForUserAccessRestrictions(_user, _door!))
            {
                _toReturnJsonRaw = new JsonObject()
                {
                    { "doorResponse" , true},
                    { "displayText" ,""},
                    { "timeStamp",  DateTime.Now}
                };
                returnValue = new JsonResult(_toReturnJsonRaw);
                return true;
            }


            //If we reached until here, the device is NOT allowed
            _toReturnJsonRaw = new JsonObject()
            {
                { "doorResponse" , false},
                { "doorStatus" , ""},
                { "displayText" ,""},
                { "timeStamp",  DateTime.Now}
            };

            returnValue = new JsonResult(_toReturnJsonRaw);
            return false;
        }

        public static bool RegisterHeartBeat(control.Data.controlContext dataContext, string doorId, string token, string timeStamp, ref JsonResult returnValue)
        {
            if (!DateTime.TryParse(timeStamp, out DateTime _timeStamp))
                return false;

            //Request timed out
            if (CheckIfTimedOut(_timeStamp))
                return false;

            Door? _door = GetDoor(dataContext, doorId);
            JsonObject _toReturnJsonRaw;

            //No door with this name found
            if (_door is null)
                return false;

            //Wrong token
            if (_door.Secret != token)
                return false;


            //Update checkin Time
            _door.LastCheckInTime = DateTime.Now;
            dataContext.SaveChanges();

            _toReturnJsonRaw = new JsonObject()
            {
                { "entryMode" , _door.EntryStatus.ToString()},
                { "displayText" , _door.DisplayName},
                { "timeStamp",  DateTime.Now}
            };

            returnValue = new JsonResult(_toReturnJsonRaw);
            return true;
        }

        private static bool CheckForDoorRestrictions(Door? door,string token, DateTime timeStamp)
        {
            //No door with this name found
            if (door is null)
                return false;

            //Check, if token is the same as provided
            if (token != door.Secret)
                return false;

            //This door not registered yet or locked
            if (!door.Registered || door.EntryStatus == EDoorEntryMode.kNoAccess)
                return false;

            //This door is set to accept all entries
            if (door.EntryStatus == EDoorEntryMode.kUniversalAccess) 
                return true;

            if (CheckIfTimedOut(timeStamp))
                return false;

            return true;
        }
        private static bool CheckForUserAccessRestrictions(User? user, Door door)
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

            return !((DateTime.Now - timeStamp) < timeOut);
        }

        private static bool CheckIfExpired(DateTime timeStamp)
        {
            return CheckIfTimedOut(timeStamp, TimeSpan.Zero);
        }
    }
}

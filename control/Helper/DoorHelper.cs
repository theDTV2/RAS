using control.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Signing;
using System.Linq;
using System.Net.Sockets;
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

             if (CheckIfTimedOut(_timeStamp))
                return false;

            string _newSecret = HashHelper.GenerateRandomBase64String(128);

            _door.Secret = _newSecret;
            _door.Registered = true;
            _door.LastCheckInTime = DateTime.Now;

            dataContext.SaveChanges();

            JsonObject _toReturnJsonRaw = new JsonObject()
            {
                { "doorID" , doorID},
                { "secret" , _newSecret},
                { "timeStamp" , DateTime.Now.ToString()},
                { "displayText", _door.DisplayText}

            };

            returnValue = new JsonResult(_toReturnJsonRaw);

            return true;

        }

        public static bool RequestDoorAccess(control.Data.controlContext dataContext, string doorId, string secret, string cardCode, string timeStamp, ref JsonResult returnValue)
        {
            Door? _door = GetDoor(dataContext, doorId);

            //If the door does not exist, it doesnt get a proper access response, only a 500
            if (_door is null)
                return false;

            if (!CheckDoorParameters(_door, secret, timeStamp, out DateTime _timeStamp))
                return false;

            JsonObject _toReturnJsonRaw;
            User? _user = dataContext.User.Where(u => u.SecretCode == cardCode).Include(u => u.AccessDoors).FirstOrDefault();

            _toReturnJsonRaw = new JsonObject()
            {
               { "timeStamp",  DateTime.Now.ToString()},
            };

            if (_user is not null && CheckForDoorRestrictions(_door) && CheckForUserAccessRestrictions(_user, _door!))
            {
                _toReturnJsonRaw["doorResponse"] = true;
                _toReturnJsonRaw["doorStatus"] = _door.EntryStatus.ToString();
                _toReturnJsonRaw["displayText"] = _door.DisplayText;
                _toReturnJsonRaw["responseText"] = "Access granted";

                LogHelper.AddSuccessfulUserEntryToLog(dataContext, _door, _user);

                returnValue = new JsonResult(_toReturnJsonRaw);
                return true;
            }
            EAccessReturnValue _return_val = CheckForUserAccessRestrictionsEAccessReturn(_user, _door);


            _toReturnJsonRaw["doorResponse"] = false;
            _toReturnJsonRaw["doorStatus"] = _door.EntryStatus.ToString();
            _toReturnJsonRaw["displayText"] = _door.DisplayText;
            _toReturnJsonRaw["responseText"] = EnumHelper.ConvertEAccessReturnValueToString(_return_val);
            returnValue = new JsonResult(_toReturnJsonRaw);
            return true;
        }

        public static bool RegisterHeartBeat(control.Data.controlContext dataContext, string doorId, string secret, string timeStamp, ref JsonResult returnValue)
        {
            Door? _door = GetDoor(dataContext, doorId);

            if (_door == null)
                return false;
            if (!CheckDoorParameters(_door, secret, timeStamp, out _))
                return false;

            JsonObject _toReturnJsonRaw;

            //No door with this name found
            if (_door is null)
                return false;

            //Wrong token
            if (_door.Secret != secret)
                return false;


            //Update checkin Time
            _door.LastCheckInTime = DateTime.Now;
            dataContext.SaveChanges();

            _toReturnJsonRaw = new JsonObject()
            {
                { "entryMode" , _door.EntryStatus.ToString()},
                { "displayText" , _door.DisplayText},
                { "timeStamp",  DateTime.Now.ToString()}
            };

            returnValue = new JsonResult(_toReturnJsonRaw);
            return true;
        }

        private static bool CheckForDoorRestrictions(Door? door)
        {
            //No door with this name found
            if (door is null)
                return false;

            //This door not registered yet or locked
            if (!door.Registered || door.EntryStatus == EDoorEntryMode.kNoAccess)
                return false;

            //This door is set to accept all entries
            if (door.EntryStatus == EDoorEntryMode.kUniversalAccess)
                return true;

            return true;
        }
        private static bool CheckForUserAccessRestrictions(User? user, Door door)
        {
            EAccessReturnValue _return_val = CheckForUserAccessRestrictionsEAccessReturn(user, door);

           if (_return_val == EAccessReturnValue.kAccessGranted || _return_val == EAccessReturnValue.kAdminGranted)
                return true;

            return false;
        }
        private static EAccessReturnValue CheckForUserAccessRestrictionsEAccessReturn(User? user, Door door)
        {
            //No user exists with this access code
            if (user is null)
                return EAccessReturnValue.kAccountNotFound;

            //User has not completed registration yet
            if (!user.CompletedRegistration)
                return EAccessReturnValue.kAccountRegistrationNotCompleted;

            //User is locked
            if (user.AccessLevel == EAccessLevel.kNone)
                return EAccessReturnValue.kAccountLocked;

            //User is expired
            if (CheckIfExpired(user.ExpiryDate))
                return EAccessReturnValue.kAccountExpired;

            //Admins are always allowed
            if (user.AccessLevel >= EAccessLevel.kAdmin)
                return EAccessReturnValue.kAdminGranted;

            //User is not allowed to access door
            if (!user.AccessDoors.Contains(door))
                return EAccessReturnValue.kAccessDenied;

            return EAccessReturnValue.kAccessGranted;

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

        private static bool CheckDoorParameters(Door door, string secret, string timeStampStr, out DateTime timeStamp)
        {
            timeStamp = DateTime.MinValue;

            //If the door is not registered, also 500
            if (!door.Registered)
                return false;

            //If secret is not same as provided? 500 time
            if (secret != door.Secret)
                return false;

            //If a faulty time is passed, guess what? 500
            if (!DateTime.TryParse(timeStampStr, out timeStamp))
                return false;

            //if Request timed out
            if (CheckIfTimedOut(timeStamp))
                return false;

            return true;

        }
    }
}

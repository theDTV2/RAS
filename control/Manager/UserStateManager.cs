using control.Manager.Classes;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Session;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Text;
using System.Linq;

namespace control.Manager
{
    public static class UserStateManager
    {
        private static IDictionary<string, UserState> UserStates { get; set; } = new Dictionary<string, UserState>();

        public static bool AddUserState(string sessionid, string userName)
        {
            //If the user previously was logged in, we return true;
            bool _previouslyLoggedIn = false;
            //If we find a session, that has the same username, we invalidate 
            KeyValuePair<string, UserState> _previousState = UserStates.FirstOrDefault(e => e.Value.UserName == userName);
            if (_previousState.Value is not null)
            {
                _previousState.Value.SetInvalid();
                _previouslyLoggedIn = true;
            }
            UserStates.Add(sessionid, new(userName));

            return _previouslyLoggedIn;
        }

        public static bool CheckUserState(string sessionid)
        {
            UserState? _userState;

            if (!UserStates.TryGetValue(sessionid, out _userState))
                return false;

            if (!_userState.Valid)
            {
                RemoveState(sessionid);
                return false;
            }

            _userState.UpdateActionTime();

            return true;
        }

        public static bool RemoveState(string sessionid, string userName = "")
        {
            return UserStates.Remove(sessionid);
        }

        public static async Task DeleteOldStatesAsync()
        {
            while (true)
            {
                /*We can use a rather large delay here to save performance
                Doesnt really matter, if we remove a inactive user after 10 Minutes and 1 second or 10 minutes and 28 seconds, as
                 the user session will invalidate itself after 10 minutes regardless */
                await Task.Delay(550000);

                //Use intermediary variabels to save performance
                DateTime _now = DateTime.Now;
                TimeSpan _timeOut = TimeSpan.FromMinutes(10);

                //TODO: Make this faster, if possible/necessary
                UserStates = UserStates
                    .Where(u => (_now - u.Value.LastAction) < _timeOut)
                    .ToDictionary(u => u.Key, u => u.Value);


            }
        }



    }
}

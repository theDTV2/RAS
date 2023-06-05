using control.Manager.Classes;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Session;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Text;

namespace control.Manager
{
    public static class UserStateManager
    {
        private static IDictionary<string,UserState> UserStates { get; set; } = new Dictionary<string, UserState>();

        public static bool AddUserState(string sessionid, string userName)
        {
            bool _previouslyLoggedIn = false;
            //If we find a session, that has the same username, we invalidate 
            KeyValuePair<string, UserState>? _previousSession = UserStates.FirstOrDefault(e => e.Value.UserName == userName);
            if (_previousSession is not null)
            {
                _previousSession.Value.Value.SetInvalid();
                _previouslyLoggedIn = true;
            }
            UserStates.Add(sessionid, new(userName));

            return _previouslyLoggedIn;
        }

        public static bool CheckUserState(string sessionid, string userName)
        {
            UserState? _session;

            if (!UserStates.TryGetValue(sessionid, out _session))
                return false;

            if (!_session.Valid)
                return false;

            return true;
        }

        public static bool RemoveState(string sessionid, string userName)
        {
            return UserStates.Remove(sessionid);
        }

        public static async Task<int> DeleteOldStatesAsync()
        {
            //TODO: Implement this
            return 5;
        }



    }
}

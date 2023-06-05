using control.Manager.Classes;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Session;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Text;

namespace control.Manager
{
    public static class SessionManager
    {
        private static IDictionary<string,UserSession> UserSessions { get; set; } = new Dictionary<string, UserSession>();

        public static bool AddUserSession(string sessionid, string userName)
        {
            bool _previouslyLoggedIn = false;
            //If we find a session, that has the same username, we invalidate 
            KeyValuePair<string, UserSession>? _previousSession = UserSessions.FirstOrDefault(e => e.Value.UserName == userName);
            if (_previousSession is not null)
            {
                _previousSession.Value.Value.SetInvalid();
                _previouslyLoggedIn = true;
            }
            UserSessions.Add(sessionid, new(userName));

            return _previouslyLoggedIn;
        }

        public static bool CheckUserSession(string sessionid, string userName)
        {
            UserSession? _session;

            if (!UserSessions.TryGetValue(sessionid, out _session))
                return false;

            if (!_session.Valid)
                return false;

            return true;
        }

        public static async Task<int> DeleteOldSessionAsync()
        {
            //TODO: Implement this
            return 5;
        }



    }
}

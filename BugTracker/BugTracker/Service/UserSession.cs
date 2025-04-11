using System;

namespace BugTracker.Service
{
    public class UserSession
    {
        public static String UserId { get; private set; }
        public static string Username { get; private set; }

        private static UserSession _instance;
        private static readonly object _lock = new object();

        public static void Login(String userId, string username)
        {
            UserId = userId;
            Username = username;
        }

        public static UserSession Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new UserSession();
                    return _instance;
                }
            }
        }

        public static void Logout()
        {
            UserId = null;
            Username = null;
        }
    }
}

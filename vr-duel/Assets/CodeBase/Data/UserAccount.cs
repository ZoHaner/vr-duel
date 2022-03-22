using System;

namespace CodeBase.Data
{
    [Serializable]
    public class UserAccount
    {
        public string Id { get; }
        public string Username { get; }

        public UserAccount( string id, string username)
        {
            Username = username;
            Id = id;
        }
    }
}
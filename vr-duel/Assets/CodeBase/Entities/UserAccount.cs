using System;

namespace CodeBase.Entities
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
using Nakama;

namespace CodeBase.Entities
{
    public class RemotePlayer
    {
        public readonly IUserPresence Presence;

        public RemotePlayer(IUserPresence presence)
        {
            Presence = presence;
        }
    }
}
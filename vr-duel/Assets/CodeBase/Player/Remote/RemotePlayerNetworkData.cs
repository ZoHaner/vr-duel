using Nakama;

namespace CodeBase.Player.Remote
{
    public class RemotePlayerNetworkData
    {
        public IUserPresence User;

        public RemotePlayerNetworkData(IUserPresence user)
        {
            User = user;
        }
    }
}
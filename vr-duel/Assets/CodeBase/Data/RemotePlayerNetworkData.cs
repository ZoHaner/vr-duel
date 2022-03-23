using Nakama;

namespace CodeBase.Data
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
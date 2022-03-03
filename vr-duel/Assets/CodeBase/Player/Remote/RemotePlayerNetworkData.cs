using Nakama;

namespace CodeBase.Player.Remote
{
    public class RemotePlayerNetworkData
    {
        public string MatchId;
        public IUserPresence User;

        public RemotePlayerNetworkData(string matchId, IUserPresence user)
        {
            MatchId = matchId;
            User = user;
        }
    }
}
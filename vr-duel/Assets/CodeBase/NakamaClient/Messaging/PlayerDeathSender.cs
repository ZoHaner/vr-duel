using System.Threading.Tasks;
using CodeBase.NakamaClient.Utilities;

namespace CodeBase.NakamaClient.Messaging
{
    public class NetworkMessaging : INetworkMessaging
    {
        private readonly ConnectionService _connectionService;

        public NetworkMessaging(ConnectionService connectionService)
        {
            _connectionService = connectionService;
        }
        
        public async Task SendDeathState(string matchId, string userId)
        {
            await _connectionService.Socket.SendMatchStateAsync(matchId, OpCodes.Died, MatchDataJson.Died(userId));
        }

    }
}
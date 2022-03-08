using System.Collections.Generic;
using CodeBase.Services;
using Nakama;

namespace CodeBase.Infrastructure.Factory
{
    public interface INetworkPlayerFactory : IService
    {
        void SpawnPlayers(IEnumerable<IUserPresence> users, IRoundService roundService);
        void RemovePlayers(IEnumerable<IUserPresence> userPresences);
        void UpdatePlayersState(IMatchState state);
        int PlayersCount { get; }
        string LocalUserSessionId { get; set; }
        void RemovePlayer(string sessionId);
        void RemoveAllPlayers();
    }
}
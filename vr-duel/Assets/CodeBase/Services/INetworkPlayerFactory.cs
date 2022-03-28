using System.Collections.Generic;
using Nakama;

namespace CodeBase.Services
{
    public interface INetworkPlayerFactory : IService
    {
        void SpawnPlayers(IEnumerable<IUserPresence> users, IRoundService roundService);
        void RemovePlayers(IEnumerable<IUserPresence> userPresences);
        void UpdatePlayersState(IMatchState state);
        int PlayersCount { get; }
        string LocalUserId { get; set; }
        void RemovePlayer(string userId);
        void RemoveAllPlayers();
        void DeactivatePlayer(string userId);
        void Initialize();
        void CleanUp();
    }
}
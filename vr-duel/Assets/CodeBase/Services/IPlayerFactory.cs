using Nakama;
using UnityEngine;

namespace CodeBase.Services
{
    public interface IPlayerFactory : IService
    {
        GameObject SpawnLobbyPlayer();
        GameObject SpawnStaticLocalPlayer();
        GameObject SpawnLocalNetworkPlayer(string userId);
        GameObject SpawnRemoteNetworkPlayer(IUserPresence presence, IRoundService roundService);
        void Initialize();
        void CleanUp();
    }
}
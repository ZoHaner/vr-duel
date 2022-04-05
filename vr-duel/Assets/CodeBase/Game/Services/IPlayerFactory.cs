using CodeBase.ServiceLocator;
using Nakama;
using UnityEngine;

namespace CodeBase.Services
{
    public interface IPlayerFactory : IService
    {
        GameObject SpawnMovingLocalPlayer();
        GameObject SpawnStaticLocalPlayer();
        GameObject SpawnLocalNetworkPlayer(string userId);
        GameObject SpawnRemoteNetworkPlayer(IUserPresence presence, IRoundService roundService);
        void Initialize();
        void CleanUp();
    }
}
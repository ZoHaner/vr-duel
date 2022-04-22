using System.Threading.Tasks;
using Nakama;
using UnityEngine;

namespace CodeBase.Services
{
    public interface IPlayerFactory : IService
    {
        Task<GameObject> SpawnLobbyPlayer();
        Task<GameObject> SpawnStaticLocalPlayer();
        Task<GameObject> SpawnLocalNetworkPlayer(string userId);
        Task<GameObject> SpawnRemoteNetworkPlayer(IUserPresence presence, IRoundService roundService);
        void Initialize();
        void CleanUp();
    }
}
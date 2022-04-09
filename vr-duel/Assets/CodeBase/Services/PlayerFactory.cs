using CodeBase.Behaviours.Gun;
using CodeBase.Behaviours.Player;
using CodeBase.Behaviours.Player.Remote;
using CodeBase.Entities;
using CodeBase.Services.Input;
using CodeBase.Services.Input.Standalone;
using CodeBase.StaticData;
using CodeBase.Utilities;
using CodeBase.Utilities.Spawn;
using Nakama;
using UnityEngine;

namespace CodeBase.Services
{
    public abstract class PlayerFactory : IPlayerFactory
    {
        private readonly INetworkService _networkService;
        private readonly IInputService _inputService;
        private InitialPointHolder _pointHolder;

        protected PlayerFactory(IInputService inputService, INetworkService networkService)
        {
            _inputService = inputService;
            _networkService = networkService;
        }
        
        public void Initialize()
        {
            _pointHolder = new InitialPointHolder();
        }
        
        public void CleanUp()
        {
            _pointHolder.CleanPoints();
        }

        public abstract GameObject SpawnLobbyPlayer();
        public abstract GameObject SpawnStaticLocalPlayer();
        public abstract GameObject SpawnLocalNetworkPlayer(string userId);

        protected GameObject SpawnMovingLocalPlayerBase(string prefabPath)
        {
            var player = ResourcesUtilities.Instantiate(prefabPath);
            player.GetComponent<PlayerMovement>().Construct(_inputService);
            return player;  
        }

        protected GameObject SpawnStaticLocalPlayerBase(string prefabPath)
        {
            var player = ResourcesUtilities.Instantiate(prefabPath);
            return player;
        }
        
        protected GameObject SpawnLocalNetworkPlayerBase(string prefabPath)
        {
            var initialPoint = _pointHolder.GetInitialPoint();
            var player = ResourcesUtilities.Instantiate(prefabPath, initialPoint);
            player.transform.LookAt(Vector3.zero);

            player.GetComponent<PlayerStateSender>().Construct(_networkService, _inputService);
            player.GetComponent<PlayerMovement>().Construct(_inputService);
            CreateLocalPlayerGun(player);

            return player;
        }

        public GameObject SpawnRemoteNetworkPlayer(IUserPresence presence, IRoundService roundService)
        {
            var initialPoint = _pointHolder.GetInitialPoint();
            var player = ResourcesUtilities.Instantiate(AssetsPath.RemoteNetworkPlayer, initialPoint);
            player.transform.LookAt(Vector3.zero);
            GunShooting gunMono = CreateGun(player);

            var networkData = new RemotePlayer(presence);
            player.GetComponent<RemotePlayerSync>().Construct(networkData, gunMono);
            player.GetComponent<RemotePlayerSender>().Construct(roundService, networkData);
            
            return player;
        }

        private void CreateLocalPlayerGun(GameObject player)
        {
            GunShooting gunMono = CreateGun(player);
            gunMono.Construct(_inputService);
        }

        private GunShooting CreateGun(GameObject player)
        {
            var gunPivot = player.GetComponentInChildren<GunPivot>().transform;
            var gunObject = ResourcesUtilities.Instantiate(AssetsPath.Revolver, gunPivot);
            var gunMono = gunObject.GetComponent<GunShooting>();
            return gunMono;
        }
    }
}
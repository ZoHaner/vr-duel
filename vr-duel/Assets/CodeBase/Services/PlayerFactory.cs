using System.Threading.Tasks;
using CodeBase.Behaviours.Gun;
using CodeBase.Behaviours.Player;
using CodeBase.Behaviours.Player.Remote;
using CodeBase.Entities;
using CodeBase.Services.Input;
using CodeBase.Services.Input.Standalone;
using CodeBase.StaticData;
using CodeBase.Utilities.Spawn;
using Nakama;
using UnityEngine;

namespace CodeBase.Services
{
    public abstract class PlayerFactory : IPlayerFactory
    {
        private readonly INetworkService _networkService;
        private readonly IInputService _inputService;
        private readonly IAssetProvider _assetProvider;
        private InitialPointHolder _pointHolder;

        protected PlayerFactory(IInputService inputService, INetworkService networkService, IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
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

        public abstract Task<GameObject> SpawnLobbyPlayer();
        public abstract Task<GameObject> SpawnStaticLocalPlayer();
        public abstract Task<GameObject> SpawnLocalNetworkPlayer(string userId);

        protected async Task<GameObject> SpawnMovingLocalPlayerBase(string prefabAddress)
        {
            var prefab = await _assetProvider.Load<GameObject>(prefabAddress);
            var player = Object.Instantiate(prefab);
            player.GetComponent<PlayerMovement>().Construct(_inputService);
            return player;  
        }

        protected async Task<GameObject> SpawnStaticLocalPlayerBase(string prefabAddress)
        {
            var prefab = await _assetProvider.Load<GameObject>(prefabAddress);
            var player = Object.Instantiate(prefab);
            return player;
        }
        
        protected async Task<GameObject> SpawnLocalNetworkPlayerBase(string prefabAddress)
        {
            var initialPoint = _pointHolder.GetInitialPoint();
            var prefab = await _assetProvider.Load<GameObject>(prefabAddress);
            var player = Object.Instantiate(prefab, initialPoint, Quaternion.identity);
            player.transform.LookAt(Vector3.zero);

            player.GetComponent<PlayerStateSender>().Construct(_networkService, _inputService);
            player.GetComponent<PlayerMovement>().Construct(_inputService);
            CreateLocalPlayerGun(player);

            return player;
        }

        public async Task<GameObject> SpawnRemoteNetworkPlayer(IUserPresence presence, IRoundService roundService)
        {
            var initialPoint = _pointHolder.GetInitialPoint();
            var prefab = await _assetProvider.Load<GameObject>(AssetAddresses.RemoteNetworkPlayer);
            var player = Object.Instantiate(prefab, initialPoint, Quaternion.identity);
            
            player.transform.LookAt(Vector3.zero);
            GunShooting gunMono = await CreateGun(player);

            var networkData = new RemotePlayer(presence);
            player.GetComponent<RemotePlayerSync>().Construct(networkData, gunMono);
            player.GetComponent<RemotePlayerSender>().Construct(roundService, networkData);
            
            return player;
        }

        private async Task CreateLocalPlayerGun(GameObject player)
        {
            GunShooting gunMono = await CreateGun(player);
            gunMono.Construct(_inputService);
        }

        private async Task<GunShooting> CreateGun(GameObject player)
        {
            var gunPivot = player.GetComponentInChildren<GunPivot>().transform;
            var prefab = await _assetProvider.Load<GameObject>(AssetAddresses.Revolver);
            var gunObject = Object.Instantiate(prefab, gunPivot);
            var gunMono = gunObject.GetComponent<GunShooting>();
            return gunMono;
        }
    }
}
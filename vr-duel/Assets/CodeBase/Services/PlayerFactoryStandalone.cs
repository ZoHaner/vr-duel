using System.Threading.Tasks;
using CodeBase.Services.Input;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Services
{
    public class PlayerFactoryStandalone : PlayerFactory
    {
        private readonly Vector3 _lobbyPlayerPosition = new Vector3(-1.27f, 0.61f, -2.84f);

        public PlayerFactoryStandalone(IInputService inputService, INetworkService networkService, IAssetProvider assetProvider) : base(inputService, networkService, assetProvider)
        {
        }

        public override async Task<GameObject> SpawnLobbyPlayer()
        {
            var player = await SpawnMovingLocalPlayerBase(AssetAddresses.LocalPlayerStandalone);
            player.transform.position = _lobbyPlayerPosition;
            return player;
        }

        public override async Task<GameObject> SpawnStaticLocalPlayer()
        {
            return await SpawnStaticLocalPlayerBase(AssetAddresses.LocalStaticPlayerStandalone);
        }

        public override async Task<GameObject> SpawnLocalNetworkPlayer(string userId)
        {
            return await SpawnLocalNetworkPlayerBase(AssetAddresses.LocalNetworkPlayerStandalone);
        }
    }
}
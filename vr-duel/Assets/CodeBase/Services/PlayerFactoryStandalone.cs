using CodeBase.Services.Input;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Services
{
    public class PlayerFactoryStandalone : PlayerFactory
    {
        private readonly Vector3 _lobbyPlayerPosition = new Vector3(-1.27f, 0.61f, -2.84f);

        public PlayerFactoryStandalone(IInputService inputService, INetworkService networkService) : base(inputService, networkService)
        {
        }

        public override GameObject SpawnLobbyPlayer()
        {
            var player = SpawnMovingLocalPlayerBase(AssetsPath.LocalPlayerStandalone);
            player.transform.position = _lobbyPlayerPosition;
            return player;
        }

        public override GameObject SpawnStaticLocalPlayer()
        {
            return SpawnStaticLocalPlayerBase(AssetsPath.LocalStaticPlayerStandalone);
        }

        public override GameObject SpawnLocalNetworkPlayer(string userId)
        {
            return SpawnLocalNetworkPlayerBase(AssetsPath.LocalNetworkPlayerStandalone);
        }
    }
}
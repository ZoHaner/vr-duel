using CodeBase.Services.Input;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Services
{
    public class PlayerFactoryXR : PlayerFactory
    {
        private readonly Vector3 _lobbyPlayerPosition = new Vector3(-1.27f, 0f, -2.84f);

        public PlayerFactoryXR(IInputService inputService, INetworkService networkService) : base(inputService, networkService)
        {
        }

        public override GameObject SpawnLobbyPlayer()
        {
            var player = SpawnMovingLocalPlayerBase(AssetsPath.LocalPlayerXR);
            player.transform.position = _lobbyPlayerPosition;
            return player;
        }

        public override GameObject SpawnStaticLocalPlayer()
        {
            return SpawnStaticLocalPlayerBase(AssetsPath.LocalStaticPlayerXR);
        }

        public override GameObject SpawnLocalNetworkPlayer(string userId)
        {
            return SpawnLocalNetworkPlayerBase(AssetsPath.LocalNetworkPlayerXR);
        }
    }
}
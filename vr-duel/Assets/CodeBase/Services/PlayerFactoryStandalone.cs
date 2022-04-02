using CodeBase.Services.Input;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Services
{
    public class PlayerFactoryStandalone : PlayerFactory
    {
        public PlayerFactoryStandalone(IInputService inputService, INetworkService networkService) : base(inputService, networkService)
        {
        }

        public override GameObject SpawnMovingLocalPlayer()
        {
            return SpawnMovingLocalPlayerBase(AssetsPath.LocalPlayerStandalone);
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
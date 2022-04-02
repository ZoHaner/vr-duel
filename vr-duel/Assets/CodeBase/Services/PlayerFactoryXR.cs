using CodeBase.Services.Input;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Services
{
    public class PlayerFactoryXR : PlayerFactory
    {
        public PlayerFactoryXR(IInputService inputService, INetworkService networkService) : base(inputService, networkService)
        {
        }

        public override GameObject SpawnMovingLocalPlayer()
        {
            return SpawnMovingLocalPlayerBase(AssetsPath.LocalPlayerXR);
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
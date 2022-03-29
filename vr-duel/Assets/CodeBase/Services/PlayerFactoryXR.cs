using CodeBase.Services.Input;
using CodeBase.Services.Input.Standalone;
using CodeBase.StaticData;
using CodeBase.Utilities;
using UnityEngine;

namespace CodeBase.Services
{
    public class PlayerFactoryXR : PlayerFactory
    {
        public PlayerFactoryXR(IInputService inputService) : base(inputService)
        {
        }

        public override GameObject SpawnMovingLocalPlayer()
        {
            var player = ResourcesUtilities.Instantiate(AssetsPath.LocalPlayerXR);
            player.GetComponent<PlayerMovement>().Construct(InputService);
            return player;  
        }

        public override GameObject SpawnStaticLocalPlayer()
        {
            var player = ResourcesUtilities.Instantiate(AssetsPath.LocalStaticPlayerXR);
            return player;
        }
    }
}
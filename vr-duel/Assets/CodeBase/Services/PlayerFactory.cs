using CodeBase.Services.Input;
using CodeBase.Services.Input.Standalone;
using CodeBase.StaticData;
using CodeBase.Utilities;
using UnityEngine;

namespace CodeBase.Services
{
    public class PlayerFactory : IPlayerFactory
    {
        private IInputService _inputService;

        public PlayerFactory(IInputService inputService)
        {
            _inputService = inputService;
        }
        
        public GameObject SpawnLocalPlayer()
        {
            var player = ResourcesUtilities.Instantiate(AssetsPath.LocalPlayer);
            player.GetComponent<PlayerMovement>().Construct(_inputService);
            return player;
        }
    }
}
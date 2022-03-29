using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Services
{
    public abstract class PlayerFactory : IPlayerFactory
    {
        protected readonly IInputService InputService;

        protected PlayerFactory(IInputService inputService)
        {
            InputService = inputService;
        }

        public abstract GameObject SpawnMovingLocalPlayer();

        public abstract GameObject SpawnStaticLocalPlayer();
    }
}
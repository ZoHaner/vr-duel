using CodeBase.Infrastructure;
using CodeBase.ServiceLocator;
using CodeBase.Services.Singletons;
using CodeBase.States;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        public LoadingCurtain Curtain;
        
        private Game _game;

        private void Awake()
        {
            _game = new Game(this, Curtain, MainThreadDispatcher.Instance, UpdateProvider.Instance, AllServices.Container);
            _game.StateMachine.Enter<BootstrapState>();
            
            DontDestroyOnLoad(this);
        }

        private void OnDestroy()
        {
            _game.StateMachine.Enter<CleanupState>();
        }
    }
}

using CodeBase.Infrastructure.StateMachine.States;
using CodeBase.Logic;
using CodeBase.Network;
using CodeBase.Services.ServiceLocator;
using Nakama;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        public LoadingCurtain Curtain;
        
        private Game _game;

        private void Awake()
        {
            _game = new Game(this, Curtain, MainThreadDispatcher.Instance(), UnityWebRequestAdapter.Instance, AllServices.Container);
            _game.StateMachine.Enter<BootstrapState>();
            
            DontDestroyOnLoad(this);
        }
    }
}

using CodeBase.Infrastructure.Utilities;
using CodeBase.Network;
using CodeBase.Services.Network;
using CodeBase.Services.ServiceLocator;
using Nakama;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _allServices;
        private readonly UnityWebRequestAdapter _unityWebRequestAdapter;
        private readonly MainThreadDispatcher _mainThreadDispatcher;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices allServices, UnityWebRequestAdapter unityWebRequestAdapter, MainThreadDispatcher mainThreadDispatcher)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _allServices = allServices;
            _unityWebRequestAdapter = unityWebRequestAdapter;
            _mainThreadDispatcher = mainThreadDispatcher;
            
            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(AssetsPath.InitialSceneName, onLoaded: EnterLoadLevel);
        }

        private void RegisterServices()
        {
            var networkService = new NetworkService(_unityWebRequestAdapter, _mainThreadDispatcher);
            _allServices.Register<INetworkService>(networkService);
        }

        private void EnterLoadLevel() => 
            _stateMachine.Enter<LoadLobbyLevelState, string>(AssetsPath.LobbySceneName);

        public void Exit()
        {
        }
    }
}
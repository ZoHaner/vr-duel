using CodeBase.Infrastructure.Utilities;
using CodeBase.Network;
using CodeBase.Services.Network;
using CodeBase.Services.ServiceLocator;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
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
            RegisterNetworkService();
            RegisterStaticDataService();

            _allServices.Register<IUIFactory>(new UIFactory(_allServices.Single<IStaticDataService>(), _allServices.Single<INetworkService>()));
            _allServices.Register<IWindowService>(new WindowService(_allServices.Single<IUIFactory>()));
            
            RegisterSessionService();
        }

        private void RegisterSessionService()
        {
            var sessionService = new SessionService();
            sessionService.Construct(
                _allServices.Single<INetworkService>(),
                _mainThreadDispatcher);
            _allServices.Register<ISessionService>(sessionService);
        }

        private void RegisterStaticDataService()
        {
            var staticData = new StaticDataService();
            staticData.LoadStatics();
            _allServices.Register<IStaticDataService>(staticData);
        }

        private void RegisterNetworkService()
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
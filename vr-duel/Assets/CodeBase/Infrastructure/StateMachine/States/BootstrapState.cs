using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Utilities;
using CodeBase.Network;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.Input.Standalone;
using CodeBase.Services.Input.VR;
using CodeBase.Services.Network;
using CodeBase.Services.Progress;
using CodeBase.Services.ServiceLocator;
using CodeBase.Services.StaticData;
using CodeBase.Services.UpdateProvider;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
using Nakama;
using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _allServices;
        private readonly UnityWebRequestAdapter _unityWebRequestAdapter;
        private readonly MainThreadDispatcher _mainThreadDispatcher;
        private readonly UpdateProvider _updateProvider;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices allServices, UnityWebRequestAdapter unityWebRequestAdapter, MainThreadDispatcher mainThreadDispatcher, UpdateProvider updateProvider)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _allServices = allServices;
            _unityWebRequestAdapter = unityWebRequestAdapter;
            _mainThreadDispatcher = mainThreadDispatcher;
            _updateProvider = updateProvider;

            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(AssetsPath.InitialSceneName, onLoaded: EnterLoadLevel);
        }

        public void Exit()
        {
        }

        private void EnterLoadLevel() => 
            _stateMachine.Enter<LoadLobbyLevelState, string>(AssetsPath.LobbySceneName);

        private void RegisterServices()
        {
            RegisterInputService();
            
            _allServices.Register<IPlayerDataService>(new PlayerDataService());
            
            RegisterNetworkService();
            RegisterStaticDataService();

            _allServices.Register<INameSelectorService>(new NameSelectorService());
            _allServices.Register<IProgressService>(new ProgressService());
            _allServices.Register<ISaveLoadService>(new SaveLoadService());
            _allServices.Register<IUIFactory>(new UIFactory(_allServices.Single<IStaticDataService>(), _allServices.Single<INetworkService>()));
            _allServices.Register<IWindowService>(new WindowService(_allServices.Single<IUIFactory>()));
            _allServices.Register<INetworkPlayerFactory>(new NetworkPlayerFactory(_allServices.Single<INetworkService>(), _allServices.Single<IInputEventService>()));
            _allServices.Register<IRoundService>(new RoundService(_allServices.Single<INetworkService>(), _allServices.Single<INetworkPlayerFactory>(),_allServices.Single<IProgressService>(), _allServices.Single<IPlayerDataService>()));
        }

        private void RegisterInputService()
        {
            if (Application.isMobilePlatform)
            {
                _allServices.Register<IInputEventService>(new VRInputEventService());
            }
            else
            {
                _allServices.Register<IInputEventService>(new StandaloneInputEventService(_updateProvider));
            }
        }

        private void RegisterNetworkService()
        {
            var networkService = new NetworkService(_unityWebRequestAdapter, _mainThreadDispatcher, _allServices.Single<IPlayerDataService>());
            _allServices.Register<INetworkService>(networkService);
        }

        private void RegisterStaticDataService()
        {
            var staticData = new StaticDataService();
            staticData.LoadStatics();
            _allServices.Register<IStaticDataService>(staticData);
        }
    }
}
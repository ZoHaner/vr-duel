using CodeBase.Infrastructure.StateMachine;
using CodeBase.Services;
using CodeBase.Services.Editor;
using CodeBase.Services.Input;
using CodeBase.Services.Input.Standalone;
using CodeBase.Services.Input.VR;
using CodeBase.Services.Progress;
using CodeBase.Services.ServiceLocator;
using CodeBase.Services.Singletons;
using CodeBase.Services.UI;
using CodeBase.StaticData;
using CodeBase.Utilities;
using Nakama;
using UnityEngine;

namespace CodeBase.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _allServices;
        private readonly UnityWebRequestAdapter _unityWebRequestAdapter;
        private readonly MainThreadDispatcher _mainThreadDispatcher;
        private readonly IUpdateProvider _updateProvider;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices allServices, UnityWebRequestAdapter unityWebRequestAdapter, MainThreadDispatcher mainThreadDispatcher, IUpdateProvider updateProvider)
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
            _stateMachine.Enter<ChoosingNameState>();

        private void RegisterServices()
        {
            RegisterInputService();
            RegisterApplicationCloseService();
            _allServices.Register<IUpdateProvider>(_updateProvider);

            _allServices.Register<IStorageService>(new StorageService());
            _allServices.Register<IPlayerDataService>(new PlayerDataService());
            _allServices.Register<IAssetProvider>(new AssetProvider());
            RegisterNetworkService();
            RegisterStaticDataService();

            _allServices.Register<IPlayerAccountsService>(new PlayerAccountsService(_allServices.Single<IStorageService>()));
            _allServices.Register<INameSelectorService>(new NameSelectorService());
            _allServices.Register<ISaveLoadProgressService>(new SaveLoadProgressService(_allServices.Single<IStorageService>()));
            _allServices.Register<IProgressService>(new ProgressService());

            _allServices.Register<IUIFactory>(new UIFactory(_allServices.Single<IStaticDataService>(), _allServices.Single<INetworkService>(), _allServices.Single<INameSelectorService>(), _allServices.Single<IPlayerAccountsService>()));
            _allServices.Register<IGameUIFactory>(new GameUIFactory(_allServices.Single<IStaticDataService>(), _allServices.Single<IProgressService>()));
            _allServices.Register<ILobbyUIFactory>(new LobbyUIFactory(_allServices.Single<IStaticDataService>(), _allServices.Single<INetworkService>(), _allServices.Single<IPlayerDataService>(), _allServices.Single<IProgressService>(), _allServices.Single<ICloseApplicationService>()));

            _allServices.Register<IWindowService>(new WindowService(_allServices.Single<IUIFactory>(), _allServices.Single<IGameUIFactory>()));

            _allServices.Register<IGameMenuService>(new GameMenuService(_allServices.Single<IInputService>(), _allServices.Single<IWindowService>(), _allServices.Single<IUpdateProvider>()));
            RegisterPlayerFactory();
            _allServices.Register<IRoundService>(new RoundService(_allServices.Single<INetworkService>(), _allServices.Single<IPlayerFactory>(), _allServices.Single<IProgressService>(), _allServices.Single<IPlayerDataService>(), _allServices.Single<IWindowService>()));
            _allServices.Register<ILobbyConfigureService>(new LobbyConfigureService(_allServices.Single<IPlayerFactory>(), _allServices.Single<ILobbyUIFactory>()));
        }

        private void RegisterApplicationCloseService()
        {
#if UNITY_EDITOR
            _allServices.Register<ICloseApplicationService>(new EditorCloseApplicationService());
#else
            _allServices.Register<ICloseApplicationService>(new StandaloneCloseApplicationService());
#endif
        }

        private void RegisterInputService()
        {
            if (Application.isMobilePlatform)
            {
                var xrInputSystem = new XRInputUpdater(_updateProvider);
                xrInputSystem.Initialize();
                var vrInputService = new VRInputService(xrInputSystem);
                vrInputService.Initialize();
                _allServices.Register<IInputService>(vrInputService);
            }
            else
                _allServices.Register<IInputService>(new StandaloneInputService());
        }

        private void RegisterPlayerFactory()
        {
            if (Application.isMobilePlatform)
                _allServices.Register<IPlayerFactory>(new PlayerFactoryXR(_allServices.Single<IInputService>(), _allServices.Single<INetworkService>()));
            else
                _allServices.Register<IPlayerFactory>(new PlayerFactoryStandalone(_allServices.Single<IInputService>(), _allServices.Single<INetworkService>()));
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
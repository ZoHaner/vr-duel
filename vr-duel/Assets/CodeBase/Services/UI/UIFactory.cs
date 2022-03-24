using CodeBase.UI.Windows;
using CodeBase.Utilities;
using UnityEngine;

namespace CodeBase.Services.UI
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPath = "UI/UIRoot";
        private Transform _uiRoot;

        private readonly IStaticDataService _staticData;
        private readonly INetworkService _networkService;
        private readonly INameSelectorService _nameSelectorService;
        private readonly IPlayerAccountsService _playersAccountService;

        public UIFactory(IStaticDataService staticData, INetworkService networkService, INameSelectorService nameSelectorService, IPlayerAccountsService playersAccountService)
        {
            _staticData = staticData;
            _networkService = networkService;
            _nameSelectorService = nameSelectorService;
            _playersAccountService = playersAccountService;
        }

        public void CreateRootIfNotExist()
        {
            if (_uiRoot == null)
                CreateUIRoot();
        }

        public GameObject CreateMatchesListWindow()
        {
            CreateRootIfNotExist();

            var config = _staticData.ForWindow(WindowId.MatchesList);
            var configPrefab = config.Prefab;
            var window = InstantiateWindow(configPrefab).GetComponent<MatchListWindow>();
            window.Construct(_networkService);
            return window.gameObject;
        }

        public GameObject CreateMatchmakingWindow()
        {
            CreateRootIfNotExist();

            var config = _staticData.ForWindow(WindowId.Matchmaking);
            var matchmakingWindow = InstantiateWindow(config.Prefab).GetComponent<MatchmakingWindow>();
            matchmakingWindow.Construct(_networkService);
            return matchmakingWindow.gameObject;
        }

        public GameObject CreateChoosePlayerNameWindow(IWindowService windowService)
        {
            CreateRootIfNotExist();

            var config = _staticData.ForWindow(WindowId.ChoosePlayerName);
            var chooseNameWindow = InstantiateWindow(config.Prefab).GetComponent<ChooseNameWindow>();
            chooseNameWindow.Construct(_nameSelectorService, _playersAccountService, windowService);
            return chooseNameWindow.gameObject;
        }

        public GameObject CreateGeneratePlayerNameWindow(IWindowService windowService)
        {
            CreateRootIfNotExist();

            var config = _staticData.ForWindow(WindowId.GeneratePlayerName);
            var generateNameWindow = InstantiateWindow(config.Prefab).GetComponent<GenerateNameWindow>();
            generateNameWindow.Construct(_nameSelectorService, windowService);

            return generateNameWindow.gameObject;
        }

        private WindowBase InstantiateWindow(WindowBase configPrefab)
        {
            var window = Object.Instantiate(configPrefab, _uiRoot);
            window.gameObject.transform.localPosition = Vector3.zero;
            return window;
        }

        private void CreateUIRoot()
        {
            _uiRoot = ResourcesUtilities.Instantiate(UIRootPath).transform;
            _uiRoot.position += Vector3.up;
        }
    }
}
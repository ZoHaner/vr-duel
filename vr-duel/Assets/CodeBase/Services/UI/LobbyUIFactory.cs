using CodeBase.Services.Progress;
using CodeBase.UI.Windows;
using CodeBase.Utilities;
using UnityEngine;

namespace CodeBase.Services.UI
{
    public class LobbyUIFactory : ILobbyUIFactory
    {
        private const string UIRootPath = "UI/Lobby/UIRoot";

        private Transform _uiRoot;
        private readonly IStaticDataService _staticData;
        private readonly INetworkService _networkService;
        private readonly IPlayerDataService _playerDataService;
        private readonly IProgressService _progressService;
        private readonly ICloseApplicationService _closeApplicationService;

        public LobbyUIFactory(IStaticDataService staticData, INetworkService networkService, IPlayerDataService playerDataService, IProgressService progressService, ICloseApplicationService closeApplicationService)
        {
            _staticData = staticData;
            _networkService = networkService;
            _playerDataService = playerDataService;
            _progressService = progressService;
            _closeApplicationService = closeApplicationService;
        }

        public GameObject CreateLobbyWindow()
        {
            CreateRootIfNotExist();

            var config = _staticData.ForWindow(WindowId.LobbyWindow);
            var configPrefab = config.Prefab;
            var window = InstantiateWindow(configPrefab).GetComponent<LobbyWindow>();
            window.Construct(_playerDataService, _progressService, _networkService, _closeApplicationService);
            return window.gameObject;
        }

        public void CreateRootIfNotExist()
        {
            if (_uiRoot == null)
                CreateUIRoot();
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
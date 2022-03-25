using System;
using CodeBase.Services.Progress;
using CodeBase.Services.UI;
using CodeBase.UI.Windows;
using CodeBase.Utilities;
using UnityEngine;

namespace CodeBase.Services
{
    public class GameUIFactory : IGameUIFactory
    {
        private const string UIRootPath = "UI/GameLoop/UIRoot";
        private readonly Vector3 _rootPosition = Vector3.up * 2;
        
        private readonly IStaticDataService _staticData;
        private readonly IProgressService _progressService;
        private Transform _uiRoot;
        private Action _backToLobby;

        public GameUIFactory(IStaticDataService staticData, IProgressService progressService)
        {
            _staticData = staticData;
            _progressService = progressService;
        }

        public void CreateRootIfNotExist()
        {
            if (_uiRoot == null)
                CreateUIRoot();
        }

        public GameObject CreateWinnerPopup()
        {
            CreateRootIfNotExist();

            var config = _staticData.ForWindow(WindowId.WinnerPopup);
            var window = InstantiateWindow(config.Prefab).GetComponent<WinPopup>();
            window.Construct(_progressService);
            return window.gameObject;
        }

        public GameObject ShowLoosePopup()
        {
            CreateRootIfNotExist();

            var config = _staticData.ForWindow(WindowId.LoosePopup);
            var window = InstantiateWindow(config.Prefab);
            return window.gameObject;
        }

        public GameObject CreateBackToLobbyWindow(WindowService windowService)
        {
            CreateRootIfNotExist();

            var config = _staticData.ForWindow(WindowId.BackToLobby);
            var window = InstantiateWindow(config.Prefab).GetComponent<BackToLobbyWindow>();
            window.Construct(
                windowService.CloseAllWindows, 
                () => _backToLobby());
            return window.gameObject;
        }

        public void SetExitCallback(Action backToLobby)
        {
            _backToLobby = backToLobby;
        }

        public void ClearExitCallback()
        {
            _backToLobby = null;
        }

        private void CreateUIRoot()
        {
            _uiRoot = ResourcesUtilities.Instantiate(UIRootPath).transform;
            _uiRoot.position = _rootPosition;
        }

        private WindowBase InstantiateWindow(WindowBase configPrefab)
        {
            var window = UnityEngine.Object.Instantiate(configPrefab, _uiRoot);
            window.gameObject.transform.localPosition = Vector3.zero;
            return window;
        }
    }
}
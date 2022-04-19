using System;
using System.Threading.Tasks;
using CodeBase.Services.Progress;
using CodeBase.UI.Windows;
using CodeBase.Utilities;
using UnityEngine;

namespace CodeBase.Services.UI
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

        public async Task<GameObject> CreateWinnerPopup()
        {
            CreateRootIfNotExist();

            var config = _staticData.ForWindow(WindowId.WinnerPopup);
            var prefab = await config.PrefabReference.LoadAssetAsync().Task;
            var window = InstantiateWindow(prefab).GetComponent<WinPopup>();
            window.Construct(_progressService);
            return window.gameObject;
        }

        public async Task<GameObject> ShowLoosePopup()
        {
            CreateRootIfNotExist();

            var config = _staticData.ForWindow(WindowId.LoosePopup);
            var prefab = await config.PrefabReference.LoadAssetAsync().Task;
            var window = InstantiateWindow(prefab);
            return window.gameObject;
        }

        public async Task<GameObject> CreateBackToLobbyWindow(WindowService windowService)
        {
            CreateRootIfNotExist();

            var config = _staticData.ForWindow(WindowId.BackToLobby);
            var prefab = await config.PrefabReference.LoadAssetAsync().Task;
            var window = InstantiateWindow(prefab).GetComponent<BackToLobbyWindow>();
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

        private GameObject InstantiateWindow(GameObject configPrefab)
        {
            var window = UnityEngine.Object.Instantiate(configPrefab, _uiRoot);
            window.transform.localPosition = Vector3.zero;
            return window;
        }
    }
}
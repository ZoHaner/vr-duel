using System;
using System.Threading.Tasks;
using CodeBase.Services.Progress;
using CodeBase.UI.Windows;
using CodeBase.Utilities;
using UnityEngine;

namespace CodeBase.Services.UI
{
    public class GameUIFactory : UIBaseFactory, IGameUIFactory
    {
        protected override string UIRootPrefabPath => "UI/GameLoop/UIRoot";
        private readonly Vector3 _rootPosition = Vector3.up * 2;

        private readonly IStaticDataService _staticData;
        private readonly IProgressService _progressService;
        private Action _backToLobby;

        public GameUIFactory(IStaticDataService staticData, IProgressService progressService, IAssetProvider assetProvider) : base(assetProvider)
        {
            _staticData = staticData;
            _progressService = progressService;
        }

        public async Task<GameObject> CreateWinnerPopup()
        {
            await CreateRootIfNotExist();

            var config = _staticData.ForWindow(WindowId.WinnerPopup);
            var prefab = await config.PrefabReference.LoadAssetAsync().Task;
            var window = InstantiateWindow(prefab).GetComponent<WinPopup>();
            window.Construct(_progressService);
            return window.gameObject;
        }

        public async Task<GameObject> ShowLoosePopup()
        {
            await CreateRootIfNotExist();

            var config = _staticData.ForWindow(WindowId.LoosePopup);
            var prefab = await config.PrefabReference.LoadAssetAsync().Task;
            var window = InstantiateWindow(prefab);
            return window.gameObject;
        }

        public async Task<GameObject> CreateBackToLobbyWindow(WindowService windowService)
        {
            await CreateRootIfNotExist();

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
    }
}
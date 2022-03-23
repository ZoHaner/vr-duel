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
        private Transform _uiRoot;

        public GameUIFactory(IStaticDataService staticData)
        {
            _staticData = staticData;
        }

        public GameObject CreateWinnerPopup()
        {
            CreateRootIfNecessary();

            var config = _staticData.ForWindow(WindowId.WinnerPopup);
            var window = InstantiateWindow(config.Prefab);
            return window.gameObject;
        }

        public GameObject ShowLoosePopup()
        {
            CreateRootIfNecessary();

            var config = _staticData.ForWindow(WindowId.LoosePopup);
            var window = InstantiateWindow(config.Prefab);
            return window.gameObject;
        }

        private void CreateRootIfNecessary()
        {
            if (_uiRoot == null)
                CreateUIRoot();
        }

        private void CreateUIRoot()
        {
            _uiRoot = ResourcesUtilities.Instantiate(UIRootPath).transform;
            _uiRoot.position = _rootPosition;
        }

        private WindowBase InstantiateWindow(WindowBase configPrefab)
        {
            var window = Object.Instantiate(configPrefab, _uiRoot);
            window.gameObject.transform.localPosition = Vector3.zero;
            return window;
        }
    }
}
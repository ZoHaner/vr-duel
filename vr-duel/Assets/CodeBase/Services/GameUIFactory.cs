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

        public void CreateRootIfNotExist()
        {
            if (_uiRoot == null)
                CreateUIRoot();
        }

        public GameObject CreateWinnerPopup()
        {
            CreateRootIfNotExist();

            var config = _staticData.ForWindow(WindowId.WinnerPopup);
            var window = InstantiateWindow(config.Prefab);
            return window.gameObject;
        }

        public GameObject ShowLoosePopup()
        {
            CreateRootIfNotExist();

            var config = _staticData.ForWindow(WindowId.LoosePopup);
            var window = InstantiateWindow(config.Prefab);
            return window.gameObject;
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
using CodeBase.Infrastructure.Utilities;
using CodeBase.Services.Network;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPath = "UI/UIRoot";
        private Transform _uiRoot;
        private readonly IStaticDataService _staticData;
        private readonly INetworkService _networkService;

        public UIFactory(IStaticDataService staticData, INetworkService networkService)
        {
            _staticData = staticData;
            _networkService = networkService;
        }

        public void CreateMatchesListWindow()
        {
            if (_uiRoot == null)
                CreateUIRoot();

            var config = _staticData.ForWindow(WindowId.MatchesList);
            var window = Object.Instantiate(config.Prefab, _uiRoot).GetComponent<MatchListWindow>();
            window.Construct(_networkService);
        }

        public void CreateUIRoot()
        {
            _uiRoot = ResourcesUtilities.Instantiate(UIRootPath).transform;
        }
    }
}
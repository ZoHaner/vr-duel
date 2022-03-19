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
            CreateRootIfNecessary();

            var config = _staticData.ForWindow(WindowId.MatchesList);
            var window = Object.Instantiate(config.Prefab, _uiRoot).GetComponent<MatchListWindow>();
            window.Construct(_networkService);
        }

        public void CreateMatchmakingWindow()
        {
            CreateRootIfNecessary();

            var config = _staticData.ForWindow(WindowId.Matchmaking);
            var matchmakingWindow = Object.Instantiate(config.Prefab, _uiRoot).GetComponent<MatchmakingWindow>();
            matchmakingWindow.Construct(_networkService);
        }

        private void CreateRootIfNecessary()
        {
            if (_uiRoot == null)
                CreateUIRoot();
        }

        private void CreateUIRoot()
        {
            _uiRoot = ResourcesUtilities.Instantiate(UIRootPath).transform;
            _uiRoot.position += Vector3.up;
        }
    }
}
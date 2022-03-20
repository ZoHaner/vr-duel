using CodeBase.Infrastructure.Utilities;
using CodeBase.Services;
using CodeBase.Services.Network;
using CodeBase.Services.Progress;
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
        private readonly ISaveLoadService _saveLoadService;

        public UIFactory(IStaticDataService staticData, INetworkService networkService, ISaveLoadService saveLoadService)
        {
            _staticData = staticData;
            _networkService = networkService;
            _saveLoadService = saveLoadService;
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

        public void CreateChoosePlayerNameWindow(INameSelectorService nameSelectorService)
        {
            CreateRootIfNecessary();
            
            var config = _staticData.ForWindow(WindowId.ChoosePlayerName);
            var chooseNameWindow = Object.Instantiate(config.Prefab, _uiRoot).GetComponent<ChooseNameWindow>();
            chooseNameWindow.Construct(nameSelectorService);
        }

        public void CreateGeneratePlayerNameWindow()
        {
            
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
using System.Threading.Tasks;
using CodeBase.Services.Progress;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.Services.UI
{
    public class LobbyUIFactory : UIBaseFactory, ILobbyUIFactory
    {
        protected override string UIRootPath => "UI/Lobby/UIRoot";

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

        public async Task<GameObject> CreateLobbyWindow()
        {
            CreateRootIfNotExist();

            var config = _staticData.ForWindow(WindowId.LobbyWindow);
            var prefab = await config.PrefabReference.LoadAssetAsync().Task;
            var window = InstantiateWindow(prefab).GetComponent<LobbyWindow>();
            window.Construct(_playerDataService, _progressService, _networkService, _closeApplicationService);
            return window.gameObject;
        }
    }
}
using CodeBase.Services;
using CodeBase.Services.Progress;
using CodeBase.UI.Windows.Elements;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    public class LobbyWindow : WindowBase
    {
        [SerializeField] private PlayerInformationContainer _playerInformationContainer;
        [SerializeField] private ConnectionStatusContainer _connectionStatusContainer;
        [SerializeField] private MatchmakingContainer _matchmakingContainer;
        [SerializeField] private ApplicationCloseContainer _applicationCloseContainer;

        public void Construct(IPlayerDataService playerDataService, IProgressService progressService, INetworkService networkService, ICloseApplicationService closeApplicationService)
        {
            _playerInformationContainer.Construct(playerDataService, progressService);
            _connectionStatusContainer.Construct(networkService);
            _matchmakingContainer.Construct(networkService);
            _applicationCloseContainer.Construct(closeApplicationService);
        }

        protected override void Initialize()
        {
            _connectionStatusContainer.Initialize();
            _matchmakingContainer.Initialize();
        }

        protected override void SubscribeUpdates()
        {
            _connectionStatusContainer.SubscribeUpdates();
            _matchmakingContainer.SubscribeUpdates();
            _applicationCloseContainer.SubscribeEvents();
        }

        protected override void Cleanup()
        {
            _connectionStatusContainer.Cleanup();
            _matchmakingContainer.Cleanup();
            _applicationCloseContainer.Cleanup();
        }
    }
}
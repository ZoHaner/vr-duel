using CodeBase.Services;
using UnityEngine;

namespace CodeBase.UI.Windows.Elements
{
    public class MatchmakingContainer : MonoBehaviour
    {
        [SerializeField] private MatchmakingInProcessContainer _matchmakingInProcessContainer;
        [SerializeField] private MatchmakingStoppedContainer _matchmakingStoppedContainer;

        private INetworkService _networkService;

        public void Construct(INetworkService networkService)
        {
            _networkService = networkService;
            _matchmakingInProcessContainer.Construct(networkService);
            _matchmakingStoppedContainer.Construct(networkService);
        }

        public void Initialize()
        {
            SwitchToStoppedWindow();
        }

        public void SubscribeUpdates()
        {
            _networkService.OnDisconnect += SwitchToStoppedWindow;
            _networkService.MatchmakingStarted += SwitchToInProcessWindow;
            _networkService.MatchmakingCanceled += SwitchToStoppedWindow;

            _matchmakingInProcessContainer.SubscribeEvents();
            _matchmakingStoppedContainer.SubscribeEvents();
        }

        public void Cleanup()
        {
            _networkService.OnDisconnect -= SwitchToStoppedWindow;
            _networkService.MatchmakingStarted -= SwitchToInProcessWindow;
            _networkService.MatchmakingCanceled -= SwitchToStoppedWindow;

            _matchmakingInProcessContainer.Cleanup();
            _matchmakingStoppedContainer.Cleanup();
        }

        private void SwitchToInProcessWindow()
        {
            _matchmakingStoppedContainer.Hide();
            _matchmakingInProcessContainer.Show();
        }

        private void SwitchToStoppedWindow()
        {
            _matchmakingInProcessContainer.Hide();
            _matchmakingStoppedContainer.Show();
            _matchmakingStoppedContainer.Initialize();
        }
    }
}
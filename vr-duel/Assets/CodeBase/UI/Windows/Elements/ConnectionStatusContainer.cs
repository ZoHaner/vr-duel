using CodeBase.Services;
using UnityEngine;

namespace CodeBase.UI.Windows.Elements
{
    public class ConnectionStatusContainer : MonoBehaviour
    {
        [SerializeField] private ConnectionEnabledContainer _connectionEnabledContainer;
        [SerializeField] private ConnectionDisabledContainer _connectionDisabledContainer;

        private INetworkService _networkService;

        public void Construct(INetworkService networkService)
        {
            _networkService = networkService;

            _connectionDisabledContainer.Construct(networkService);
        }

        public void Initialize()
        {
            if (_networkService.IsConnected())
            {
                _connectionEnabledContainer.Show();
                return;
            }

            _connectionDisabledContainer.Show();
        }

        public void SubscribeUpdates()
        {
            _networkService.OnConnect += SwitchToConnectedWindow;
            _networkService.OnDisconnect += SwitchToDisconnectedWindow;
        }

        public void Cleanup()
        {
            _networkService.OnConnect -= SwitchToConnectedWindow;
            _networkService.OnDisconnect -= SwitchToDisconnectedWindow;
        }

        private void SwitchToConnectedWindow()
        {
            _connectionDisabledContainer.Hide();
            _connectionEnabledContainer.Show();
        }

        private void SwitchToDisconnectedWindow()
        {
            _connectionEnabledContainer.Hide();
            _connectionDisabledContainer.Show();
        }
    }
}
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.UI.Windows.Elements
{
    public class ConnectionDisabledContainer : MonoBehaviour
    {
        [SerializeField] private GameObject[] _activeGameObjects;
        [SerializeField] private ReconnectButton _reconnectButton;

        public void Construct(INetworkService networkService)
        {
            _reconnectButton.Construct(networkService);
        }

        public void Show()
        {
            foreach (var activeObject in _activeGameObjects)
            {
                activeObject.SetActive(true);
            }
            
            _reconnectButton.SubscribeEvents();
        }

        public void Hide()
        {
            _reconnectButton.Cleanup();
            
            foreach (var activeObject in _activeGameObjects)
            {
                activeObject.SetActive(false);
            }
        }
    }
}
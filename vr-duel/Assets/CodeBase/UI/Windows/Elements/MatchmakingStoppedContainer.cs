using CodeBase.Services;
using UnityEngine;

namespace CodeBase.UI.Windows.Elements
{
    public class MatchmakingStoppedContainer : MonoBehaviour
    {
        [SerializeField] private GameObject[] _activeGameObjects;
        [SerializeField] private StartMatchmakingButton _startMatchmakingButton;
        
        private INetworkService _networkService;

        public void Construct(INetworkService networkService)
        {
            _networkService = networkService;
            _startMatchmakingButton.Construct(networkService);
        }

        public void Initialize()
        {
            CheckButtonInteractable();
        }

        public void SubscribeEvents()
        {
            _networkService.OnConnect += CheckButtonInteractable;
            _networkService.OnDisconnect += CheckButtonInteractable;
        }

        public void Cleanup()
        {
            _networkService.OnConnect -= CheckButtonInteractable;
            _networkService.OnDisconnect -= CheckButtonInteractable;
        }

        public void Show()
        {
            foreach (var activeObject in _activeGameObjects)
            {
                activeObject.SetActive(true);
            }
            
            _startMatchmakingButton.SubscribeEvents();
        }

        public void Hide()
        {
            _startMatchmakingButton.Cleanup();
            
            foreach (var activeObject in _activeGameObjects)
            {
                activeObject.SetActive(false);
            }
        }

        private void CheckButtonInteractable() => 
            _startMatchmakingButton.SetInteractable(_networkService.IsConnected());
    }
}
using CodeBase.Services;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Elements
{
    public class ReconnectButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        private INetworkService _networkService;

        public void Construct(INetworkService networkService)
        {
            _networkService = networkService;
        }

        public void SubscribeEvents()
        {
            _button.onClick.AddListener(OnButtonClick);
            _networkService.OnConnectionError += () => SetInteractableButton(true);
            _networkService.OnConnect += () => SetInteractableButton(false);
            _networkService.OnDisconnect += () => SetInteractableButton(true);
        }
        
        public void Cleanup()
        {
            _button.onClick.RemoveListener(OnButtonClick);
            _networkService.OnConnectionError -= () => SetInteractableButton(true);
            _networkService.OnConnect -= () => SetInteractableButton(false);
            _networkService.OnDisconnect -= () => SetInteractableButton(true);
        }
        
        private void OnButtonClick()
        {
            SetInteractableButton(false);
            _networkService.Connect();
        }

        private void SetInteractableButton(bool interactable)
        {
            _button.interactable = interactable;
        }
    }
}
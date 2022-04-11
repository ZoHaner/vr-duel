using CodeBase.Services;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Elements
{
    public class StartMatchmakingButton : MonoBehaviour
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
        }
        
        public void Cleanup()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        public void SetInteractable(bool isInteractable)
        {
            _button.interactable = isInteractable;
        }
        
        private void OnButtonClick()
        {
            _networkService.AddMatchmaker();
        }
    }
}
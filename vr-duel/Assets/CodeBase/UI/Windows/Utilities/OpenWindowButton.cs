using CodeBase.Services.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Utilities
{
    public class OpenWindowButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        private IWindowService _windowService;
        private WindowId _windowId;

        public void Construct(IWindowService windowService, WindowId windowId)
        {
            _windowService = windowService;
            _windowId = windowId;
        }

        public void SubscribeUpdates()
        {
            _button.onClick.AddListener(Open);
        }

        public void Cleanup()
        {
            _button.onClick.RemoveListener(Open);
        }

        private void Open()
        {
            _windowService.Open(_windowId);
        }
    }
}
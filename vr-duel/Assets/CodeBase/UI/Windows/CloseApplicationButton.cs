using CodeBase.Services;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public class CloseApplicationButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private ICloseApplicationService _closeApplicationService;

        public void Construct(ICloseApplicationService closeApplicationService)
        {
            _closeApplicationService = closeApplicationService;
        }

        public void SubscribeEvents()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        public void Cleanup()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            _closeApplicationService.Execute();
        }
    }

    
}
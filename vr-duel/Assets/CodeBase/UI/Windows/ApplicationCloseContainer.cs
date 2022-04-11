using CodeBase.Services;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    public class ApplicationCloseContainer : MonoBehaviour
    {
        [SerializeField] private CloseApplicationButton _closeApplicationButton;

        public void Construct(ICloseApplicationService closeApplicationService)
        {
            _closeApplicationButton.Construct(closeApplicationService);
        }

        public void SubscribeEvents()
        {
            _closeApplicationButton.SubscribeEvents();
        }

        public void Cleanup()
        {
            _closeApplicationButton.Cleanup();
        }
    }
}
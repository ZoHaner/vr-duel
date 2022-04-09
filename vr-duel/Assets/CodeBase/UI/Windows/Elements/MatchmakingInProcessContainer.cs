using CodeBase.Services;
using UnityEngine;

namespace CodeBase.UI.Windows.Elements
{
    public class MatchmakingInProcessContainer : MonoBehaviour
    {
        [SerializeField] private GameObject[] _activeGameObjects;
        [SerializeField] private StopMatchmakingButton _stopMatchmakingButton;

        public void Construct(INetworkService networkService)
        {
            _stopMatchmakingButton.Construct(networkService);
        }

        public void SubscribeEvents()
        {
            _stopMatchmakingButton.SubscribeEvents();
        }
        
        public void Cleanup()
        {
            _stopMatchmakingButton.Cleanup();
        }
        
        public void Show()
        {
            foreach (var activeObject in _activeGameObjects)
            {
                activeObject.SetActive(true);
            }
        }

        public void Hide()
        {
            foreach (var activeObject in _activeGameObjects)
            {
                activeObject.SetActive(false);
            }
        }
    }
}
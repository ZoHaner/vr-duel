using CodeBase.Services.Network;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public class MatchmakingWindow : WindowBase
    {
        public Button StartMatchmaking;
        public Button StopMatchmaking;

        public GameObject StartMatchmakingUI; 
        public GameObject StopMatchmakingUI;
        
        private INetworkService _networkService;

        public void Construct(INetworkService networkService)
        {
            _networkService = networkService;
        }

        protected override void Initialize()
        {
            StartMatchmakingUI.SetActive(true);
        }

        protected override void SubscribeUpdates()
        {
            StartMatchmaking.onClick.AddListener(StartMatchmakingPressed);
            StopMatchmaking.onClick.AddListener(StopMatchmakingPressed);
        }

        protected override void Cleanup()
        {
            StartMatchmaking.onClick.RemoveListener(StartMatchmakingPressed);
            StopMatchmaking.onClick.RemoveListener(StopMatchmakingPressed);
        }
        
        private async void StartMatchmakingPressed()
        {
            StartMatchmaking.interactable = false;
            
            await _networkService.AddMatchmaker();
            
            StartMatchmakingUI.SetActive(false);
            StopMatchmakingUI.SetActive(true);
        }
        
        private async void StopMatchmakingPressed()
        {
            StartMatchmaking.interactable = true;
            
            await _networkService.CancelMatchmaker();

            StopMatchmakingUI.SetActive(false);
            StartMatchmakingUI.SetActive(true);
        }
    }
}
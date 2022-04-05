using CodeBase.Services;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Elements
{
    public class CreateMatchButton : MonoBehaviour
    {
        public Button Button;

        private INetworkService _networkService;

        public void Construct(INetworkService networkService)
        {
            _networkService = networkService;
        }

        public void SubscribeUpdates()
        {
            Button.onClick.AddListener(() => CreateAndJoinMatch());
        }

        public void Cleanup()
        {
            Button.onClick.RemoveListener(() => CreateAndJoinMatch());
        }

        private async void CreateAndJoinMatch()
        {
            Button.interactable = false;
            var match = await _networkService.CreateMatch();
            Debug.Log($"Match created: '{match.Id}'");
            Button.interactable = true;
        }
    }
}
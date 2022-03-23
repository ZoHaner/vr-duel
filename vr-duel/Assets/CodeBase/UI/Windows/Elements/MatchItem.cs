using CodeBase.Services;
using Nakama;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Elements
{
    public class MatchItem : MonoBehaviour
    {
        public TextMeshProUGUI TextField;
        public Button JoinButton;

        private INetworkService _networkService;
        private IApiMatch _match;

        public void Construct(INetworkService networkService, IApiMatch match)
        {
            _networkService = networkService;
            _match = match;

            UpdateText();
            AddCallback();
        }

        private void AddCallback() => 
            JoinButton.onClick.AddListener(() => _networkService.JoinMatch(_match.MatchId));

        private void UpdateText() => 
            TextField.text = $"ID: '{_match.MatchId}'";
    }
}
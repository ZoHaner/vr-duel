using System.Text;
using CodeBase.Services;
using CodeBase.Services.Progress;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Windows.Elements
{
    public class PlayerInformationContainer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI PlayerInfoText;
        
        private IPlayerDataService _playerDataService;
        private IProgressService _progressService;

        public void Construct(IPlayerDataService playerDataService, IProgressService progressService)
        {
            _playerDataService = playerDataService;
            _progressService = progressService;
        }
        
        public void Initialize() => 
            UpdateInformation();

        private void UpdateInformation()
        {
            string playerInfo = BuildTextInformation();
            SetUpdatedText(playerInfo);
        }

        private void SetUpdatedText(string text) => 
            PlayerInfoText.text = text;

        private string BuildTextInformation()
        {
            var sb = new StringBuilder();
            sb.Append("Hello, ").Append(_playerDataService.User.Username).Append("!\n");
            sb.Append("Your score is: ").Append(_progressService.Progress.WinsCount);
            return sb.ToString();
        }
    }
}
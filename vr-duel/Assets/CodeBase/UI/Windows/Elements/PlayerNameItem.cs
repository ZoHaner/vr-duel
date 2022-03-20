using CodeBase.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Elements
{
    public class PlayerNameItem : MonoBehaviour
    {
        [SerializeField] private Button SelectNameButton;
        [SerializeField] private TextMeshProUGUI NameField;
        
        private INameSelectorService _nameSelectorService;

        public void Construct(INameSelectorService nameSelectorService)
        {
            _nameSelectorService = nameSelectorService;
        }

        public void Initialize(string playerName)
        {
            NameField.text = playerName;
            SelectNameButton.onClick.AddListener(() => _nameSelectorService.OnSelect(playerName));
        }
    }
}
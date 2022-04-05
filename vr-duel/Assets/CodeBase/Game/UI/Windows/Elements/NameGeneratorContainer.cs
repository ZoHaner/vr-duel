using CodeBase.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Elements
{
    public class NameGeneratorContainer : MonoBehaviour
    {
        public string Name { get; private set; }
        
        [SerializeField] private Button GenerateButton;
        [SerializeField] private TextMeshProUGUI NameField;

        private NameGenerator _generator;
        
        public void Construct()
        {
            _generator = new NameGenerator();
        }

        public void Initialize() => 
            SetNewName();

        public void SubscribeUpdates() => 
            GenerateButton.onClick.AddListener(SetNewName);

        public void Cleanup() => 
            GenerateButton.onClick.RemoveListener(SetNewName);

        private void SetNewName() => 
            NameField.text = GenerateName();

        private string GenerateName() => 
            Name = _generator.GetRandomName();
    }
}
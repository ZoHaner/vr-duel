using CodeBase.Services;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Elements
{
    public class ConfirmNameButton : MonoBehaviour
    {
        [SerializeField] private Button ConfirmButton;
        
        private NameGeneratorContainer _nameGeneratorContainer;
        private INameSelectorService _nameSelectorService;

        public void Construct(NameGeneratorContainer nameGeneratorContainer, INameSelectorService nameSelectorService)
        {
            _nameSelectorService = nameSelectorService;
            _nameGeneratorContainer = nameGeneratorContainer;
        }
        
        public void SubscribeUpdates() => 
            ConfirmButton.onClick.AddListener(InvokeSelectNameEvent);

        public void Cleanup() => 
            ConfirmButton.onClick.RemoveListener(InvokeSelectNameEvent);

        private void InvokeSelectNameEvent() => 
            _nameSelectorService.OnSelect(_nameGeneratorContainer.Name);
    }
}
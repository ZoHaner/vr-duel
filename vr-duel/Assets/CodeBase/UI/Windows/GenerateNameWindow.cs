using CodeBase.Services;
using CodeBase.Services.UI;
using CodeBase.UI.Windows.Elements;
using CodeBase.UI.Windows.Utilities;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    public class GenerateNameWindow : WindowBase
    {
        [SerializeField] private OpenWindowButton BackButton;
        [SerializeField] private NameGeneratorContainer NameGeneratorContainer;
        [SerializeField] private ConfirmNameButton ConfirmNameButton;

        public void Construct(INameSelectorService nameSelectorService, IWindowService windowService)
        {
            NameGeneratorContainer.Construct();
            BackButton.Construct(windowService, WindowId.ChoosePlayerName);
            ConfirmNameButton.Construct(NameGeneratorContainer, nameSelectorService);
        }

        protected override void Initialize()
        {
            NameGeneratorContainer.Initialize();
        }

        protected override void SubscribeUpdates()
        {
            NameGeneratorContainer.SubscribeUpdates();
            BackButton.SubscribeUpdates();
            ConfirmNameButton.SubscribeUpdates();
        }

        protected override void Cleanup()
        {
            NameGeneratorContainer.Cleanup();
            BackButton.Cleanup();
            ConfirmNameButton.Cleanup();
        }
    }
}
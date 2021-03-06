using CodeBase.Services;
using CodeBase.Services.UI;
using CodeBase.UI.Windows.Elements;
using CodeBase.UI.Windows.Utilities;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    public class ChooseNameWindow : WindowBase
    {
        [SerializeField] private NameListContainer _nameListContainer;
        [SerializeField] private OpenWindowButton _openGeneratorWindowButton;

        public void Construct(INameSelectorService nameSelectorService, IPlayerAccountsService playersAccountService, IWindowService windowService)
        {
            _nameListContainer.Construct(nameSelectorService, playersAccountService);
            _openGeneratorWindowButton.Construct(windowService, WindowId.GeneratePlayerName);
        }

        protected override void Initialize()
        {
            _nameListContainer.Initialize();
        }

        protected override void SubscribeUpdates()
        {
            _openGeneratorWindowButton.SubscribeUpdates();
        }

        protected override void Cleanup()
        {
            _openGeneratorWindowButton.Cleanup();
        }
    }
}
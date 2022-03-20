using CodeBase.Services;
using CodeBase.UI.Windows.Elements;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    public class ChooseNameWindow : WindowBase
    {
        [SerializeField] private NameListContainer _nameListContainer;

        public void Construct(INameSelectorService nameSelectorService)
        {
            _nameListContainer.Construct(nameSelectorService);
        }

        protected override void Initialize()
        {
            _nameListContainer.Initialize();
        }
    }
}
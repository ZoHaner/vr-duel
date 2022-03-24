using CodeBase.Services.Progress;
using CodeBase.UI.Windows.Elements;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    public class WinPopup : WindowBase
    {
        [SerializeField] private WinsCountContainer WinsCountContainer;
        
        public void Construct(IProgressService progressService)
        {
            WinsCountContainer.Construct(progressService);
        }

        protected override void Initialize()
        {
            WinsCountContainer.Initialize();
        }
    }
}
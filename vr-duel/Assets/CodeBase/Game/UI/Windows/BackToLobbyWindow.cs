using System;
using CodeBase.UI.Windows.Elements;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    public class BackToLobbyWindow : WindowBase
    {
        [SerializeField] private UIButton ContinueButton;
        [SerializeField] private UIButton ExitButton;
        
        public void Construct(Action continueCallback, Action exitCallback)
        {
            ContinueButton.Construct(continueCallback);
            ExitButton.Construct(exitCallback);
        }
        
        protected override void SubscribeUpdates()
        {
            ContinueButton.SubscribeUpdates();
            ExitButton.SubscribeUpdates();
        }

        protected override void Cleanup()
        {
            ContinueButton.Cleanup();
            ExitButton.Cleanup();
        }
    }
}
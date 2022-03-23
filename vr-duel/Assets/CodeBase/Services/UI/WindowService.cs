using UnityEngine;

namespace CodeBase.Services.UI
{
    public class WindowService : IWindowService
    {
        private readonly IUIFactory _uiFactory;

        private GameObject _openWindow;

        public WindowService(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void Open(WindowId windowId)
        {
            CloseWindowIfOpened();

            switch (windowId)
            {
                case WindowId.Unknown:
                    break;
                case WindowId.Matchmaking:
                    _openWindow = _uiFactory.CreateMatchmakingWindow();
                    break;
                case WindowId.MatchesList:
                    _openWindow = _uiFactory.CreateMatchesListWindow();
                    break;
                case WindowId.ChoosePlayerName:
                    _openWindow = _uiFactory.CreateChoosePlayerNameWindow(this);
                    break;
                case WindowId.GeneratePlayerName:
                    _openWindow = _uiFactory.CreateGeneratePlayerNameWindow(this);
                    break;
            }
        }

        private void CloseWindowIfOpened()
        {
            if (_openWindow != null)
            {
                Object.Destroy(_openWindow);
            }

            _openWindow = null;
        }
    }
}
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Services.UI
{
    public class WindowService : IWindowService
    {
        private readonly IUIFactory _uiFactory;
        private readonly IGameUIFactory _gameUIFactory;

        private GameObject _openWindow;

        public WindowService(IUIFactory uiFactory, IGameUIFactory gameUIFactory)
        {
            _uiFactory = uiFactory;
            _gameUIFactory = gameUIFactory;
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
                case WindowId.WinnerPopup:
                    _openWindow = _gameUIFactory.CreateWinnerPopup();
                    break;
                case WindowId.LoosePopup:
                    _openWindow = _gameUIFactory.ShowLoosePopup();
                    break;
                case WindowId.BackToLobby:
                    _openWindow = _gameUIFactory.CreateBackToLobbyWindow(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(windowId), windowId, null);
            }
        }

        public void CloseAllWindows() => 
            CloseWindowIfOpened();

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
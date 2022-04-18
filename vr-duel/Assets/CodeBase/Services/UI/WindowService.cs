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

        public async void Open(WindowId windowId)
        {
            CloseWindowIfOpened();

            switch (windowId)
            {
                case WindowId.Unknown:
                    break;
                case WindowId.Matchmaking:
                    _openWindow = await _uiFactory.CreateMatchmakingWindow();
                    break;
                case WindowId.MatchesList:
                    _openWindow = await _uiFactory.CreateMatchesListWindow();
                    break;
                case WindowId.ChoosePlayerName:
                    _openWindow = await _uiFactory.CreateChoosePlayerNameWindow(this);
                    break;
                case WindowId.GeneratePlayerName:
                    _openWindow = await _uiFactory.CreateGeneratePlayerNameWindow(this);
                    break;
                case WindowId.WinnerPopup:
                    _openWindow = await _gameUIFactory.CreateWinnerPopup();
                    break;
                case WindowId.LoosePopup:
                    _openWindow = await _gameUIFactory.ShowLoosePopup();
                    break;
                case WindowId.BackToLobby:
                    _openWindow = await _gameUIFactory.CreateBackToLobbyWindow(this);
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
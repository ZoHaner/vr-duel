using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Services.UI
{
    public class WindowService : IWindowService
    {
        private readonly IGameUIFactory _gameUIFactory;
        private readonly ISelectNameUIFactory _uiSelectNameFactory;

        private GameObject _openWindow;

        public WindowService(IGameUIFactory gameUIFactory, ISelectNameUIFactory uiSelectNameFactory)
        {
            _gameUIFactory = gameUIFactory;
            _uiSelectNameFactory = uiSelectNameFactory;
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
                    _openWindow = await _uiSelectNameFactory.CreateChoosePlayerNameWindow(this);
                    break;
                case WindowId.GeneratePlayerName:
                    _openWindow = await _uiSelectNameFactory.CreateGeneratePlayerNameWindow(this);
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
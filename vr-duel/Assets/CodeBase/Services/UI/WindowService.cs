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

        public void Open(WindowId windowId)
        {
            CloseWindowIfOpened();

            switch (windowId)
            {
                case WindowId.Unknown:
                    break;
                case WindowId.ChoosePlayerName:
                    _openWindow = _uiSelectNameFactory.CreateChoosePlayerNameWindow(this).Result;
                    break;
                case WindowId.GeneratePlayerName:
                    _openWindow = _uiSelectNameFactory.CreateGeneratePlayerNameWindow(this).Result;
                    break;
                case WindowId.WinnerPopup:
                    _openWindow = _gameUIFactory.CreateWinnerPopup().Result;
                    break;
                case WindowId.LoosePopup:
                    _openWindow = _gameUIFactory.ShowLoosePopup().Result;
                    break;
                case WindowId.BackToLobby:
                    _openWindow = _gameUIFactory.CreateBackToLobbyWindow(this).Result;
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
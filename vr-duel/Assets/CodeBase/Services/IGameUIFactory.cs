using System;
using CodeBase.Services.UI;
using UnityEngine;

namespace CodeBase.Services
{
    public interface IGameUIFactory : IService
    {
        void CreateRootIfNotExist();
        GameObject CreateWinnerPopup();
        GameObject ShowLoosePopup();
        GameObject CreateBackToLobbyWindow(WindowService windowService);
        void SetExitCallback(Action backToLobby);
        void ClearExitCallback();
    }
}
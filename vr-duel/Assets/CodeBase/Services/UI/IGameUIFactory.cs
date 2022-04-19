using System;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Services.UI
{
    public interface IGameUIFactory : IService
    {
        Task<GameObject> CreateWinnerPopup();
        Task<GameObject> ShowLoosePopup();
        Task<GameObject> CreateBackToLobbyWindow(WindowService windowService);
        void SetExitCallback(Action backToLobby);
        void ClearExitCallback();
    }
}
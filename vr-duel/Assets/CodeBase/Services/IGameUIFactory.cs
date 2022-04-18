using System;
using System.Threading.Tasks;
using CodeBase.Services.UI;
using UnityEngine;

namespace CodeBase.Services
{
    public interface IGameUIFactory : IService
    {
        void CreateRootIfNotExist();
        Task<GameObject> CreateWinnerPopup();
        Task<GameObject> ShowLoosePopup();
        Task<GameObject> CreateBackToLobbyWindow(WindowService windowService);
        void SetExitCallback(Action backToLobby);
        void ClearExitCallback();
    }
}
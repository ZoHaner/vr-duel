using UnityEngine;

namespace CodeBase.Services
{
    public interface IGameUIFactory : IService
    {
        void CreateRootIfNotExist();
        GameObject CreateWinnerPopup();
        GameObject ShowLoosePopup();
    }
}
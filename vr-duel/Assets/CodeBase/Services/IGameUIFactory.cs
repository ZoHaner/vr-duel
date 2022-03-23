using UnityEngine;

namespace CodeBase.Services
{
    public interface IGameUIFactory : IService
    {
        GameObject CreateWinnerPopup();
        GameObject ShowLoosePopup();
    }
}
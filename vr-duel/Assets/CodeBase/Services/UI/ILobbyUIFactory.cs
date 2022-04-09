using UnityEngine;

namespace CodeBase.Services.UI
{
    public interface ILobbyUIFactory : IService
    {
        GameObject CreateLobbyWindow();
        void CreateRootIfNotExist();
    }
}
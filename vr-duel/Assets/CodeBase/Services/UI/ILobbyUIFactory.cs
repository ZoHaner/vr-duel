using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Services.UI
{
    public interface ILobbyUIFactory : IService
    {
        Task<GameObject> CreateLobbyWindow();
    }
}
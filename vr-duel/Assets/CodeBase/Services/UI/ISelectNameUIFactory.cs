using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Services.UI
{
    public interface ISelectNameUIFactory : IService
    {
        Task<GameObject> CreateChoosePlayerNameWindow(IWindowService windowService);
        Task<GameObject> CreateGeneratePlayerNameWindow(IWindowService windowService);
    }
}
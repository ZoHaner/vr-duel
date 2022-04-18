using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Services.UI
{
  public interface IUIFactory : IService
  {
    void CreateRootIfNotExist();
    Task<GameObject> CreateMatchesListWindow();
    Task<GameObject> CreateMatchmakingWindow();
    Task<GameObject> CreateChoosePlayerNameWindow(IWindowService windowService);
    Task<GameObject> CreateGeneratePlayerNameWindow(IWindowService windowService);
  }
}
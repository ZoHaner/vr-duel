using UnityEngine;

namespace CodeBase.Services.UI
{
  public interface IUIFactory : IService
  {
    void CreateRootIfNotExist();
    GameObject CreateMatchesListWindow();
    GameObject CreateMatchmakingWindow();
    GameObject CreateChoosePlayerNameWindow(IWindowService windowService);
    GameObject CreateGeneratePlayerNameWindow(IWindowService windowService);
  }
}
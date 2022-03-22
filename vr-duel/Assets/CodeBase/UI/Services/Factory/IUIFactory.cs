using CodeBase.Services;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
  public interface IUIFactory : IService
  {
    GameObject CreateMatchesListWindow();
    GameObject CreateMatchmakingWindow();
    GameObject CreateChoosePlayerNameWindow(IWindowService windowService);
    GameObject CreateGeneratePlayerNameWindow(IWindowService windowService);
  }
}
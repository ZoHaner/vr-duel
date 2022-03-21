using CodeBase.Services;
using CodeBase.UI.Services.Windows;

namespace CodeBase.UI.Services.Factory
{
  public interface IUIFactory : IService
  {
    void CreateMatchesListWindow();
    void CreateMatchmakingWindow();
    void CreateChoosePlayerNameWindow(IWindowService windowService);
    void CreateGeneratePlayerNameWindow();
  }
}
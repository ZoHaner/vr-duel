using CodeBase.Services;

namespace CodeBase.UI.Services
{
  public interface IUIFactory : IService
  {
    void CreateMatchWindow();
  }
}
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Utilities;

namespace CodeBase.UI.Services
{
  public class UIFactory : IUIFactory
  {
    public void CreateMatchWindow()
    {
      ResourcesUtilities.Instantiate(AssetsPath.MatchWindow);
    }
  }
}
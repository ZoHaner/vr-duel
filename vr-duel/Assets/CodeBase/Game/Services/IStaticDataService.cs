using CodeBase.Entities;
using CodeBase.ServiceLocator;
using CodeBase.Services.UI;

namespace CodeBase.Services
{
    public interface IStaticDataService : IService
    {
        WindowConfig ForWindow(WindowId matchControl);
    }
}
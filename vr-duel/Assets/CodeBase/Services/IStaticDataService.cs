using CodeBase.Data;
using CodeBase.Services.UI;

namespace CodeBase.Services
{
    public interface IStaticDataService : IService
    {
        WindowConfig ForWindow(WindowId matchControl);
    }
}
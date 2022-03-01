using CodeBase.StaticData;
using CodeBase.UI.Services.Windows;

namespace CodeBase.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        WindowConfig ForWindow(WindowId matchControl);
    }
}
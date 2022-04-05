using CodeBase.Entities;
using CodeBase.ServiceLocator;

namespace CodeBase.Services
{
    public interface IPlayerDataService : IService
    {
        UserAccount User { get; set; }
    }
}
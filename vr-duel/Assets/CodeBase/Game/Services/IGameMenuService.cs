using CodeBase.ServiceLocator;

namespace CodeBase.Services
{
    public interface IGameMenuService : IService
    {
        void SubscribeEvents();
        void Cleanup();
    }
}
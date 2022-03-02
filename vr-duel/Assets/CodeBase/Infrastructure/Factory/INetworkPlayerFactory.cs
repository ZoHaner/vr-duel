using CodeBase.Services;

namespace CodeBase.Infrastructure.Factory
{
    public interface INetworkPlayerFactory : IService
    {
        void SubscribeEvents();
        void Cleanup();
    }
}
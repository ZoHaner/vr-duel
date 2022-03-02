using CodeBase.Network;

namespace CodeBase.Services.Network
{
    public interface ISessionService : IService
    {
        void Construct(INetworkService networkService, MainThreadDispatcher dispatcher);
        void SubscribeEvents();
        void Cleanup();
    }
}
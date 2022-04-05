using CodeBase.Services;
using Nakama;

namespace CodeBase.Entities.NetworkTest
{
    public interface IClientService : IService
    {
        IClient Client { get; }
        void CreateClient(string scheme, string host, int port, string serverKey);
    }
}
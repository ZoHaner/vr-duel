using Nakama;

namespace CodeBase.Entities.NetworkTest
{
    public interface IClientService
    {
        IClient Client { get; }
        void CreateClient(string scheme, string host, int port, string serverKey);
    }
}
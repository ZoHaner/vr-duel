using Nakama;

namespace CodeBase.Entities.NetworkTest
{
    public class ClientService : IClientService
    {
        public IClient Client { get; private set; }

        private readonly UnityWebRequestAdapter _adapter;

        public ClientService(UnityWebRequestAdapter adapter)
        {
            _adapter = adapter;
        }

        public void CreateClient(string scheme, string host, int port, string serverKey)
        {
            Client = new Client(scheme, host, port, serverKey, _adapter);
        }
    }
}
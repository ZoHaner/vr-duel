using Nakama;

namespace CodeBase.NakamaClient
{
    public class NakamaClient : NetworkAPI.IClient
    {
        public IClient Client { get; private set; }

        private readonly UnityWebRequestAdapter _adapter;

        public NakamaClient(UnityWebRequestAdapter adapter)
        {
            _adapter = adapter;
        }

        public void CreateClient(string scheme, string host, int port, string serverKey)
        {
            Client = new Client(scheme, host, port, serverKey, _adapter);
        }
    }
}
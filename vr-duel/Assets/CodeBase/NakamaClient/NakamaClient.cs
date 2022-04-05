using Nakama;

namespace CodeBase.NakamaClient
{
    public class NakamaClient : NetworkAPI.IClient
    {
        public IClient Client { get; private set; }

        public void CreateClient(string scheme, string host, int port, string serverKey)
        {
            Client = new Client(scheme, host, port, serverKey, _unityWebRequestAdapter);
        }
    }
}
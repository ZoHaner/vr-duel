namespace CodeBase.NetworkAPI
{
    public interface IClient
    {
        void CreateClient(string scheme, string host, int port, string serverKey);
    }
}
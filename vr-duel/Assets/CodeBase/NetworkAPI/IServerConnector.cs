using System.Threading.Tasks;

namespace CodeBase.NetworkAPI
{
    public interface IServerConnector
    {
        bool IsConnected();
        Task ConnectAsync();
        Task DisconnectAsync();
    }
}
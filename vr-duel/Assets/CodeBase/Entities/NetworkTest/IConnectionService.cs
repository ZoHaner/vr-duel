using System.Threading.Tasks;
using Nakama;

namespace CodeBase.Entities.NetworkTest
{
    public interface IConnectionService
    {
        ISocket Socket { get; }
        Task ConnectAsync();
        Task DisconnectAsync();
    }
}
using System.Threading.Tasks;
using CodeBase.Services;
using Nakama;

namespace CodeBase.Entities.NetworkTest
{
    public interface IConnectionService : IService
    {
        ISocket Socket { get; }
        Task ConnectAsync();
        Task DisconnectAsync();
        bool IsConnected();
    }
}
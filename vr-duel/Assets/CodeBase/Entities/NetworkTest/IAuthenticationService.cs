using System.Threading.Tasks;
using CodeBase.Services;
using Nakama;

namespace CodeBase.Entities.NetworkTest
{
    public interface IAuthenticationService : IService
    {
        ISession Session { get; }
        Task AuthenticateDeviceAsync(string userId, string username);
    }
}
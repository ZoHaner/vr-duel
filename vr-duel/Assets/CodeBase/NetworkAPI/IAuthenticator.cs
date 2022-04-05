using System.Threading.Tasks;

namespace CodeBase.NetworkAPI
{
    public interface IAuthenticator
    {
        Task AuthenticateDeviceAsync(string userId, string username);
    }
}
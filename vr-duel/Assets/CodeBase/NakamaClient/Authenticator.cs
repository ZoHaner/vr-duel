using System.Threading.Tasks;
using CodeBase.NetworkAPI;
using Nakama;

namespace CodeBase.NakamaClient
{
    public class Authenticator : IAuthenticator
    {
        private readonly NakamaClient _nakamaClient;
        public ISession Session { get; private set; }

        public Authenticator(NakamaClient nakamaClient)
        {
            _nakamaClient = nakamaClient;
        }
        
        public async Task AuthenticateDeviceAsync(string userId, string username)
        {
            Session = await _nakamaClient.Client.AuthenticateDeviceAsync(userId, username);
        }
    }
}
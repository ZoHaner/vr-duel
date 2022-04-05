using System.Threading.Tasks;
using Nakama;

namespace CodeBase.Entities.NetworkTest
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IClientService _clientService;
        public ISession Session { get; private set; }

        public AuthenticationService(IClientService clientService)
        {
            _clientService = clientService;
        }
        
        public async Task AuthenticateDeviceAsync(string userId, string username)
        {
            Session = await _clientService.Client.AuthenticateDeviceAsync(userId, username);
        }
    }
}
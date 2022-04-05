using System.Threading.Tasks;
using Nakama;
using UnityEngine;

namespace CodeBase.Entities.NetworkTest
{
    public class ConnectionService : IConnectionService
    {
        public ISocket Socket { get; private set; }

        private readonly IAuthenticationService _authenticationService;
        private readonly IClientService _clientService;

        public ConnectionService(IAuthenticationService authenticationService, IClientService clientService)
        {
            _clientService = clientService;
            _authenticationService = authenticationService;
        }

        public async Task ConnectAsync()
        {
            if (Socket == null)
                CreateSocket();

            await Socket.ConnectAsync(_authenticationService.Session, true);
        }

        public async Task DisconnectAsync()
        {
            if (Socket == null)
                return;

            await Socket.CloseAsync();
            Debug.LogError("CONNECTION CLOSED");
        }

        private void CreateSocket()
        {
            Socket = _clientService.Client.NewSocket();
        }
    }
}
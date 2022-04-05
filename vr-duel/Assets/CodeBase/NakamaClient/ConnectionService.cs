using System.Threading.Tasks;
using CodeBase.NetworkAPI;
using Nakama;
using UnityEngine;

namespace CodeBase.NakamaClient
{
    public class ConnectionService : IServerConnector
    {
        public ISocket Socket { get; private set; }

        private readonly Authenticator _authenticator;
        private readonly NakamaClient _nakamaClient;

        public ConnectionService(Authenticator authenticator, NakamaClient nakamaClient)
        {
            _nakamaClient = nakamaClient;
            _authenticator = authenticator;
        }

        public bool IsConnected()
        {
            if (Socket != null)
                return Socket.IsConnected;

            return false;
        }
        
        public async Task ConnectAsync()
        {
            if (Socket == null)
                CreateSocket();

            await Socket.ConnectAsync(_authenticator.Session, true);
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
            Socket = _nakamaClient.Client.NewSocket();
        }
    }
}
using System;
using System.Threading.Tasks;
using CodeBase.Infrastructure;
using CodeBase.Network;
using Nakama;
using UnityEngine;

namespace CodeBase.Services.Network
{
    public class NetworkService
    {
        public ISocket Socket;
        public IMatchmakerMatched MatchmakerMatch { get; private set; }
        public IMatch Match { private set; get; }

        public Action MatchJoined;

        private IClient _client;
        private ISession _session;
        private string _ticket;

        public async Task Connect()
        {
            var configHolder = Resources.Load<NetworkConfigHolder>(AssetsPath.ConfigHolder);
            var config = configHolder.GetActiveConfig();
            _client = new Client(config.Scheme, config.Host, config.Port, config.ServerKey, UnityWebRequestAdapter.Instance);
            _session = await _client.AuthenticateDeviceAsync(/*SystemInfo.deviceUniqueIdentifier*/ Guid.NewGuid().ToString());
            Socket = _client.NewSocket();
            await Socket.ConnectAsync(_session, true);

            
            Debug.LogError(_session);
            Debug.LogError(Socket);
        }

        public async Task FindMatch()
        {
            Debug.LogError("Finding match ...");
            var matchtakingTicket = await Socket.AddMatchmakerAsync("", 2, 2);
            _ticket = matchtakingTicket.Ticket;
            Debug.LogError("Ticket : " + _ticket);
        }

        public async void JoinMatch(IMatchmakerMatched matchmakerMatch)
        {
            Debug.LogError("Match found!");
            Match = await Socket.JoinMatchAsync(matchmakerMatch);
            MatchmakerMatch = matchmakerMatch;
            MainThreadDispatcher.Instance().Enqueue(() => MatchJoined?.Invoke());
        }

        public async Task Disconnect()
        {
            await Socket.CloseAsync();
        }
    }
}
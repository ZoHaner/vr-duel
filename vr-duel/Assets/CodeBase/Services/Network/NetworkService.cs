using System;
using System.Threading.Tasks;
using CodeBase.Infrastructure;
using CodeBase.Network;
using Nakama;
using UnityEngine;

namespace CodeBase.Services.Network
{
    public class NetworkService : INetworkService
    {
        public ISocket Socket { get; set; }
        private IMatch Match { set; get; }
        public Action<IMatch> MatchJoined { get; set; }
        public Action<IMatchmakerMatched> ReceivedMatchmakerMatched { get; set; }
        public Action<IMatchPresenceEvent> ReceivedMatchPresence { get; set; }
        public Action<IMatchState> ReceivedMatchState { get; set; }
        public Action<IApiMatchList> MatchListFound { get; set; }

        public const int MinPlayers = 2;
        public const int MaxPlayers = 10;

        private IClient _client;
        private ISession _session;
        private string _ticket;
        private UnityWebRequestAdapter _adapter;
        private MainThreadDispatcher _dispatcher;
        private readonly ICoroutineRunner _coroutineRunner;
        private Coroutine _matchSearchingCoroutine;
        private IPlayerDataService _playerData;

        private const int MatchesSearchLimit = 0;
        private const bool Authoritative = false;
        private const string Query = "*";

        public NetworkService(UnityWebRequestAdapter adapter, MainThreadDispatcher dispatcher, IPlayerDataService playerData)
        {
            _adapter = adapter;
            _dispatcher = dispatcher;
            _playerData = playerData;
        }

        public void SubscribeEvents()
        {
            Socket.ReceivedMatchmakerMatched += m => _dispatcher.Enqueue(() => OnReceivedMatchmakerMatched(m));
            Socket.ReceivedMatchPresence += m => _dispatcher.Enqueue(() => OnReceivedMatchPresence(m));
            Socket.ReceivedMatchState += m => _dispatcher.Enqueue(async () => await OnReceivedMatchState(m));
        }

        public void Cleanup()
        {
            Socket.ReceivedMatchmakerMatched -= m => _dispatcher.Enqueue(() => OnReceivedMatchmakerMatched(m));
            Socket.ReceivedMatchPresence -= m => _dispatcher.Enqueue(() => OnReceivedMatchPresence(m));
            Socket.ReceivedMatchState -= m => _dispatcher.Enqueue(async () => await OnReceivedMatchState(m));
        }

        private void OnReceivedMatchmakerMatched(IMatchmakerMatched matchmakerMatched)
        {
            JoinMatch(matchmakerMatched);
            ReceivedMatchmakerMatched?.Invoke(matchmakerMatched);
        }

        private void OnReceivedMatchPresence(IMatchPresenceEvent matchPresenceEvent)
        {
            ReceivedMatchPresence?.Invoke(matchPresenceEvent);
        }

        private async Task OnReceivedMatchState(IMatchState matchState)
        {
            ReceivedMatchState?.Invoke(matchState);
        }

        public async Task Connect()
        {
            var configHolder = Resources.Load<NetworkConfigHolder>(AssetsPath.ConfigHolder);
            var config = configHolder.GetActiveConfig();
            _client = new Client(config.Scheme, config.Host, config.Port, config.ServerKey, _adapter);
            _session = await _client.AuthenticateDeviceAsync(_playerData.User.Id, _playerData.User.Username);
            Socket = _client.NewSocket();
            await Socket.ConnectAsync(_session, true);

            Debug.LogError(_client);
            Debug.LogError(_session);
            Debug.LogError(Socket);
        }

        public async Task Disconnect()
        {
            await Socket.CloseAsync();
            Debug.LogError("CONNECTION CLOSED");
        }

        public async Task<IApiMatchList> GetMatchList()
        {
            var result = await _client.ListMatchesAsync(
                _session,
                MinPlayers,
                MaxPlayers,
                MatchesSearchLimit,
                Authoritative,
                null,
                Query);

            foreach (var match in result.Matches)
            {
                Debug.LogFormat("MatchId : " + match.MatchId);
            }

            MatchListFound?.Invoke(result);
            return result;
        }

        public async Task<IMatch> CreateMatch()
        {
            var match = await Socket.CreateMatchAsync();
            return match;
        }

        public async Task<IMatch> JoinMatch(string matchId)
        {
            var match = await Socket.JoinMatchAsync(matchId);
            _dispatcher.Enqueue(() => MatchJoined?.Invoke(match));

            return match;
        }

        public async Task AddMatchmaker()
        {
            Debug.LogError("Finding match ...");
            var matchmakerTicket = await Socket.AddMatchmakerAsync(Query, MinPlayers, MaxPlayers);
            _ticket = matchmakerTicket.Ticket;
            Debug.LogError("Ticket : " + _ticket);
        }

        public async Task CancelMatchmaker()
        {
            await Socket.RemoveMatchmakerAsync(_ticket);
        }

        public async void JoinMatch(IMatchmakerMatched matchmakerMatch)
        {
            Debug.LogError("Match found!");
            Match = await Socket.JoinMatchAsync(matchmakerMatch);
            _dispatcher.Enqueue(() => MatchJoined?.Invoke(Match));
        }

        public void SendMatchState(long opCode, string state)
        {
            Socket.SendMatchStateAsync(Match.Id, opCode, state);
        }
    }
}
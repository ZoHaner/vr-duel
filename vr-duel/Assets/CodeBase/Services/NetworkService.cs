using System;
using System.Threading.Tasks;
using CodeBase.Infrastructure;
using CodeBase.Services.Singletons;
using CodeBase.StaticData;
using Nakama;
using UnityEngine;

namespace CodeBase.Services
{
    public class NetworkService : INetworkService
    {
        public Action OnConnect { get; set; }
        public Action OnDisconnect { get; set; }
        public Action OnConnectionError { get; set; }
        public Action MatchmakingStarted { get; set; }
        public Action MatchmakingCanceled { get; set; }
        public Action<IMatch> MatchJoined { get; set; }
        public Action<IMatchmakerMatched> ReceivedMatchmakerMatched { get; set; }
        public Action<IMatchPresenceEvent> ReceivedMatchPresence { get; set; }
        public Action<IMatchState> ReceivedMatchState { get; set; }
        public Action<IApiMatchList> MatchListFound { get; set; }

        public const int MinPlayers = 2;
        public const int MaxPlayers = 10;

        private IClient _client;
        private ISession _session;
        private ISocket _socket;
        private IMatch _match;
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

        public bool IsConnected()
        {
            if (_socket != null)
                return _socket.IsConnected;

            return false;
        }

        public async Task Connect()
        {
            var configHolder = Resources.Load<NetworkConfigHolder>(AssetsPath.ConfigHolder);
            var config = configHolder.GetActiveConfig();
            _client = new Client(config.Scheme, config.Host, config.Port, config.ServerKey, _adapter);

            try
            {
                _session = await _client.AuthenticateDeviceAsync(_playerData.User.Id, _playerData.User.Username);
            }
            catch (ApiResponseException ex)
            {
                OnConnectionError?.Invoke();
                Debug.LogError($"Authentication error: '{ex}'");
                return;
            }

            _socket = _client.NewSocket();
            SubscribeSocketEvents();

            try
            {
                await _socket.ConnectAsync(_session, true);
            }
            catch (ApiResponseException ex)
            {
                OnConnectionError?.Invoke();
                Debug.LogError($"Socket connection error: '{ex}'");
                return;
            }

            Debug.LogError(_client);
            Debug.LogError(_session);
            Debug.LogError(_socket);
        }

        public async Task Disconnect()
        {
            if (!IsConnected())
                return;

            Cleanup();

            try
            {
                await _socket.CloseAsync();
                Debug.LogError("CONNECTION CLOSED");
            }
            catch (ApiResponseException ex)
            {
                Debug.LogError($"Socket connection error: '{ex}'");
            }
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
            var match = await _socket.CreateMatchAsync();
            return match;
        }

        public async Task<IMatch> JoinMatch(string matchId)
        {
            var match = await _socket.JoinMatchAsync(matchId);
            _dispatcher.Enqueue(() => MatchJoined?.Invoke(match));

            return match;
        }

        public async Task AddMatchmaker()
        {
            Debug.LogError("Finding match ...");
            MatchmakingStarted?.Invoke();
            var matchmakerTicket = await _socket.AddMatchmakerAsync(Query, MinPlayers, MaxPlayers);
            _ticket = matchmakerTicket.Ticket;
            Debug.LogError("Ticket : " + _ticket);
        }

        public async Task CancelMatchmaker()
        {
            await _socket.RemoveMatchmakerAsync(_ticket);
            MatchmakingCanceled?.Invoke();
        }

        public async void JoinMatch(IMatchmakerMatched matchmakerMatch)
        {
            Debug.LogError("Match found!");
            _match = await _socket.JoinMatchAsync(matchmakerMatch);
            _dispatcher.Enqueue(() => MatchJoined?.Invoke(_match));
        }

        public async void LeaveMatch(string matchId)
        {
            await _socket.LeaveMatchAsync(matchId);
        }

        public void SendMatchState(long opCode, string state)
        {
            _socket.SendMatchStateAsync(_match.Id, opCode, state);
        }

        private void SubscribeSocketEvents()
        {
            _socket.Connected += () => _dispatcher.Enqueue(OnConnected);
            _socket.Closed += () => _dispatcher.Enqueue(OnDisconnected);
            _socket.ReceivedMatchmakerMatched += m => _dispatcher.Enqueue(() => OnReceivedMatchmakerMatched(m));
            _socket.ReceivedMatchPresence += m => _dispatcher.Enqueue(() => OnReceivedMatchPresence(m));
            _socket.ReceivedMatchState += m => _dispatcher.Enqueue(async () => await OnReceivedMatchState(m));
        }

        private void Cleanup()
        {
            _socket.Connected -= () => _dispatcher.Enqueue(OnConnected);
            _socket.Closed -= () => _dispatcher.Enqueue(OnDisconnected);
            _socket.ReceivedMatchmakerMatched -= m => _dispatcher.Enqueue(() => OnReceivedMatchmakerMatched(m));
            _socket.ReceivedMatchPresence -= m => _dispatcher.Enqueue(() => OnReceivedMatchPresence(m));
            _socket.ReceivedMatchState -= m => _dispatcher.Enqueue(async () => await OnReceivedMatchState(m));
        }

        private void OnConnected()
        {
            OnConnect?.Invoke();
        }

        private void OnDisconnected()
        {
            OnDisconnect?.Invoke();
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
    }
}
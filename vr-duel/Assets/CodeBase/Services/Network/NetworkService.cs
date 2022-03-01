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
        public IMatchmakerMatched MatchmakerMatch { get; private set; }
        public IMatch Match { private set; get; }
        public Action MatchJoined { get; set; }
        public Action<IApiMatchList> MatchListFound { get; set; }

        public const int MinPlayers = 2;
        public const int MaxPlayers = 10;

        private IClient _client;
        private ISession _session;
        private string _ticket;
        private UnityWebRequestAdapter _unityWebRequestAdapter;
        private MainThreadDispatcher _mainThreadDispatcher;
        private readonly ICoroutineRunner _coroutineRunner;
        private Coroutine _matchSearchingCoroutine;

        private const int MatchesSearchLimit = 10;
        private const bool Authoritative = true;
        private const string Label = "";
        private const string Query = "";


        public NetworkService(UnityWebRequestAdapter unityWebRequestAdapter, MainThreadDispatcher mainThreadDispatcher)
        {
            _unityWebRequestAdapter = unityWebRequestAdapter;
            _mainThreadDispatcher = mainThreadDispatcher;
        }

        public async Task Connect()
        {
            var configHolder = Resources.Load<NetworkConfigHolder>(AssetsPath.ConfigHolder);
            var config = configHolder.GetActiveConfig();
            _unityWebRequestAdapter = UnityWebRequestAdapter.Instance;
            _client = new Client(config.Scheme, config.Host, config.Port, config.ServerKey, _unityWebRequestAdapter);
            _session = await _client.AuthenticateDeviceAsync(/*SystemInfo.deviceUniqueIdentifier*/ Guid.NewGuid().ToString());
            Socket = _client.NewSocket();
            await Socket.ConnectAsync(_session, true);

            Debug.LogError(_session);
            Debug.LogError(Socket);
        }


        public async Task<IApiMatchList> GetMatchList()
        {
            var result = await _client.ListMatchesAsync(
                _session, 
                MinPlayers, 
                MaxPlayers, 
                MatchesSearchLimit, 
                Authoritative, 
                Label, 
                Query);

            foreach (var match in result.Matches)
            {
                Debug.LogFormat("{0}: {1}/{2} players", match.MatchId, match.Size, MaxPlayers);
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
            _mainThreadDispatcher.Enqueue(() => MatchJoined?.Invoke());

            return match;
        }

        // public async Task AddMatchmaker()
        // {
        //     Debug.LogError("Finding match ...");
        //     var matchmakerTicket = await Socket.AddMatchmakerAsync(Query, MinPlayers, MaxPlayers);
        //     _ticket = matchmakerTicket.Ticket;
        //     Debug.LogError("Ticket : " + _ticket);
        // }

        // public async void JoinMatch(IMatchmakerMatched matchmakerMatch)
        // {
        //     Debug.LogError("Match found!");
        //     Match = await Socket.JoinMatchAsync(matchmakerMatch);
        //     MatchmakerMatch = matchmakerMatch;
        //     _mainThreadDispatcher.Enqueue(() => MatchJoined?.Invoke());
        // }

        public async Task Disconnect()
        {
            await Socket.CloseAsync();
            Debug.LogError("CONNECTION CLOSED");
        }
    }
}
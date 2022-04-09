using System;
using System.Threading.Tasks;
using Nakama;

namespace CodeBase.Services
{
    public interface INetworkService : IService
    {
        Task Connect();
        Task Disconnect();
        Action OnConnect { get; set; }
        Action OnDisconnect { get; set; }
        Action OnConnectionError { get; set; }
        Action MatchmakingStarted { get; set; }
        Action MatchmakingCanceled { get; set; }
        Action<IMatchmakerMatched> ReceivedMatchmakerMatched { get; set; }
        Action<IMatch> MatchJoined { get; set; }
        Action<IMatchPresenceEvent> ReceivedMatchPresence { get; set; }
        Action<IMatchState> ReceivedMatchState { get; set; }
        Action<IApiMatchList> MatchListFound { set; get; }
        Task<IApiMatchList> GetMatchList();
        Task<IMatch> JoinMatch(string matchId);
        Task<IMatch> CreateMatch();
        Task AddMatchmaker();
        Task CancelMatchmaker();
        void JoinMatch(IMatchmakerMatched matchmakerMatch);
        void SendMatchState(long opCode, string state);
        bool IsConnected();
        void LeaveMatch(string matchId);
    }
}
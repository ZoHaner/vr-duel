using System;
using System.Threading.Tasks;
using Nakama;

namespace CodeBase.Services.Network
{
    public interface INetworkService : IService
    {
        Task Connect();
        Task Disconnect();
        Action<IMatchmakerMatched> ReceivedMatchmakerMatched { get; set; }
        Action<IMatch> MatchJoined { get; set; }
        Action<IMatchPresenceEvent> ReceivedMatchPresence { get; set; }
        Action<IMatchState> ReceivedMatchState { get; set; }
        Action<IApiMatchList> MatchListFound { set; get; }
        Task<IApiMatchList> GetMatchList();
        Task<IMatch> JoinMatch(string matchId);
        Task<IMatch> CreateMatch();
        Task AddMatchmaker();
        void JoinMatch(IMatchmakerMatched matchmakerMatch);
        void SubscribeEvents();
        void Cleanup();
        void SendMatchState(long opCode, string state);
    }
}
using System;
using System.Threading.Tasks;
using Nakama;

namespace CodeBase.Services.Network
{
    public interface INetworkService : IService
    {
        Task Connect();
        //Task AddMatchmaker();
        //void JoinMatch(IMatchmakerMatched matchmakerMatch);
        Task Disconnect();
        IMatchmakerMatched MatchmakerMatch { get; }
        IMatch Match { get; }
        Action MatchJoined { get; set; }
        ISocket Socket { get; set; }
        Action<IApiMatchList> MatchListFound { get; set; }
        Task<IApiMatchList> GetMatchList();
        Task<IMatch> JoinMatch(string matchId);
        Task<IMatch> CreateMatch();
        Task AddMatchmaker();
        void JoinMatch(IMatchmakerMatched matchmakerMatch);
        void Cleanup();
    }
}
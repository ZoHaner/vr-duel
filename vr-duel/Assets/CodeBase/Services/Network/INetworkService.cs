using System;
using System.Threading.Tasks;
using CodeBase.Network;
using Nakama;

namespace CodeBase.Services.Network
{
    public interface INetworkService : IService
    {
        Task Connect();
        Task FindMatch();
        void JoinMatch(IMatchmakerMatched matchmakerMatch);
        Task Disconnect();
        IMatchmakerMatched MatchmakerMatch { get; }
        IMatch Match { get; }
        Action MatchJoined { get; set; }
        ISocket Socket { get; set; }
    }
}
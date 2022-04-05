using System;
using CodeBase.ServiceLocator;
using Nakama;

namespace CodeBase.Services
{
    public interface IRoundService : IService
    {
        public Action<IUserPresence> PlayerDeath { get; set; }
        void SubscribeEvents();
        void Cleanup();
        void LeaveRound();
        void CheckPlayersCountAndStartRound();
    }
}
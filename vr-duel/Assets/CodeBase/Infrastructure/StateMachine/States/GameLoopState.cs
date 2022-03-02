using CodeBase.Services.Network;
using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly INetworkService _networkService;
        private readonly ISessionService _sessionService;

        private readonly Vector3 _gunPivotOffset = new Vector3(0.4f, 0.7f, 0f);

        public GameLoopState(INetworkService networkService, ISessionService sessionService)
        {
            _networkService = networkService;
            _sessionService = sessionService;
        }

        public async void Enter()
        {
            await _networkService.Connect();
            _sessionService.SubscribeEvents();
            await _networkService.AddMatchmaker();
        }

        public void Exit()
        {
            _sessionService.Cleanup();
        }
    }
}
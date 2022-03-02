using CodeBase.Infrastructure.Factory;
using CodeBase.Services.Network;
using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly INetworkService _networkService;
        private readonly INetworkPlayerFactory _networkPlayerFactory;

        private readonly Vector3 _gunPivotOffset = new Vector3(0.4f, 0.7f, 0f);

        public GameLoopState(INetworkService networkService, INetworkPlayerFactory networkPlayerFactory)
        {
            _networkService = networkService;
            _networkPlayerFactory = networkPlayerFactory;
        }

        public async void Enter()
        {
            await _networkService.Connect();
            _networkService.SubscribeEvents();
            _networkPlayerFactory.SubscribeEvents();
            
            await _networkService.AddMatchmaker();
        }

        public void Exit()
        {
            _networkPlayerFactory.Cleanup();
            _networkService.Cleanup();
            _networkService.Disconnect();
        }
    }
}
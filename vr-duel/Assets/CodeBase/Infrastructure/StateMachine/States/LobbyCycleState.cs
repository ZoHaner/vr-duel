using CodeBase.Services.Network;
using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class LobbyCycleState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly INetworkService _networkService;

        public LobbyCycleState(GameStateMachine gameStateMachine, INetworkService networkService)
        {
            _gameStateMachine = gameStateMachine;
            _networkService = networkService;
            
            _networkService.MatchJoined += MatchJoined;
        }

        public async void Enter()
        {
            await _networkService.Connect();
            // await _networkService.AddMatchmaker();
            // _networkService.Socket.ReceivedMatchmakerMatched += _networkService.JoinMatch;
        }

        private void MatchJoined()
        {
            _gameStateMachine.Enter<LoadGameLevelState, string>(AssetsPath.GameSceneName);
            Debug.LogError("OnMatchJoined");
            Debug.LogError("Session id : " + _networkService.Match.Id);
        }

        public void Exit()
        {
        }
    }
}
using CodeBase.Services;
using CodeBase.Services.Network;
using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class LobbyCycleState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly NetworkService _networkService;

        public LobbyCycleState(GameStateMachine gameStateMachine, NetworkService networkService)
        {
            _gameStateMachine = gameStateMachine;
            _networkService = networkService;
            
            _networkService.MatchJoined += MatchJoined;
        }

        public async void Enter()
        {
            await _networkService.Connect();
            await _networkService.FindMatch();
            _networkService.Socket.ReceivedMatchmakerMatched += _networkService.JoinMatch;
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
using System.Threading.Tasks;
using CodeBase.Services;
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
        }

        public async void Enter()
        {
            await _networkService.Connect();
            // find match
            // and wait for another player
            _gameStateMachine.Enter<LoadLevelState, string>(AssetsPath.GameSceneName);
        }

        public void Exit()
        {
        }
    }
}
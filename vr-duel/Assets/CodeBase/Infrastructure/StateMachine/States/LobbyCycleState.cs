using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class LobbyCycleState : IState
    {
        private readonly GameStateMachine _gameStateMachine;

        public LobbyCycleState(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public async void Enter()
        {
            // find match
            // and wait for another player
            await Task.Delay(1000);
            _gameStateMachine.Enter<LoadLevelState, string>(AssetsPath.GameSceneName);
        }

        public void Exit()
        {
        }
    }
}
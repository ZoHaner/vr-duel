using System.Threading.Tasks;
using CodeBase.Network;
using CodeBase.Services;

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

        private void MatchJoined()
        {
            _gameStateMachine.Enter<LoadGameLevelState, string>(AssetsPath.GameSceneName);
        }

        public async void Enter()
        {
            await _networkService.Connect();
            await _networkService.FindMatch();
        }

        public void Exit()
        {
        }
    }
}
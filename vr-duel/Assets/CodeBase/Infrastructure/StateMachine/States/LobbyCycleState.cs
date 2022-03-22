using CodeBase.Services;
using CodeBase.Services.Network;
using CodeBase.UI.Services.Windows;
using Nakama;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class LobbyCycleState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly INetworkService _networkService;
        private readonly IRoundService _roundService;
        private readonly IWindowService _windowService;

        public LobbyCycleState(GameStateMachine gameStateMachine, INetworkService networkService, IRoundService roundService, IWindowService windowService)
        {
            _gameStateMachine = gameStateMachine;
            _networkService = networkService;
            _roundService = roundService;
            _windowService = windowService;
        }

        public async void Enter()
        {
            await _networkService.Connect();
            _networkService.SubscribeEvents();
            _roundService.SubscribeEvents();
            
            _networkService.ReceivedMatchmakerMatched += LoadGameState;
            
            _windowService.Open(WindowId.Matchmaking);
        }

        private void LoadGameState(IMatchmakerMatched matchmakerMatched)
        {
            _gameStateMachine.Enter<LoadGameLevelState, string>(AssetsPath.GameSceneName);
        }

        public void Exit()
        {
            _networkService.ReceivedMatchmakerMatched -= LoadGameState;
        }
    }
}
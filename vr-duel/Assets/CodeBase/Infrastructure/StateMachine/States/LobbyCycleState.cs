using CodeBase.Services;
using CodeBase.Services.Network;
using CodeBase.UI.Services.Factory;
using Nakama;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class LobbyCycleState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly INetworkService _networkService;
        private readonly IUIFactory _uiFactory;
        private readonly IRoundService _roundService;

        public LobbyCycleState(GameStateMachine gameStateMachine, INetworkService networkService, IUIFactory uiFactory, IRoundService roundService)
        {
            _gameStateMachine = gameStateMachine;
            _networkService = networkService;
            _uiFactory = uiFactory;
            _roundService = roundService;
        }

        public async void Enter()
        {
            await _networkService.Connect();
            _networkService.SubscribeEvents();
            _roundService.SubscribeEvents();
            
            _networkService.ReceivedMatchmakerMatched += LoadGameState;
            
            _uiFactory.CreateMatchmakingWindow();
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
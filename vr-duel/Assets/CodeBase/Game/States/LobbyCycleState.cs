using System.Threading.Tasks;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Services;
using CodeBase.Services.UI;
using CodeBase.StaticData;
using Nakama;

namespace CodeBase.States
{
    public class LobbyCycleState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly INetworkService _networkService;
        private readonly IRoundService _roundService;
        private readonly IWindowService _windowService;
        private MatchmakerService _matchmakerService;
        private readonly IConnectionService _connectionService;
        private readonly IServerEventsService _serverEventsService;

        public LobbyCycleState(GameStateMachine gameStateMachine, INetworkService networkService, IRoundService roundService, IWindowService windowService, IConnectionService connectionService, IServerEventsService serverEventsService)
        {
            _gameStateMachine = gameStateMachine;
            _networkService = networkService;
            _roundService = roundService;
            _windowService = windowService;
            _connectionService = connectionService;
            _serverEventsService = serverEventsService;
        }

        public async void Enter()
        {
            await ConnectIfNotConnected();
            _serverEventsService.SubscribeEvents();
            _roundService.SubscribeEvents();

            _matchmakerService = new MatchmakerService(_networkService);
            _matchmakerService.OnMatched += LoadGameState;
            // _networkService.ReceivedMatchmakerMatched += LoadGameState;

            _windowService.Open(WindowId.Matchmaking);
        }

        private async Task ConnectIfNotConnected()
        {
            if (!_connectionService.IsConnected())
                await _connectionService.ConnectAsync();
        }

        private void LoadGameState(IMatchmakerMatched matchmakerMatched)
        {
            _gameStateMachine.Enter<LoadGameLevelState, string>(AssetsPath.GameSceneName);
        }

        public void Exit()
        {
            // _networkService.ReceivedMatchmakerMatched -= LoadGameState;
            _matchmakerService.OnMatched -= LoadGameState;
        }
    }
}
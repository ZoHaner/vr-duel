using System.Threading.Tasks;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Services;
using CodeBase.StaticData;
using Nakama;

namespace CodeBase.States
{
    public class LobbyCycleState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly INetworkService _networkService;
        private readonly IRoundService _roundService;

        public LobbyCycleState(GameStateMachine gameStateMachine, INetworkService networkService, IRoundService roundService)
        {
            _gameStateMachine = gameStateMachine;
            _networkService = networkService;
            _roundService = roundService;
        }

        public async void Enter()
        {
            await ConnectIfNotConnected();
            _roundService.SubscribeEvents();

            _networkService.ReceivedMatchmakerMatched += LoadGameState;

        }

        private async Task ConnectIfNotConnected()
        {
            if (!_networkService.IsConnected())
                await _networkService.Connect();
        }

        private void LoadGameState(IMatchmakerMatched matchmakerMatched)
        {
            _gameStateMachine.Enter<LoadGameLevelState, string>(AssetAddresses.GameSceneName);
        }

        public void Exit()
        {
            _networkService.ReceivedMatchmakerMatched -= LoadGameState;
        }
    }
}
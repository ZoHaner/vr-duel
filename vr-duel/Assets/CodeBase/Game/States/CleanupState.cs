using CodeBase.Infrastructure.StateMachine;
using CodeBase.Services;

namespace CodeBase.States
{
    public class CleanupState : IState
    {
        private readonly INetworkService _networkService;
        private readonly IRoundService _roundService;

        public CleanupState(INetworkService networkService, IRoundService roundService)
        {
            _networkService = networkService;
            _roundService = roundService;
        }

        public void Enter()
        {
            _roundService.Cleanup();
            _networkService.Cleanup();
            if (_networkService.IsConnected())
                _networkService.Disconnect();
        }

        public void Exit()
        {
        }
    }
}
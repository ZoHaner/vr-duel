using CodeBase.Services;

namespace CodeBase.Infrastructure.StateMachine.States
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
            _networkService.Disconnect();
        }

        public void Exit()
        {
        }
    }
}
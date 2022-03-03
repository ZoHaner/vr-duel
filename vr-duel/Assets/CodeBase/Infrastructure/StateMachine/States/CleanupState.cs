using CodeBase.Services.Network;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class CleanupState : IState
    {
        private readonly INetworkService _networkService;

        public CleanupState(INetworkService networkService)
        {
            _networkService = networkService;
        }
        
        public void Enter()
        {
        }

        public void Exit()
        {
        }
    }
}
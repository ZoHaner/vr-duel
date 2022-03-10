using CodeBase.Services;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class ChoosingNameState : IState
    {
        private readonly INameSelectorService _nameSelectorService;
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPlayerDataService _playerDataService;

        public ChoosingNameState(GameStateMachine gameStateMachine, INameSelectorService nameSelectorService, IPlayerDataService playerDataService)
        {
            _gameStateMachine = gameStateMachine;
            _nameSelectorService = nameSelectorService;
            _playerDataService = playerDataService;
        }

        public void Enter()
        {
            _nameSelectorService.OnSelect += OnSelectedName;
        }

        private void OnSelectedName(string username)
        {
            _playerDataService.Username = username;
            _gameStateMachine.Enter<LoadProgressState>();
        }


        public void Exit()
        {
            _nameSelectorService.OnSelect -= OnSelectedName;
        }
    }
}
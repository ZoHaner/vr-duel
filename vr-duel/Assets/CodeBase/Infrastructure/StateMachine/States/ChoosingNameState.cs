using System.Linq;
using CodeBase.Services;
using CodeBase.UI.Services.Factory;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class ChoosingNameState : IState
    {
        private readonly INameSelectorService _nameSelectorService;
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPlayerDataService _playerDataService;
        private readonly IUIFactory _uiFactory;

        public ChoosingNameState(GameStateMachine gameStateMachine, INameSelectorService nameSelectorService, IPlayerDataService playerDataService, IUIFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            _nameSelectorService = nameSelectorService;
            _playerDataService = playerDataService;
            _uiFactory = uiFactory;
        }

        public void Enter()
        {
            _nameSelectorService.OnSelect += OnSelectedName;

            if (_nameSelectorService.GetSavedPlayersNames().Any())
            {
                _uiFactory.CreateChoosePlayerNameWindow(_nameSelectorService);
                return;
            }

            _uiFactory.CreateGeneratePlayerNameWindow();
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
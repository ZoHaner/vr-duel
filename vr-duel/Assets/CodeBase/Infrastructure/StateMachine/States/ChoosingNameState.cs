using System.Linq;
using CodeBase.Services;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class ChoosingNameState : IState
    {
        private readonly INameSelectorService _nameSelectorService;
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPlayerDataService _playerDataService;
        private readonly IUIFactory _uiFactory;
        private readonly IWindowService _windowService;

        public ChoosingNameState(GameStateMachine gameStateMachine, INameSelectorService nameSelectorService, IPlayerDataService playerDataService, IUIFactory uiFactory, IWindowService windowService)
        {
            _gameStateMachine = gameStateMachine;
            _nameSelectorService = nameSelectorService;
            _playerDataService = playerDataService;
            _uiFactory = uiFactory;
            _windowService = windowService;
        }

        public void Enter()
        {
            _nameSelectorService.OnSelect += OnSelectedName;

            if (_nameSelectorService.GetSavedPlayersNames().Any())
            {
                _windowService.Open(WindowId.ChoosePlayerName);
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
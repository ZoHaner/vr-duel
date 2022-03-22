using System.Linq;
using CodeBase.Services;
using CodeBase.UI.Services.Windows;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class ChoosingNameState : IState
    {
        private readonly INameSelectorService _nameSelectorService;
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPlayerDataService _playerDataService;
        private readonly IWindowService _windowService;
        private readonly IPlayerAccountsService _accountsService;

        public ChoosingNameState(GameStateMachine gameStateMachine, INameSelectorService nameSelectorService, IPlayerDataService playerDataService, IWindowService windowService, IPlayerAccountsService accountsService)
        {
            _gameStateMachine = gameStateMachine;
            _nameSelectorService = nameSelectorService;
            _playerDataService = playerDataService;
            _windowService = windowService;
            _accountsService = accountsService;
        }

        public void Enter()
        {
            _nameSelectorService.OnSelect += OnSelectedName;

            if (_accountsService.GetAccountsList().Any())
            {
                _windowService.Open(WindowId.ChoosePlayerName);
                return;
            }

            _windowService.Open(WindowId.GeneratePlayerName);
        }

        private void OnSelectedName(string username)
        {
            CreateAccountIfNotExist(username);

            _playerDataService.Username = username;
            _gameStateMachine.Enter<LoadProgressState>();
        }

        private void CreateAccountIfNotExist(string username)
        {
            if (!_accountsService.AccountExist(username))
            {
                _accountsService.SaveNewAccount(username);
            }
        }

        public void Exit()
        {
            _nameSelectorService.OnSelect -= OnSelectedName;
        }
    }
}
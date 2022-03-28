using System.Linq;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Services;
using CodeBase.Services.UI;
using CodeBase.StaticData;
using CodeBase.Utilities;

namespace CodeBase.States
{
    public class ChoosingNameState : IState
    {
        private readonly INameSelectorService _nameSelectorService;
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPlayerDataService _playerDataService;
        private readonly IWindowService _windowService;
        private readonly IPlayerAccountsService _accountsService;
        private readonly SceneLoader _sceneLoader;
        private readonly IPlayerFactory _playerFactory;

        public ChoosingNameState(GameStateMachine gameStateMachine, INameSelectorService nameSelectorService, IPlayerDataService playerDataService, IWindowService windowService, IPlayerAccountsService accountsService, SceneLoader sceneLoader, IPlayerFactory playerFactory)
        {
            _gameStateMachine = gameStateMachine;
            _nameSelectorService = nameSelectorService;
            _playerDataService = playerDataService;
            _windowService = windowService;
            _accountsService = accountsService;
            _sceneLoader = sceneLoader;
            _playerFactory = playerFactory;
        }

        public void Enter() => 
            _sceneLoader.Load(AssetsPath.NameSelectionSceneName, OnSceneLoaded);

        private void OnSceneLoaded()
        {
            _playerFactory.SpawnLocalPlayer();

            _windowService.Open(_accountsService.GetAccountsList().Any() ? 
                WindowId.ChoosePlayerName : 
                WindowId.GeneratePlayerName);

            _nameSelectorService.OnSelect += OnSelectedName;
        }

        private void OnSelectedName(string username)
        {
            CreateAccountIfNotExist(username);
            var user = _accountsService.GetAccountByUsername(username);
            _playerDataService.User = user;
            _gameStateMachine.Enter<LoadProgressState>();
        }

        private void CreateAccountIfNotExist(string username)
        {
            if (!_accountsService.AccountExist(username)) 
                _accountsService.SaveNewAccount(username);
        }

        public void Exit() => 
            _nameSelectorService.OnSelect -= OnSelectedName;
    }
}
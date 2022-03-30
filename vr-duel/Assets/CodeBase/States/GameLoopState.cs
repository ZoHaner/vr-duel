using CodeBase.Infrastructure.StateMachine;
using CodeBase.Services;
using CodeBase.Services.Progress;

namespace CodeBase.States
{
    public class GameLoopState : IState
    {
        private readonly IRoundService _roundService;
        private readonly ISaveLoadProgressService _saveLoadProgressService;
        private readonly IPlayerDataService _playerData;
        private readonly IProgressService _playerProgress;
        private readonly IGameUIFactory _gameUIFactory;
        private readonly GameStateMachine _stateMachine;
        private readonly IGameMenuService _gameMenuService;
        private readonly IPlayerFactory _playerFactory;

        public GameLoopState(GameStateMachine stateMachine, IRoundService roundService, ISaveLoadProgressService saveLoadProgressService, IPlayerDataService playerData, IProgressService playerProgress, IGameUIFactory gameUIFactory, IGameMenuService gameMenuService, IPlayerFactory playerFactory)
        {
            _roundService = roundService;
            _saveLoadProgressService = saveLoadProgressService;
            _playerData = playerData;
            _playerProgress = playerProgress;
            _gameUIFactory = gameUIFactory;
            _gameMenuService = gameMenuService;
            _playerFactory = playerFactory;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _playerFactory.Initialize();
            _roundService.CheckPlayersCountAndStartRound();
            _gameUIFactory.CreateRootIfNotExist();
            _gameUIFactory.SetExitCallback(BackToLobby);
            _gameMenuService.SubscribeEvents();
        }

        public void Exit()
        {
            _saveLoadProgressService.SaveProgressForPlayer(_playerData.User.Username, _playerProgress.Progress);
            _roundService.LeaveRound();
            _roundService.Cleanup();
            _gameUIFactory.ClearExitCallback();
            _gameMenuService.Cleanup();
            _playerFactory.CleanUp();
        }

        private void BackToLobby()
        {
            _stateMachine.Enter<LoadLobbyLevelState>();
        }
    }
}
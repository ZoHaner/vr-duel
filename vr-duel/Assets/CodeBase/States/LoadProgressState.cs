using CodeBase.Entities;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Services;
using CodeBase.Services.Progress;

namespace CodeBase.States
{
    public class LoadProgressState : IState
    {
        private readonly IProgressService _progressService;
        private readonly ISaveLoadProgressService _saveLoadProgressService;
        private readonly IPlayerDataService _playerDataService;
        private readonly GameStateMachine _gameStateMachine;

        public LoadProgressState(GameStateMachine gameStateMachine, IProgressService progressService, ISaveLoadProgressService saveLoadProgressService, IPlayerDataService playerDataService)
        {
            _progressService = progressService;
            _saveLoadProgressService = saveLoadProgressService;
            _playerDataService = playerDataService;
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            LoadProgressOrCreateNew();
        }

        public void Exit()
        {
        }

        private void LoadProgressOrCreateNew()
        {
            var username = _playerDataService.User.Username;
            _progressService.Progress = _saveLoadProgressService.LoadProgressForPlayer(username) ?? NewProgress();
            _gameStateMachine.Enter<LoadLobbyLevelState>();
        }

        private PlayerProgress NewProgress()
        {
            return new PlayerProgress(0);
        }
    }
}
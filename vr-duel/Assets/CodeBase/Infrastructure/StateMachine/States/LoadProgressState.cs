using CodeBase.Services;
using CodeBase.Services.Progress;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class LoadProgressState : IState
    {
        private readonly IProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IPlayerDataService _playerDataService;
        private readonly GameStateMachine _gameStateMachine;

        public LoadProgressState(GameStateMachine gameStateMachine, IProgressService progressService, ISaveLoadService saveLoadService, IPlayerDataService playerDataService)
        {
            _progressService = progressService;
            _saveLoadService = saveLoadService;
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
            _progressService.Progress = _saveLoadService.LoadProgressForPlayer(username) ?? NewProgress();
            _gameStateMachine.Enter<LobbyCycleState>();
        }

        private PlayerProgress NewProgress()
        {
            return new PlayerProgress(0);
        }
    }
}
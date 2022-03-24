using CodeBase.Infrastructure.StateMachine;
using CodeBase.Services;
using CodeBase.Services.Progress;
using CodeBase.Services.UI;
using UnityEngine;

namespace CodeBase.States
{
    public class GameLoopState : IState
    {
        private readonly IRoundService _roundService;
        private readonly ISaveLoadProgressService _saveLoadProgressService;
        private readonly IPlayerDataService _playerData;
        private readonly IProgressService _playerProgress;
        private readonly IGameUIFactory _uiFactory;

        public GameLoopState(IRoundService roundService, ISaveLoadProgressService saveLoadProgressService, IPlayerDataService playerData, IProgressService playerProgress, IGameUIFactory uiFactory)
        {
            _roundService = roundService;
            _saveLoadProgressService = saveLoadProgressService;
            _playerData = playerData;
            _playerProgress = playerProgress;
            _uiFactory = uiFactory;
        }

        public void Enter()
        {
            Debug.LogError("Enter GameLoopState");
            _roundService.CheckPlayersCountAndStartRound();
            _uiFactory.CreateRootIfNotExist();
        }

        public void Exit()
        {
            _saveLoadProgressService.SaveProgressForPlayer(_playerData.User.Username, _playerProgress.Progress);
            _roundService.StopRound();
            _roundService.Cleanup();
        }
    }
}
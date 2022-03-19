using CodeBase.Services;
using CodeBase.Services.Progress;
using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly IRoundService _roundService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IPlayerDataService _playerData;
        private readonly IProgressService _playerProgress;

        public GameLoopState(IRoundService roundService, ISaveLoadService saveLoadService, IPlayerDataService playerData, IProgressService playerProgress)
        {
            _roundService = roundService;
            _saveLoadService = saveLoadService;
            _playerData = playerData;
            _playerProgress = playerProgress;
        }

        public void Enter()
        {
            Debug.LogError("Enter GameLoopState");
            _roundService.CheckPlayersCountAndStartRound();
        }

        public void Exit()
        {
            _saveLoadService.SaveProgressForPlayer(_playerData.Username, _playerProgress.Progress);
            _roundService.StopRound();
            _roundService.Cleanup();
        }
    }
}
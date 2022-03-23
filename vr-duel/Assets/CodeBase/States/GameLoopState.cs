using CodeBase.Infrastructure.StateMachine;
using CodeBase.Services;
using CodeBase.Services.Progress;
using UnityEngine;

namespace CodeBase.States
{
    public class GameLoopState : IState
    {
        private readonly IRoundService _roundService;
        private readonly ISaveLoadProgressService _saveLoadProgressService;
        private readonly IPlayerDataService _playerData;
        private readonly IProgressService _playerProgress;

        public GameLoopState(IRoundService roundService, ISaveLoadProgressService saveLoadProgressService, IPlayerDataService playerData, IProgressService playerProgress)
        {
            _roundService = roundService;
            _saveLoadProgressService = saveLoadProgressService;
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
            _saveLoadProgressService.SaveProgressForPlayer(_playerData.User.Username, _playerProgress.Progress);
            _roundService.StopRound();
            _roundService.Cleanup();
        }
    }
}
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Services;
using CodeBase.Services.Progress;
using CodeBase.StaticData;
using UnityEngine;

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

        public GameLoopState(GameStateMachine stateMachine, IRoundService roundService, ISaveLoadProgressService saveLoadProgressService, IPlayerDataService playerData, IProgressService playerProgress, IGameUIFactory gameUIFactory)
        {
            _roundService = roundService;
            _saveLoadProgressService = saveLoadProgressService;
            _playerData = playerData;
            _playerProgress = playerProgress;
            _gameUIFactory = gameUIFactory;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            Debug.LogError("Enter GameLoopState");
            _roundService.CheckPlayersCountAndStartRound();
            _gameUIFactory.CreateRootIfNotExist();
            _gameUIFactory.SetExitCallback(BackToLobby);
        }

        public void Exit()
        {
            _saveLoadProgressService.SaveProgressForPlayer(_playerData.User.Username, _playerProgress.Progress);
            _roundService.StopRound();
            _roundService.Cleanup();
            _gameUIFactory.ClearExitCallback();
        }

        private void BackToLobby()
        {
            _stateMachine.Enter<LoadLobbyLevelState, string>(AssetsPath.LobbySceneName);
        }
    }
}
using CodeBase.Services;
using CodeBase.Services.Network;
using CodeBase.Services.Progress;
using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly INetworkService _networkService;
        private readonly IRoundService _roundService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IPlayerDataService _playerData;
        private readonly IProgressService _playerProgress;

        private readonly Vector3 _gunPivotOffset = new Vector3(0.4f, 0.7f, 0f);

        public GameLoopState(INetworkService networkService, IRoundService roundService, ISaveLoadService saveLoadService, IPlayerDataService playerData, IProgressService playerProgress)
        {
            _networkService = networkService;
            _roundService = roundService;
            _saveLoadService = saveLoadService;
            _playerData = playerData;
            _playerProgress = playerProgress;
        }

        public async void Enter()
        {
            await _networkService.Connect();
            _networkService.SubscribeEvents();
            _roundService.SubscribeEvents();
            
            await _networkService.AddMatchmaker();
        }

        public void Exit()
        {
            _saveLoadService.SaveProgressForPlayer(_playerData.Username, _playerProgress.Progress);
            _roundService.Cleanup();
            _networkService.Cleanup();
            _networkService.Disconnect();
        }
    }
}
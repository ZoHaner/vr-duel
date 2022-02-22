using System.Linq;
using CodeBase.Infrastructure.Utilities;
using CodeBase.Logic;
using CodeBase.Services.Network;
using Nakama;
using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class LoadGameLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly InitialPointHolder _initialPointHolder;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly INetworkService _networkService;
        private readonly Vector3 _gunPivotOffset = new Vector3(0.4f, 0.7f, 0f);


        public LoadGameLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, InitialPointHolder initialPointHolder, LoadingCurtain loadingCurtain, INetworkService networkService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _initialPointHolder = initialPointHolder;
            _loadingCurtain = loadingCurtain;
            _networkService = networkService;
        }

        public void Enter(string sceneName)
        {
            Debug.LogError("LoadGameLevelState | Enter ");

            _loadingCurtain.Show();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }

        private void OnLoaded()
        {
            InstantiatePlayers();
            
            _gameStateMachine.Enter<GameLoopState>();
        }

        private void InstantiatePlayers()
        {
            Debug.LogError("InstantiatePlayers");
            Debug.LogError($"_networkService.Match.Presences.Count() : {_networkService.Match.Presences.Count()}");

            foreach (var user in _networkService.Match.Presences)
            {
                SpawnPlayer(_networkService.Match.Id, user);
            }
        }

        private void SpawnPlayer(string matchId, IUserPresence user)
        {
            IUserPresence localUser = _networkService.MatchmakerMatch.Self.Presence;
            var isLocal = user.SessionId == localUser.SessionId;
            Debug.LogError($"Is local : {isLocal} ; user.SessionId =  {user.SessionId} ; localUser.SessionId {localUser.SessionId}");
            var playerPrefabPath = isLocal ? AssetsPath.LocalPlayer : AssetsPath.NetworkPlayer;

            var initialPoint = _initialPointHolder.GetInitialPoint(isLocal? 1 : 0);
            var player = ResourcesUtilities.Instantiate(playerPrefabPath, initialPoint);
        }

        // private void InstantiatePlayerWithGun()
        // {
        //     var initialPoint = _initialPointHolder.GetInitialPoint(0);
        //     var player = ResourcesUtilities.Instantiate(AssetsPath.Player, initialPoint);
        //     
        //     var cameraTransform = Camera.main.transform;
        //     var gunPivot = cameraTransform.position + _gunPivotOffset;
        //     var gun = ResourcesUtilities.Instantiate(AssetsPath.Revolver, gunPivot, new Vector3(90,0,0));
        // }
    }
}
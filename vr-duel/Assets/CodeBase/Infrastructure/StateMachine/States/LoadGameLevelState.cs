using CodeBase.Infrastructure.Utilities;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class LoadGameLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly InitialPointHolder _initialPointHolder;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly Vector3 _gunPivotOffset = new Vector3(0.4f, 0.7f, 0f);


        public LoadGameLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, InitialPointHolder initialPointHolder, LoadingCurtain loadingCurtain)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _initialPointHolder = initialPointHolder;
            _loadingCurtain = loadingCurtain;
        }

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }

        private void OnLoaded()
        {
            InstantiatePlayerWithGun();
            
            _gameStateMachine.Enter<GameLoopState>();
        }

        private void InstantiatePlayerWithGun()
        {
            var initialPoint = _initialPointHolder.GetInitialPoint(0);
            var player = ResourcesUtilities.Instantiate(AssetsPath.Player, initialPoint);
            
            var cameraTransform = Camera.main.transform;
            var gunPivot = cameraTransform.position + _gunPivotOffset;
            var gun = ResourcesUtilities.Instantiate(AssetsPath.Revolver, gunPivot, new Vector3(90,0,0));
        }
    }
}
using CodeBase.Infrastructure.Utilities;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string PlayerPath = "Player/XR Player";
        private const string RevolverPath = "Guns/Revolver";
        private const string GunPivotName = "GunPivot";
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly InitialPointHolder _initialPointHolder;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly Vector3 _gunPivotOffset = new Vector3(0.4f, 0.7f, 0f);


        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, InitialPointHolder initialPointHolder, LoadingCurtain loadingCurtain)
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
            var player = Instantiate(PlayerPath, initialPoint);
            
            var cameraTransform = Camera.main.transform;
            var gunPivot = cameraTransform.position + _gunPivotOffset;
            var gun = Instantiate(RevolverPath, gunPivot, new Vector3(90,0,0));
        }

        private static GameObject Instantiate(string path)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }
        
        private static GameObject Instantiate(string path, Vector3 at)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity);
        }
        
        private static GameObject Instantiate(string path, Vector3 at, Vector3 rotation)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.Euler(rotation));
        }
        
    }
}
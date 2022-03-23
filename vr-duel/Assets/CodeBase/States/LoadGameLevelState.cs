using CodeBase.Infrastructure.StateMachine;
using CodeBase.UI;
using CodeBase.Utilities;
using UnityEngine;

namespace CodeBase.States
{
    public class LoadGameLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;

        public LoadGameLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
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
            _gameStateMachine.Enter<GameLoopState>();
        }
    }
}
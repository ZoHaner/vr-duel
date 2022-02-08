using CodeBase.Infrastructure.Utilities;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class LoadLobbyLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public LoadLobbyLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter(string sceneName)
        {
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        private void OnLoaded()
        {
            var player = ResourcesUtilities.Instantiate(AssetsPath.LocalPlayer);
            _gameStateMachine.Enter<LobbyCycleState>();
        }

        public void Exit()
        {
        }
    }
}
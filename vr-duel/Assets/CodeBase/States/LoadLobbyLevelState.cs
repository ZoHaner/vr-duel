using CodeBase.Infrastructure.StateMachine;
using CodeBase.Services;
using CodeBase.Services.UI;
using CodeBase.Utilities;

namespace CodeBase.States
{
    public class LoadLobbyLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IWindowService _windowService;
        private readonly IPlayerFactory _playerFactory;

        public LoadLobbyLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IWindowService windowService, IPlayerFactory playerFactory)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _windowService = windowService;
            _playerFactory = playerFactory;
        }

        public void Enter(string sceneName)
        {
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        private void OnLoaded()
        {
            _playerFactory.SpawnLocalPlayer();
            _windowService.Open(WindowId.MatchesList);
            _gameStateMachine.Enter<ChoosingNameState>();
        }

        public void Exit()
        {
        }
    }
}
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Services;
using CodeBase.Services.UI;
using CodeBase.StaticData;
using CodeBase.Utilities;

namespace CodeBase.States
{
    public class LoadLobbyLevelState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IPlayerFactory _playerFactory;
        private readonly ILobbyUIFactory _lobbyUIFactory;
        private ILobbyConfigureService _lobbyService;

        public LoadLobbyLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IPlayerFactory playerFactory, ILobbyUIFactory lobbyUIFactory, ILobbyConfigureService lobbyService)
        {
            _lobbyUIFactory = lobbyUIFactory;
            _lobbyService = lobbyService;
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _playerFactory = playerFactory;
        }

        public void Enter()
        {
            _sceneLoader.Load(AssetsPath.LobbySceneName, OnLoaded);
        }

        private void OnLoaded()
        {
            
            _lobbyService.ConfigureLobbyObjects();
            _gameStateMachine.Enter<LobbyCycleState>();
        }

        public void Exit()
        {
        }
    }
}
using CodeBase.Infrastructure.Utilities;
using CodeBase.Services.UI;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class LoadLobbyLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;
        private readonly IWindowService _windowService;

        public LoadLobbyLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IUIFactory uiFactory, IWindowService windowService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _windowService = windowService;
        }

        public void Enter(string sceneName)
        {
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        private void OnLoaded()
        {
            var player = ResourcesUtilities.Instantiate(AssetsPath.LocalPlayer);
            _windowService.Open(WindowId.MatchesList);
            _gameStateMachine.Enter<ChoosingNameState>();
        }

        public void Exit()
        {
        }
    }
}
using CodeBase.Services.Input;
using CodeBase.Services.Singletons;
using CodeBase.Services.UI;

namespace CodeBase.Services
{
    public class GameMenuService : IGameMenuService
    {
        private IInputService _inputService;
        private IWindowService _windowService;
        private IUpdateProvider _updateProvider;

        public GameMenuService(IInputService inputService, IWindowService windowService, IUpdateProvider updateProvider)
        {
            _updateProvider = updateProvider;
            _windowService = windowService;
            _inputService = inputService;
        }

        public void SubscribeEvents()
        {
            _updateProvider.AddListener(CheckMenuInputs);
        }

        public void Cleanup()
        {
            _updateProvider.RemoveListener(CheckMenuInputs);
        }

        private void CheckMenuInputs()
        {
            if(_inputService.IsMenuButtonPressed())
                _windowService.Open(WindowId.BackToLobby);
        }
    }
}
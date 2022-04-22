using CodeBase.Extensions;
using CodeBase.Services.UI;

namespace CodeBase.Services
{
    public class LobbyConfigureService : ILobbyConfigureService
    {
        private readonly IPlayerFactory _playerFactory;
        private readonly ILobbyUIFactory _lobbyUIFactory;

        public LobbyConfigureService(IPlayerFactory playerFactory, ILobbyUIFactory lobbyUIFactory)
        {
            _playerFactory = playerFactory;
            _lobbyUIFactory = lobbyUIFactory;
        }
        
        public async void ConfigureLobbyObjects()
        {
            var player = await _playerFactory.SpawnLobbyPlayer();
            var window = await _lobbyUIFactory.CreateLobbyWindow();
            player.RotateToObjectAroundYAxis(window);
        }
    }
}
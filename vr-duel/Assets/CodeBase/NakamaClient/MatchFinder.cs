using System.Threading.Tasks;
using CodeBase.NetworkAPI;
using UnityEngine;

namespace CodeBase.NakamaClient
{
    public class MatchFinder : IMatchFinder
    {
        private readonly ConnectionService _connectionService;
        
        private string _ticket;

        private const int MinPlayers = 2;
        private const int MaxPlayers = 10;
        private const string Query = "*";

        public MatchFinder(ConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public async Task StartMatchFindingAsync()
        {
            Debug.LogError("Finding match ...");
            var matchmakerTicket = await _connectionService.Socket.AddMatchmakerAsync(Query, MinPlayers, MaxPlayers);
            _ticket = matchmakerTicket.Ticket;
            Debug.LogError("Ticket : " + _ticket);
        }

        public async Task CancelMatchFindingAsync()
        {
            await _connectionService.Socket.RemoveMatchmakerAsync(_ticket);
        }
    }
}
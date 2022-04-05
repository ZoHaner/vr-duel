using System;
using CodeBase.Services;
using Nakama;

namespace CodeBase.Entities.NetworkTest
{
    public class MatchmakerService
    {
        public Action<IMatchmakerMatched> OnMatched { get; set; }

        //private IClientService _networkService;

        public MatchmakerService(INetworkService networkService)
        {
            //_networkService = networkService;
        }

        void StartMatchmaking()
        {
        }

        void StopMatchmaking()
        {
        }
        
        // public async Task AddMatchmaker()
        // {
        //     Debug.LogError("Finding match ...");
        //     var matchmakerTicket = await _socket.AddMatchmakerAsync(Query, MinPlayers, MaxPlayers);
        //     _ticket = matchmakerTicket.Ticket;
        //     Debug.LogError("Ticket : " + _ticket);
        // }
        //
        // public async Task CancelMatchmaker()
        // {
        //     await _socket.RemoveMatchmakerAsync(_ticket);
        // }
    }
    
}
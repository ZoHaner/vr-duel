using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Utilities;
using CodeBase.Network;
using Nakama;
using UnityEngine;

namespace CodeBase.Services.Network
{
    public class SessionService : ISessionService
    {
        private INetworkService _networkService;
        private MainThreadDispatcher _dispatcher;
        private InitialPointHolder _pointHolder;

        Dictionary<string, GameObject> _players = new Dictionary<string, GameObject>();

        
        public void Construct(INetworkService networkService, MainThreadDispatcher dispatcher)
        {
            _pointHolder = new InitialPointHolder();
            _dispatcher = dispatcher;
            _networkService = networkService;
        }

        public void SubscribeEvents()
        {
            _networkService.Socket.ReceivedMatchmakerMatched += m => _dispatcher.Enqueue(() => OnReceivedMatchmakerMatched(m));
            _networkService.Socket.ReceivedMatchPresence += m => _dispatcher.Enqueue(() => OnReceivedMatchPresence(m));
            _networkService.Socket.ReceivedMatchState += m => _dispatcher.Enqueue(async () => await OnReceivedMatchState(m));
        }

        public void Cleanup()
        {
            _networkService.Socket.ReceivedMatchmakerMatched -= m => _dispatcher.Enqueue(() => OnReceivedMatchmakerMatched(m));
            _networkService.Socket.ReceivedMatchPresence -= m => _dispatcher.Enqueue(() => OnReceivedMatchPresence(m));
            _networkService.Socket.ReceivedMatchState -= m => _dispatcher.Enqueue(async () => await OnReceivedMatchState(m));

        }

        private void OnReceivedMatchmakerMatched(IMatchmakerMatched matchmakerMatched)
        {
            InstantiatePlayers();
        }

        private void OnReceivedMatchPresence(IMatchPresenceEvent matchPresenceEvent)
        {
            UpdatePlayers(matchPresenceEvent);
        }

        private async Task OnReceivedMatchState(IMatchState matchState)
        {
            throw new System.NotImplementedException();
        }
        
        private void InstantiatePlayers()
        {
            Debug.LogError("FirstInstantiatePlayers");
            Debug.LogError($"_networkService.Match.Presences.Count() : {_networkService.Match.Presences.Count()}");
            Debug.LogError($"MatchID : {_networkService.Match.Id}");

            foreach (var user in _networkService.Match.Presences)
            {
                SpawnPlayer(user);
            }
        }

        private void SpawnPlayer(IUserPresence user)
        {
            IUserPresence localUser = _networkService.MatchmakerMatch.Self.Presence;
            var isLocal = user.SessionId == localUser.SessionId;
            Debug.LogError($"Is local : {isLocal} ; user.SessionId =  {user.SessionId} ; localUser.SessionId {localUser.SessionId}");
            var playerPrefabPath = isLocal ? AssetsPath.LocalPlayer : AssetsPath.NetworkPlayer;

            var initialPoint = _pointHolder.GetInitialPoint(isLocal ? 1 : 0);
            var player = ResourcesUtilities.Instantiate(playerPrefabPath, initialPoint);

            _players.Add(user.SessionId, player);
        }

        private void UpdatePlayers(IMatchPresenceEvent matchPresenceEvent)
        {
            Debug.LogError("UpdatePlayers");

            foreach (var presence in matchPresenceEvent.Joins)
            {
                SpawnPlayer(presence);
            }

            foreach (var presence in matchPresenceEvent.Leaves)
            {
                if (_players.ContainsKey(presence.SessionId))
                {
                    _players[presence.SessionId].SetActive(false);
                    _players.Remove(presence.SessionId);
                }
            }
        }
        
    }
}
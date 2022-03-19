using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeBase.Infrastructure.Factory;
using CodeBase.Player;
using CodeBase.Services.Network;
using CodeBase.Services.Progress;
using Nakama;
using Nakama.TinyJson;
using UnityEngine;

namespace CodeBase.Services
{
    public enum RoundState
    {
        Disable,
        WaitForPlayers,
        Playing
    }

    public class RoundService : IRoundService
    {
        public Action<IUserPresence> PlayerDeath { get; set; }

        private INetworkService _networkService;
        private INetworkPlayerFactory _playerFactory;
        private IProgressService _progressService;
        private IPlayerDataService _playerDara;

        private RoundState RoundState = RoundState.Disable;

        private const int MinPlayers = 2;

        private List<IUserPresence> _createdPlayers = new List<IUserPresence>();
        private List<IUserPresence> _waitingPlayers = new List<IUserPresence>();

        public RoundService(INetworkService networkService, INetworkPlayerFactory playerFactory, IProgressService progressService, IPlayerDataService playerDara)
        {
            _networkService = networkService;
            _playerFactory = playerFactory;
            _progressService = progressService;
            _playerDara = playerDara;
        }

        public void SubscribeEvents()
        {
            _networkService.ReceivedMatchmakerMatched += CacheLocalUser;
            _networkService.MatchJoined += MatchJoined;
            _networkService.ReceivedMatchPresence += ReceivedMatchPresence;
            _networkService.ReceivedMatchState += ReceivedMatchState;
            
            PlayerDeath += OnPlayerDeath;
        }

        public void Cleanup()
        {
            _networkService.ReceivedMatchmakerMatched -= CacheLocalUser;
            _networkService.MatchJoined -= MatchJoined;
            _networkService.ReceivedMatchPresence -= ReceivedMatchPresence;
            _networkService.ReceivedMatchState -= ReceivedMatchState;
            
            PlayerDeath -= OnPlayerDeath;
        }

        private void CacheLocalUser(IMatchmakerMatched matched)
        {
            _playerFactory.LocalUserSessionId = matched.Self.Presence.SessionId;

            RoundState = RoundState.WaitForPlayers;
        }

        private void MatchJoined(IMatch match)
        {
            switch (RoundState)
            {
                case RoundState.Disable:
                    AddPlayersToWaitList(match.Presences);
                    if (EnoughPlayers())
                        StartRound();
                    break;
                case RoundState.WaitForPlayers:
                    AddPlayersToWaitList(match.Presences);
                    if (EnoughPlayers())
                        StartRound();
                    break;
                case RoundState.Playing:
                    Debug.LogError("You shouldn't be here");
                    break;
            }
        }

        private void ReceivedMatchPresence(IMatchPresenceEvent matchPresenceEvent)
        {
            switch (RoundState)
            {
                case RoundState.Disable:
                    Debug.LogError("Match updated event, but RoundState = Disable");
                    break;
                case RoundState.WaitForPlayers:
                    TryStartRound(matchPresenceEvent.Joins, matchPresenceEvent.Leaves);
                    break;
                case RoundState.Playing:
                    _playerFactory.SpawnPlayers(matchPresenceEvent.Joins, this);
                    _playerFactory.RemovePlayers(matchPresenceEvent.Leaves);
                    break;
            }
        }

        private void ReceivedMatchState(IMatchState matchState)
        {
            switch (matchState.OpCode)
            {
                case OpCodes.VelocityAndPosition:
                case OpCodes.Input:
                    _playerFactory.UpdatePlayersState(matchState);
                    break;
                case OpCodes.Died:
                    Debug.LogError("Got a died match state. Deleting session id : " + matchState.UserPresence);
                    var deadInfo = GetStateAsDictionary(matchState.State);
                    HandlePlayerDeath(deadInfo["deadPlayerSessionId"]);
                    break;
            }
        }
        
        /// <summary>
        /// Local player death handling
        /// </summary>
        private void OnPlayerDeath(IUserPresence userPresence)
        {
            _networkService.SendMatchState(OpCodes.Died, MatchDataJson.Died(userPresence.SessionId));
            HandlePlayerDeath(userPresence.SessionId);
        }

        private void HandlePlayerDeath(string sessionId)
        {
            var playerToRemove = _createdPlayers.First(p => p.SessionId == sessionId);
            
            _playerFactory.RemovePlayer(sessionId);
            _createdPlayers.Remove(playerToRemove);
            _waitingPlayers.Add(playerToRemove);
            CheckFinishRound();
        }

        private void TryStartRound(IEnumerable<IUserPresence> newUsers, IEnumerable<IUserPresence> leaveUsers)
        {
            AddPlayersToWaitList(newUsers);
            RemovePlayersFromWaitList(leaveUsers);

            if (EnoughPlayers())
            {
                StartRound();
            }
        }

        private void RemovePlayersFromWaitList(IEnumerable<IUserPresence> leaveUsers)
        {
            foreach (var user in leaveUsers)
            {
                if (_waitingPlayers.Contains(user))
                {
                    _waitingPlayers.Remove(user);
                }
                else
                {
                    Debug.LogError("There is no such user in waiting list");
                }
            }
        }

        private void StartRound()
        {
            Debug.LogError("StartRound");
            _playerFactory.SpawnPlayers(_waitingPlayers, this);
            _createdPlayers = new List<IUserPresence>(_waitingPlayers);
            _waitingPlayers.Clear();

            RoundState = RoundState.Playing;
        }

        private void CheckFinishRound()
        {
            Debug.LogError("CheckFinishRound");
            if (_playerFactory.PlayersCount == 1)
            {
                FinishRound();
            }
        }

        private async void FinishRound()
        {
            Debug.LogError("FinishRound");

            AnnounceWinner();
            await WaitAndRespawn();
        }

        private void AnnounceWinner()
        {
            IUserPresence winnerPresence = _createdPlayers.First();
            string winnerName = winnerPresence.Username;

            Debug.LogError("Player " + winnerName + " won the round");

            if (_playerDara.Username == winnerName)
            {
                _progressService.Progress.WinsCount++;
                Debug.LogError("WinsCount : " + _progressService.Progress.WinsCount);
            }
        }

        private async Task WaitAndRespawn()
        {
            RoundState = RoundState.WaitForPlayers;

            await Task.Delay(2000);
            
            _waitingPlayers.AddRange(_createdPlayers);
            _createdPlayers.Clear();
            
            _playerFactory.RemoveAllPlayers();
            
            if (EnoughPlayers())
                StartRound();
        }

        private void AddPlayersToWaitList(IEnumerable<IUserPresence> matchPresences)
        {
            _waitingPlayers.AddRange(matchPresences);
        }

        private bool EnoughPlayers()
        {
            Debug.LogError("EnoughPlayers. Count : " + _waitingPlayers.Count);
            return _waitingPlayers.Count >= MinPlayers;
        }
        
        private IDictionary<string, string> GetStateAsDictionary(byte[] state)
        {
            return Encoding.UTF8.GetString(state).FromJson<Dictionary<string, string>>();
        }
    }
}
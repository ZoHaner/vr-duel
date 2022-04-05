using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeBase.Behaviours.Player.Remote;
using CodeBase.NetworkAPI.Messaging;
using CodeBase.Services.Progress;
using CodeBase.Services.UI;
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

    public class MatchService : IRoundService
    {
        public Action<IUserPresence> PlayerDeath { get; set; }

        private const int StartRoundCheckDelay = 1000;
        private const int MinPlayers = 2;

        private readonly INetworkService _networkService;
        private readonly IProgressService _progressService;
        private readonly IPlayerFactory _playerFactory;
        private readonly IPlayerDataService _playerData;
        private readonly IWindowService _windowService;

        private readonly IPlayerDeathSender _playerDeathSender;
        
        private readonly List<IUserPresence> _waitingPlayers = new List<IUserPresence>();
        private List<IUserPresence> _createdPlayers = new List<IUserPresence>();

        private RoundState _roundState = RoundState.WaitForPlayers;
        private IMatch _currentMatch;

        public MatchService(INetworkService networkService, IPlayerFactory playerFactory, IProgressService progressService, IPlayerDataService playerData, IWindowService windowService, IPlayerDeathSender playerDeathSender)
        {
            _networkService = networkService;
            _playerFactory = playerFactory;
            _progressService = progressService;
            _playerData = playerData;
            _windowService = windowService;
            _playerDeathSender = playerDeathSender;
        }

        public void SubscribeEvents()
        {
            // _networkService.ReceivedMatchmakerMatched += CacheLocalUser;
            _networkService.MatchJoined += MatchJoined;
            _networkService.ReceivedMatchPresence += ReceivedMatchPresence;
            _networkService.ReceivedMatchState += ReceivedMatchState;

            PlayerDeath += OnPlayerDeath;
        }

        public void Cleanup()
        {
            // _networkService.ReceivedMatchmakerMatched -= CacheLocalUser;
            _networkService.MatchJoined -= MatchJoined;
            _networkService.ReceivedMatchPresence -= ReceivedMatchPresence;
            _networkService.ReceivedMatchState -= ReceivedMatchState;

            PlayerDeath -= OnPlayerDeath;

            RemoveAllPlayers();
            _waitingPlayers.Clear();
            _createdPlayers.Clear();
        }


        public async void CheckPlayersCountAndStartRound()
        {
            while (!EnoughPlayers())
            {
                if (_roundState != RoundState.WaitForPlayers)
                {
                    Debug.LogError($"RoundState should be in state WaitForPlayers, but it is - {_roundState}");
                    return;
                }

                await Task.Delay(StartRoundCheckDelay);
            }

            StartRound();
        }


        public void LeaveRound()
        {
            _networkService.LeaveMatch(_currentMatch.Id);
            _roundState = RoundState.Disable;
        }


        private void CacheLocalUser(string localUserId)
        {
            LocalUserId = localUserId;
        }

        private void MatchJoined(IMatch match)
        {
            CacheLocalUser(match.Self.UserId);
            // _roundState = RoundState.WaitForPlayers;
            _currentMatch = match;
            AddPlayersToWaitList(match.Presences);
            
            // switch (_roundState)
            // {
            //     case RoundState.Disable:
            //         AddPlayersToWaitList(match.Presences);
            //         break;
            //     case RoundState.WaitForPlayers:
            //         AddPlayersToWaitList(match.Presences);
            //         break;
            //     case RoundState.Playing:
            //         Debug.LogError("You shouldn't be here");
            //         break;
            // }
        }

        private void ReceivedMatchPresence(IMatchPresenceEvent matchPresenceEvent)
        {
            switch (_roundState)
            {
                case RoundState.Disable:
                    Debug.LogError("Match updated event, but RoundState = Disable");
                    break;
                case RoundState.WaitForPlayers:
                    UpdateWaitList(matchPresenceEvent.Joins, matchPresenceEvent.Leaves);
                    break;
                case RoundState.Playing:
                    SpawnPlayers(matchPresenceEvent.Joins);
                    RemovePlayers(matchPresenceEvent.Leaves);
                    break;
            }
        }

        private void ReceivedMatchState(IMatchState matchState)
        {
            switch (matchState.OpCode)
            {
                case OpCodes.VelocityAndPosition:
                case OpCodes.Input:
                    UpdatePlayersState(matchState);
                    break;
                case OpCodes.Died:
                    Debug.LogError("Got a died match state. Deleting session id : " + matchState.UserPresence);
                    var deadInfo = GetStateAsDictionary(matchState.State);
                    HandlePlayerDeath(deadInfo["deadPlayerId"]);
                    break;
            }
        }

        /// <summary>
        /// Local player death handling
        /// </summary>
        private void OnPlayerDeath(IUserPresence userPresence)
        {
            _playerDeathSender.SendMatchState(OpCodes.Died, MatchDataJson.Died(userPresence.UserId));
            HandlePlayerDeath(userPresence.UserId);
        }

        private void HandlePlayerDeath(string userId)
        {
            var playerToRemove = _createdPlayers.First(p => p.UserId == userId);

            DeactivatePlayer(userId);
            _createdPlayers.Remove(playerToRemove);
            AddPlayerToWaitList(playerToRemove);
            CheckFinishRound();
        }

        private void UpdateWaitList(IEnumerable<IUserPresence> newUsers, IEnumerable<IUserPresence> leaveUsers)
        {
            AddPlayersToWaitList(newUsers);
            RemovePlayersFromWaitList(leaveUsers);
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
            SpawnPlayers(_waitingPlayers);
            _createdPlayers = new List<IUserPresence>(_waitingPlayers);
            _waitingPlayers.Clear();

            _roundState = RoundState.Playing;
        }

        private void CheckFinishRound()
        {
            Debug.LogError("CheckFinishRound");
            if (PlayersCount == 1)
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

            if (_playerData.User.Username == winnerName)
            {
                _progressService.Progress.AddWin();
                _windowService.Open(WindowId.WinnerPopup);
                Debug.LogError($"You won {_progressService.Progress.WinsCount} times");
            }
            else
            {
                _windowService.Open(WindowId.LoosePopup);
                Debug.LogError($"You dead");
            }
        }

        private async Task WaitAndRespawn()
        {
            _roundState = RoundState.WaitForPlayers;

            await Task.Delay(2000);

            AddPlayersToWaitList(_createdPlayers);
            _createdPlayers.Clear();

            RemoveAllPlayers();
            _windowService.CloseAllWindows();

            if (EnoughPlayers())
                StartRound();
        }

        private void AddPlayersToWaitList(IEnumerable<IUserPresence> matchPresences)
        {
            foreach (var presence in matchPresences)
            {
                AddPlayerToWaitList(presence);
            }
        }

        private void AddPlayerToWaitList(IUserPresence presence)
        {
            if (!_waitingPlayers.Contains(presence))
                _waitingPlayers.Add(presence);
            else
                Debug.LogError($"Presence '{presence.Username}' is already in the waiting list!");
        }

        private bool EnoughPlayers()
        {
            Debug.LogError("EnoughPlayers? Count : " + _waitingPlayers.Count);
            return _waitingPlayers.Count >= MinPlayers;
        }

        private IDictionary<string, string> GetStateAsDictionary(byte[] state)
        {
            return Encoding.UTF8.GetString(state).FromJson<Dictionary<string, string>>();
        }
        
        // from factory
        
        
        
        private readonly Vector3 _gunPivotOffset = new Vector3(0.4f, 0.7f, 0f);
        Dictionary<string, GameObject> _players = new Dictionary<string, GameObject>();
        
        
        public int PlayersCount => _players.Count;
        public string LocalUserId { get; set; }
        
        
        public void SpawnPlayers(IEnumerable<IUserPresence> presences)
        {
            foreach (var presence in presences)
            {
                var isLocal = presence.UserId == LocalUserId;
                GameObject player;
                if (isLocal)
                {
                    player = _playerFactory.SpawnLocalNetworkPlayer(presence.UserId);
                }
                else
                {
                    player = _playerFactory.SpawnRemoteNetworkPlayer(presence, this);
                }
                
                _players.Add(presence.UserId, player);
            }
        }
        public void RemovePlayers(IEnumerable<IUserPresence> userPresences)
        {
            foreach (var presence in userPresences)
            {
                if (_players.ContainsKey(presence.UserId))
                {
                    RemovePlayer(presence.UserId);
                }
            }
        }

        public void RemovePlayer(string userId)
        {
            Debug.LogError("Remove player");
            var player = _players[userId];
            _players.Remove(userId);
            UnityEngine.Object.Destroy(player);
        }

        // Todo shouldn't destroy player
        public void DeactivatePlayer(string userId)
        {
            Debug.LogError("Deactivate Player");
            var player = _players[userId];
            _players.Remove(userId);
            UnityEngine.Object.Destroy(player, 1.5f);
        }

        public void UpdatePlayersState(IMatchState state)
        {
            string userId = state.UserPresence.UserId;

            if (!_players.ContainsKey(userId))
            {
                return;
            }

            if (IsRemotePlayer(userId))
            {
                _players[userId].GetComponent<RemotePlayerSync>().UpdateState(state);
            }
        }

        public void RemoveAllPlayers()
        {
            foreach (var player in _players)
            {
                UnityEngine.Object.Destroy(player.Value);
            }
            
            _players.Clear();
        }


        private bool IsRemotePlayer(string userId)
        {
            return userId != LocalUserId;
        }
    }
}
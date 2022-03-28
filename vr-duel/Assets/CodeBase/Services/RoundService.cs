using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeBase.Services.Progress;
using CodeBase.Services.UI;
using CodeBase.StaticData;
using CodeBase.Utilities.Network;
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

        private const int StartRoundCheckDelay = 1000;
        private const int MinPlayers = 2;

        private readonly INetworkService _networkService;
        private readonly INetworkPlayerFactory _playerFactory;
        private readonly IProgressService _progressService;
        private readonly IPlayerDataService _playerData;
        private readonly IWindowService _windowService;

        private readonly List<IUserPresence> _waitingPlayers = new List<IUserPresence>();
        private List<IUserPresence> _createdPlayers = new List<IUserPresence>();

        private RoundState _roundState = RoundState.Disable;
        private IMatch _currentMatch;

        public RoundService(INetworkService networkService, INetworkPlayerFactory playerFactory, IProgressService progressService, IPlayerDataService playerData, IWindowService windowService)
        {
            _networkService = networkService;
            _playerFactory = playerFactory;
            _progressService = progressService;
            _playerData = playerData;
            _windowService = windowService;
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


        private void CacheLocalUser(IMatchmakerMatched matched)
        {
            _playerFactory.LocalUserId = matched.Self.Presence.UserId;

            _roundState = RoundState.WaitForPlayers;
        }

        private void MatchJoined(IMatch match)
        {
            _currentMatch = match;
            switch (_roundState)
            {
                case RoundState.Disable:
                    AddPlayersToWaitList(match.Presences);
                    break;
                case RoundState.WaitForPlayers:
                    AddPlayersToWaitList(match.Presences);
                    break;
                case RoundState.Playing:
                    Debug.LogError("You shouldn't be here");
                    break;
            }
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
                    HandlePlayerDeath(deadInfo["deadPlayerId"]);
                    break;
            }
        }

        /// <summary>
        /// Local player death handling
        /// </summary>
        private void OnPlayerDeath(IUserPresence userPresence)
        {
            _networkService.SendMatchState(OpCodes.Died, MatchDataJson.Died(userPresence.UserId));
            HandlePlayerDeath(userPresence.UserId);
        }

        private void HandlePlayerDeath(string userId)
        {
            var playerToRemove = _createdPlayers.First(p => p.UserId == userId);

            _playerFactory.DeactivatePlayer(userId);
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
            _playerFactory.SpawnPlayers(_waitingPlayers, this);
            _createdPlayers = new List<IUserPresence>(_waitingPlayers);
            _waitingPlayers.Clear();

            _roundState = RoundState.Playing;
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

            _playerFactory.RemoveAllPlayers();
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
    }
}
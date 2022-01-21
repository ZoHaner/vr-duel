﻿using System;
using System.Threading.Tasks;
using CodeBase.Infrastructure;
using CodeBase.Network;
using CodeBase.Services.Network;
using Nakama;
using UnityEngine;

namespace CodeBase.Services
{
    public class NetworkService
    {
        private IClient _client;
        private ISession _session;
        private ISocket _socket;
        private string _ticket;
        
        public Action MatchJoined;

        public async Task Connect()
        {
            var configHolder = Resources.Load<NetworkConfigHolder>(AssetsPath.ConfigHolder);
            var config = configHolder.GetActiveConfig();
            _client = new Client(config.Scheme, config.Host, config.Port, config.ServerKey, UnityWebRequestAdapter.Instance);
            _session = await _client.AuthenticateDeviceAsync(SystemInfo.deviceUniqueIdentifier);
            _socket = _client.NewSocket();
            await _socket.ConnectAsync(_session, true);

            _socket.ReceivedMatchmakerMatched += OnReceivedMatchmakerMatched;
            
            Debug.Log(_session);
            Debug.Log(_socket);
        }

        public async Task FindMatch()
        {
            Debug.Log("Finding match ...");
            var matchtakingTicket = await _socket.AddMatchmakerAsync("*", 2, 2);
            _ticket = matchtakingTicket.Ticket;
            Debug.Log("Ticket : " + _ticket);
        }

        private async void OnReceivedMatchmakerMatched(IMatchmakerMatched matchmakerMatch)
        {
            Debug.Log("Match found!");
            var match = await _socket.JoinMatchAsync(matchmakerMatch);
            MainThreadDispatcher.Instance().Enqueue(() => MatchJoined?.Invoke());
            Debug.Log("Session id : " + match.Id);
        }

        public async Task Disconnect()
        {
            await _socket.CloseAsync();
        }
    }
}
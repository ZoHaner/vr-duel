using System.Threading.Tasks;
using CodeBase.Infrastructure;
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

        public async Task Connect()
        {
            var configHolder = Resources.Load<NetworkConfigHolder>(AssetsPath.ConfigHolder);
            var cfg = configHolder.GetActiveConfig();
            _client = new Client(cfg.Scheme, cfg.Host, cfg.Port, cfg.ServerKey, UnityWebRequestAdapter.Instance);
            _session = await _client.AuthenticateDeviceAsync(SystemInfo.deviceUniqueIdentifier);
            _socket = _client.NewSocket();
            await _socket.ConnectAsync(_session, true);
            
            Debug.Log(_session);
            Debug.Log(_socket);
        }
    }
}
using System.Collections.Generic;
using CodeBase.StaticData;
using CodeBase.UI.Services.Windows;
using System.Linq;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<WindowId, WindowConfig> _windowConfigs;
        private Dictionary<string, PlayerStaticData> _players;
        private const string StaticDataWindowsPath = "StaticData/UI/WindowStaticData";

        public void LoadStatics()
        {
            _windowConfigs = Resources
                .Load<WindowStaticData>(StaticDataWindowsPath)
                .Configs
                .ToDictionary(t => t.WindowId, t => t);

            _players = new Dictionary<string, PlayerStaticData>();
        }

        public WindowConfig ForWindow(WindowId windowId) =>
            _windowConfigs.TryGetValue(windowId, out var windowConfig)
                ? windowConfig
                : null;

        public PlayerStaticData ForPlayer(string name) => 
            _players.TryGetValue(name, out var playerData) ? playerData : null;

        
    }
}
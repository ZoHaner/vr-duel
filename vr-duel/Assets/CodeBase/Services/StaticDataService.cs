using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Services.UI;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Services
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<WindowId, WindowConfig> _windowConfigs;
        private const string StaticDataWindowsPath = "StaticData/UI/WindowStaticData";

        public void LoadStatics()
        {
            _windowConfigs = Resources
                .Load<WindowStaticData>(StaticDataWindowsPath)
                .Configs
                .ToDictionary(t => t.WindowId, t => t);
        }

        public WindowConfig ForWindow(WindowId windowId) =>
            _windowConfigs.TryGetValue(windowId, out var windowConfig)
                ? windowConfig
                : null;
    }
}
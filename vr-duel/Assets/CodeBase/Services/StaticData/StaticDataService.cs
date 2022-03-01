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
        private const string StaticDataWindowsPath = "StaticData/UI/WindowStaticData";

        public void LoadStatics() =>
            _windowConfigs = Resources
                .Load<WindowStaticData>(StaticDataWindowsPath)
                .Configs
                .ToDictionary(t => t.WindowId, t => t);

        public WindowConfig ForWindow(WindowId windowId) =>
            _windowConfigs.TryGetValue(windowId, out var windowConfig)
                ? windowConfig
                : null;
    }
}
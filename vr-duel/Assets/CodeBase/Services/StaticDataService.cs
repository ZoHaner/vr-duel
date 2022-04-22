using System.Collections.Generic;
using System.Linq;
using CodeBase.Entities;
using CodeBase.Services.UI;
using CodeBase.StaticData;

namespace CodeBase.Services
{
    public class StaticDataService : IStaticDataService
    {
        private const string StaticDataWindowsPath = "StaticData/UI/WindowStaticData";
        
        private readonly IAssetProvider _assetProvider;
        private Dictionary<WindowId, WindowConfig> _windowConfigs;

        public StaticDataService(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }
        
        public async void LoadStatics()
        {
            var staticData = await _assetProvider.Load<WindowStaticData>(StaticDataWindowsPath);
            _windowConfigs = staticData
                .Configs
                .ToDictionary(t => t.WindowId, t => t);
        }

        public WindowConfig ForWindow(WindowId windowId) =>
            _windowConfigs.TryGetValue(windowId, out var windowConfig)
                ? windowConfig
                : null;
    }
}
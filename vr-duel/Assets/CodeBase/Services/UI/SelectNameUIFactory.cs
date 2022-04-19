using System.Threading.Tasks;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.Services.UI
{
    public class SelectNameUIFactory : UIBaseFactory, ISelectNameUIFactory
    {
        protected override string UIRootPath => "UI/UIRoot";

        private readonly IStaticDataService _staticData;
        private readonly INameSelectorService _nameSelectorService;
        private readonly IPlayerAccountsService _playersAccountService;

        public SelectNameUIFactory(IStaticDataService staticData, INameSelectorService nameSelectorService, IPlayerAccountsService playersAccountService)
        {
            _staticData = staticData;
            _nameSelectorService = nameSelectorService;
            _playersAccountService = playersAccountService;
        }
        
        public async Task<GameObject> CreateChoosePlayerNameWindow(IWindowService windowService)
        {
            CreateRootIfNotExist();

            var config = _staticData.ForWindow(WindowId.ChoosePlayerName);
            var prefab = await config.PrefabReference.LoadAssetAsync().Task;
            var chooseNameWindow = InstantiateWindow(prefab).GetComponent<ChooseNameWindow>();
            chooseNameWindow.Construct(_nameSelectorService, _playersAccountService, windowService);
            return chooseNameWindow.gameObject;
        }

        public async Task<GameObject> CreateGeneratePlayerNameWindow(IWindowService windowService)
        {
            CreateRootIfNotExist();

            var config = _staticData.ForWindow(WindowId.GeneratePlayerName);
            var prefab = await config.PrefabReference.LoadAssetAsync().Task;
            var generateNameWindow = InstantiateWindow(prefab).GetComponent<GenerateNameWindow>();
            generateNameWindow.Construct(_nameSelectorService, windowService);

            return generateNameWindow.gameObject;
        }
    }
}
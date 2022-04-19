using System.Threading.Tasks;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.Services.UI
{
    public class SelectNameUIFactory : UIBaseFactory, ISelectNameUIFactory
    {
        protected override string UIRootPrefabPath => "UI/SelectName/UIRoot";

        private readonly IStaticDataService _staticData;
        private readonly INameSelectorService _nameSelectorService;
        private readonly IPlayerAccountsService _playersAccountService;

        public SelectNameUIFactory(IStaticDataService staticData, INameSelectorService nameSelectorService, IPlayerAccountsService playersAccountService, IAssetProvider assetProvider) : base(assetProvider)
        {
            _staticData = staticData;
            _nameSelectorService = nameSelectorService;
            _playersAccountService = playersAccountService;
        }
        
        public async Task<GameObject> CreateChoosePlayerNameWindow(IWindowService windowService)
        {
            await CreateRootIfNotExist();

            var config = _staticData.ForWindow(WindowId.ChoosePlayerName);
            var prefab = await config.PrefabReference.LoadAssetAsync().Task;
            var chooseNameWindow = InstantiateWindow(prefab).GetComponent<ChooseNameWindow>();
            chooseNameWindow.Construct(_nameSelectorService, _playersAccountService, windowService);
            return chooseNameWindow.gameObject;
        }

        public async Task<GameObject> CreateGeneratePlayerNameWindow(IWindowService windowService)
        {
            await CreateRootIfNotExist();

            var config = _staticData.ForWindow(WindowId.GeneratePlayerName);
            var prefab = await config.PrefabReference.LoadAssetAsync().Task;
            var generateNameWindow = InstantiateWindow(prefab).GetComponent<GenerateNameWindow>();
            generateNameWindow.Construct(_nameSelectorService, windowService);

            return generateNameWindow.gameObject;
        }
    }
}
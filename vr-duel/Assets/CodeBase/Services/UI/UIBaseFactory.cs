using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Services.UI
{
    public abstract class UIBaseFactory
    {
        protected abstract string UIRootPrefabId { get; }
        
        protected readonly IAssetProvider _assetProvider;
        private Transform _uiRoot;

        protected UIBaseFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        protected GameObject InstantiateWindow(GameObject configPrefab)
        {
            var window = Object.Instantiate(configPrefab, _uiRoot);
            window.gameObject.transform.localPosition = Vector3.zero;
            return window;
        }

        protected async Task CreateRootIfNotExist()
        {
            if (_uiRoot == null)
                await CreateUIRoot();
        }

        private async Task CreateUIRoot()
        {
            var prefab = await _assetProvider.Load<GameObject>(UIRootPrefabId);
            _uiRoot = Object.Instantiate(prefab).transform;
            _uiRoot.position += Vector3.up;
        }
    }
}
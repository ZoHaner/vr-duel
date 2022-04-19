using System.Threading.Tasks;
using CodeBase.Utilities;
using UnityEngine;

namespace CodeBase.Services.UI
{
    public abstract class UIBaseFactory
    {
        private readonly IAssetProvider _assetProvider;
        private Transform _uiRoot;

        protected UIBaseFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        protected abstract string UIRootPrefabPath { get; }

        protected async Task CreateRootIfNotExist()
        {
            if (_uiRoot == null)
                await CreateUIRoot();
        }
        
        protected GameObject InstantiateWindow(GameObject configPrefab)
        {
            var window = Object.Instantiate(configPrefab, _uiRoot);
            window.gameObject.transform.localPosition = Vector3.zero;
            return window;
        }

        private async Task CreateUIRoot()
        {
            var prefab = await _assetProvider.Load<GameObject>(UIRootPrefabPath);
            _uiRoot = Object.Instantiate(prefab).transform;
            _uiRoot.position += Vector3.up;
        }
    }
}
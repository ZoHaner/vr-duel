using CodeBase.Utilities;
using UnityEngine;

namespace CodeBase.Services.UI
{
    public abstract class UIBaseFactory
    {
        private Transform _uiRoot;
        protected abstract string UIRootPath { get; } // UIRootPrefabPath

        protected void CreateRootIfNotExist()
        {
            if (_uiRoot == null)
                CreateUIRoot();
        }
        
        protected GameObject InstantiateWindow(GameObject configPrefab)
        {
            var window = Object.Instantiate(configPrefab, _uiRoot);
            window.gameObject.transform.localPosition = Vector3.zero;
            return window;
        }

        private void CreateUIRoot()
        {
            _uiRoot = ResourcesUtilities.Instantiate(UIRootPath).transform;
            _uiRoot.position += Vector3.up;
        }
    }
}
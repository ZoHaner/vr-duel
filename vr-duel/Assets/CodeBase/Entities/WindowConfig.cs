using System;
using CodeBase.Services.UI;
using CodeBase.UI.Windows;
using UnityEngine.AddressableAssets;

namespace CodeBase.Entities
{
    [Serializable]
    public class WindowConfig
    {
        public WindowId WindowId;
        public AssetReferenceGameObject PrefabReference;
    }
}
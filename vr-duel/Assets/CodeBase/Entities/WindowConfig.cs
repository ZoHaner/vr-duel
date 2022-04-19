using System;
using CodeBase.Services.UI;
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
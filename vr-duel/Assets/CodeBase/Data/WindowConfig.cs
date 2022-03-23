using System;
using CodeBase.Services.UI;
using CodeBase.UI.Windows;

namespace CodeBase.Data
{
    [Serializable]
    public class WindowConfig
    {
        public WindowId WindowId;
        public WindowBase Prefab;
    }
}
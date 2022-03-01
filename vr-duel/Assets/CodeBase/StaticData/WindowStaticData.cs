using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "WindowStaticData", menuName = "StaticData/Window Static Data")]
    public class WindowStaticData : ScriptableObject
    {
        public List<WindowConfig> Configs;
    }
}
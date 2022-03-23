using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using UnityEngine;

namespace CodeBase.StaticData
{
    [Serializable]
    [CreateAssetMenu]
    public class NetworkConfigHolder : ScriptableObject
    {
        [SerializeField]
        private List<NetworkConfig> _configs;

        private void OnValidate()
        {
            if (_configs.Count(c => c.IsActive) > 1)
            {
                foreach (var config in _configs)
                {
                    config.IsActive = false;
                }
            }
        }

        public NetworkConfig GetActiveConfig()
        {
            return _configs.First(c => c.IsActive);
        } 
    }
}

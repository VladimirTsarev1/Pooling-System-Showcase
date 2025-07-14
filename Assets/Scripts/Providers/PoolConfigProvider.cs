using System.Collections.Generic;
using System.Linq;
using Showcase.Core;
using Showcase.PoolingSystem;
using UnityEngine;

namespace Showcase.Providers
{
    /// <summary>
    /// Provider for pool configuration access
    /// </summary>
    public class PoolConfigProvider
    {
        private readonly Dictionary<PoolType, PoolConfig> _configs;

        public PoolConfigProvider()
        {
            _configs = Resources.LoadAll<PoolConfig>(Constants.PoolsResourcesPath)
                .ToDictionary(info => info.PoolType);

#if UNITY_EDITOR
            ValidateConfigs();
#endif
        }

        public PoolConfig GetConfig(PoolType poolType)
        {
            if (_configs.TryGetValue(poolType, out var config))
            {
                return config;
            }

            Debug.LogError($"[PoolConfigProvider] Pool config not found for type: {poolType}");
            return null;
        }

        public IEnumerable<PoolType> GetAvailablePoolTypes() => _configs.Keys;

#if UNITY_EDITOR
        private void ValidateConfigs()
        {
            foreach (var (poolType, config) in _configs)
            {
                if (config.Prefab == null)
                {
                    Debug.LogError($"[PoolConfigProvider] Prefab is null for {poolType}", config);
                }
            }
        }
#endif
    }
}
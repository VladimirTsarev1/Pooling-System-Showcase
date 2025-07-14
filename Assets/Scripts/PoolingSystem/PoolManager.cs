using System.Collections.Generic;
using Showcase.Providers;
using Showcase.Singleton;
using UnityEngine;

namespace Showcase.PoolingSystem
{
    /// <summary>
    /// Central manager for all object pools
    /// </summary>
    public class PoolManager : BaseSingleton<PoolManager>
    {
        private readonly Dictionary<PoolType, Pool> _pools = new Dictionary<PoolType, Pool>();
        private Transform _poolsContainer;

        protected override void OnSingletonAwake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public static void Initialize()
        {
            if (Instance == null)
            {
                Debug.LogError("[PoolManager] Instance not found for initialization!");
                return;
            }

            Instance.InitializePools();
        }

        public T GetPoolItem<T>(PoolType poolType, Vector3 position, Quaternion rotation)
            where T : Component
        {
            if (!_pools.TryGetValue(poolType, out var pool))
            {
                Debug.LogError($"[PoolManager] Pool not found for type: {poolType}");
                return null;
            }

            var item = pool.Get<T>();
            if (item != null)
            {
                SetupPooledObject(item, position, rotation);
            }

            return item;
        }

        private void InitializePools()
        {
            Debug.Log("[PoolManager] Initializing pools...");

            CreatePoolsContainer();

            var availablePoolTypes = ConfigProvider.Pools.GetAvailablePoolTypes();
            foreach (var poolType in availablePoolTypes)
            {
                CreatePool(poolType);
            }

            Debug.Log($"[PoolManager] Initialized {_pools.Count} pools");
        }

        private void CreatePoolsContainer()
        {
            var poolsObject = new GameObject("Pools");
            _poolsContainer = poolsObject.transform;
            DontDestroyOnLoad(poolsObject);
        }

        private void CreatePool(PoolType poolType)
        {
            if (_pools.ContainsKey(poolType))
            {
                Debug.LogWarning($"[PoolManager] Pool for type '{poolType}' already exists! Skipping creation.");
                return;
            }

            var config = ConfigProvider.Pools.GetConfig(poolType);
            if (config == null)
            {
                return;
            }

            var holderObject = new GameObject($"{poolType}Pool");
            holderObject.transform.SetParent(_poolsContainer);

            var pool = new Pool(config, holderObject.transform);
            pool.Prewarm(config.InitialSize);

            _pools[poolType] = pool;
            Debug.Log($"[PoolManager] Created pool for {poolType} with {config.InitialSize} pre-warmed objects");
        }

        private void SetupPooledObject<T>(T item, Vector3 position, Quaternion rotation)
            where T : Component
        {
            item.transform.position = position;
            item.transform.rotation = rotation;
        }
    }
}
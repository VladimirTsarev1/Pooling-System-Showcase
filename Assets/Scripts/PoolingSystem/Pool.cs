using System.Collections.Generic;
using UnityEngine;

namespace Showcase.PoolingSystem
{
    /// <summary>
    /// Generic object pool implementation
    /// </summary>
    public class Pool
    {
        private readonly Queue<PooledObject> _pool = new Queue<PooledObject>();
        private readonly PoolConfig _config;
        private readonly Transform _parent;

        public Pool(PoolConfig config, Transform parent = null)
        {
            _config = config;
            _parent = parent;
        }

        public T Get<T>() where T : Component
        {
            PooledObject pooledObject;

            if (_pool.Count > 0)
            {
                pooledObject = _pool.Dequeue();
            }
            else
            {
                pooledObject = CreateNewObject();
            }

            pooledObject.OnGet();
            
            pooledObject.gameObject.SetActive(true);
            return pooledObject.GetComponent<T>();
        }

        public void Return(PooledObject item)
        {
            if (item == null)
            {
                Debug.LogWarning("[Pool] Trying to return null object to pool");
                return;
            }

            item.gameObject.SetActive(false);
            _pool.Enqueue(item);
        }

        public void Prewarm(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var obj = CreateNewObject();
                obj.gameObject.SetActive(false);
                _pool.Enqueue(obj);
            }
        }

        private PooledObject CreateNewObject()
        {
            var instance = Object.Instantiate(_config.Prefab, _parent);

            instance.Initialize(_config);
            instance.OnReturnToPool += Return;

            return instance;
        }
    }
}
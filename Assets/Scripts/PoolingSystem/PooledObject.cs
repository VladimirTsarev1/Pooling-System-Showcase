using System;
using Showcase.OutOfBounds;
using UnityEngine;

namespace Showcase.PoolingSystem
{
    /// <summary>
    /// Base class for pooled objects with auto-return logic
    /// </summary>
    public class PooledObject : MonoBehaviour, IOutOfBounds
    {
        private PoolConfig _config;
        private bool _isActiveInPool;

        public event Action<PooledObject> OnReturnToPool;

        private void OnDisable()
        {
            if (_config?.ReturnCondition == PoolObjectReturnCondition.Disable && _isActiveInPool)
            {
                ReturnToPool();
            }
        }

        private void OnDestroy()
        {
            OnReturnToPool = null;
        }

        public void Initialize(PoolConfig config)
        {
            _config = config;
        }

        public virtual void OnGet()
        {
            _isActiveInPool = true;
            SetupAutoReturn();
        }

        protected virtual void ReturnToPool()
        {
            if (!_isActiveInPool)
            {
                return;
            }

            _isActiveInPool = false;
            CancelInvoke(nameof(ReturnToPool));

            OnReturnToPool?.Invoke(this);
        }

        private void SetupAutoReturn()
        {
            if (_config?.ReturnCondition == PoolObjectReturnCondition.Timer)
            {
                Invoke(nameof(ReturnToPool), _config.ReturnTime);
            }
        }

        public virtual void OnOutOfBounds()
        {
            if (_config?.ReturnCondition == PoolObjectReturnCondition.Timer)
            {
                CancelInvoke(nameof(ReturnToPool));
            }
            
            ReturnToPool();
        }
    }
}
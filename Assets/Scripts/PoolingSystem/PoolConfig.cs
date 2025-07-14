using UnityEngine;

namespace Showcase.PoolingSystem
{
    [CreateAssetMenu(fileName = "New Pool Config", menuName = "Pooling System/Pool Config")]
    public class PoolConfig : ScriptableObject
    {
        [SerializeField] private PoolType poolType;
        [SerializeField] private PooledObject prefab;
        [SerializeField] private int initialSize;

        [SerializeField] private PoolObjectReturnCondition returnCondition = PoolObjectReturnCondition.Timer;
        [SerializeField] private float returnTime;

        public PoolType PoolType => poolType;
        public PooledObject Prefab => prefab;
        public int InitialSize => initialSize;
        public PoolObjectReturnCondition ReturnCondition => returnCondition;
        public float ReturnTime => returnTime;
    }
}
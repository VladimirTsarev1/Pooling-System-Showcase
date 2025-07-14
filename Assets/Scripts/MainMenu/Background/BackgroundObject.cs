using Showcase.PoolingSystem;
using UnityEngine;

namespace Showcase.MainMenu.Background
{
    [RequireComponent(typeof(Rigidbody))]
    public class BackgroundObject : PooledObject
    {
        [SerializeField] private Rigidbody rigidbodyComponent;

        public void Setup(Vector3 newVelocity)
        {
            rigidbodyComponent.angularVelocity = Vector3.zero;
            
            rigidbodyComponent.velocity = newVelocity;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!rigidbodyComponent)
            {
                rigidbodyComponent = GetComponent<Rigidbody>();
            }
        }
#endif
    }
}
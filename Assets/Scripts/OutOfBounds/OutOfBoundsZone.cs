using UnityEngine;

namespace Showcase.OutOfBounds
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class OutOfBoundsZone : MonoBehaviour
    {
        private BoxCollider _boxCollider;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IOutOfBounds>(out var boundaryObject))
            {
                boundaryObject.OnOutOfBounds();
            }
        }

        public void SetBoxCollierSize(Vector3 size)
        {
            _boxCollider.size = size;
        }

        public void SetBoxCollierSize(float xSize = 1f, float ySize = 1f, float zSize = 1f)
        {
            var newSize = new Vector3(xSize, ySize, zSize);

            _boxCollider.size = newSize;
        }
    }
}
using System.Collections;
using Showcase.PoolingSystem;
using UnityEngine;

namespace Showcase.MainMenu.Background
{
    public class BackgroundObjectSpawner : MonoBehaviour
    {
        [SerializeField] private Vector2 cameraViewMinPosition;
        [SerializeField] private Vector2 cameraViewMaxPosition;
        [SerializeField] private Vector2 spawnIntervalRange = new(0.5f, 2f);
        [SerializeField] private PoolType[] projectileTypes;
        [SerializeField] private Vector2 forceRange = new(5f, 15f);
        [SerializeField] private Vector3 direction = Vector3.down;

        private Vector3 _minPositionForSpawn;
        private Vector3 _maxPositionForSpawn;

        public void StartSpawn(Camera cameraComponent, float distanceFromCamera)
        {
            _minPositionForSpawn = cameraComponent.ViewportToWorldPoint(new Vector3(cameraViewMinPosition.x,
                cameraViewMinPosition.y, distanceFromCamera));

            _maxPositionForSpawn = cameraComponent.ViewportToWorldPoint(new Vector3(cameraViewMaxPosition.x,
                cameraViewMaxPosition.y, distanceFromCamera));

            StartCoroutine(SpawnObjectsLoop());
        }

        private IEnumerator SpawnObjectsLoop()
        {
            while (true)
            {
                var randomPoint = Vector3.Lerp(_minPositionForSpawn, _maxPositionForSpawn, Random.value);

                var velocity = direction.normalized * Random.Range(forceRange.x, forceRange.y);

                PoolManager.Instance.GetPoolItem<BackgroundObject>(
                        projectileTypes[Random.Range(0, projectileTypes.Length)], randomPoint,
                        Quaternion.identity)
                    .Setup(velocity);

                yield return new WaitForSeconds(Random.Range(spawnIntervalRange.x, spawnIntervalRange.y));
            }
        }
    }
}
using Showcase.OutOfBounds;
using UnityEngine;

namespace Showcase.MainMenu.Background
{
    public class BackgroundController : MonoBehaviour
    {
        [SerializeField] private BackgroundObjectSpawner[] backgroundObjectSpawners;
        [SerializeField] private OutOfBoundsZone outOfBoundsZonePrefab;

        [SerializeField] private float distanceFromCamera;
        [SerializeField] private float additionalDistanceForBounds;

        public void ActivateBackground()
        {
            var cameraComponent = Camera.main;

            if (!cameraComponent)
            {
                Debug.LogWarning(
                    "[BackgroundBoundZonesController] There is no Main Camera on the scene, can't spawn bounds!", this);

                return;
            }

            SpawnBounds(cameraComponent);

            foreach (var spawner in backgroundObjectSpawners)
            {
                spawner.StartSpawn(cameraComponent, distanceFromCamera);
            }
        }

        private void SpawnBounds(Camera cameraComponent)
        {
            var lowerLeftCornerPosition = cameraComponent.ViewportToWorldPoint(new Vector3(0, 0, distanceFromCamera));
            var upperLeftCornerPosition = cameraComponent.ViewportToWorldPoint(new Vector3(0, 1, distanceFromCamera));
            var lowerRightCornerPosition = cameraComponent.ViewportToWorldPoint(new Vector3(1, 0, distanceFromCamera));
            var upperRightCornerPosition = cameraComponent.ViewportToWorldPoint(new Vector3(1, 1, distanceFromCamera));

            var additionalVerticalDistance = Vector3.up * additionalDistanceForBounds;
            var additionalHorizontalDistance = Vector3.right * additionalDistanceForBounds;

            var bottomBoundSpawnPosition =
                (lowerLeftCornerPosition + lowerRightCornerPosition - additionalVerticalDistance) * 0.5f;
            var topBoundSpawnPosition =
                (upperLeftCornerPosition + upperRightCornerPosition + additionalVerticalDistance) * 0.5f;

            var leftBoundSpawnPosition =
                (lowerLeftCornerPosition + upperLeftCornerPosition - additionalHorizontalDistance) * 0.5f;
            var rightBoundSpawnPosition =
                (lowerRightCornerPosition + upperRightCornerPosition + additionalHorizontalDistance) * 0.5f;

            var bottomBoundZone = Instantiate(outOfBoundsZonePrefab, bottomBoundSpawnPosition, Quaternion.identity);
            var topBoundZone = Instantiate(outOfBoundsZonePrefab, topBoundSpawnPosition, Quaternion.identity);
            var leftBoundZone = Instantiate(outOfBoundsZonePrefab, leftBoundSpawnPosition, Quaternion.identity);
            var rightBoundZone = Instantiate(outOfBoundsZonePrefab, rightBoundSpawnPosition, Quaternion.identity);

            var horizontalSize = (rightBoundZone.transform.position - leftBoundZone.transform.position).x;
            var verticalSize = (topBoundZone.transform.position - bottomBoundZone.transform.position).y;

            bottomBoundZone.SetBoxCollierSize(xSize: horizontalSize);
            topBoundZone.SetBoxCollierSize(xSize: horizontalSize);
            leftBoundZone.SetBoxCollierSize(ySize: verticalSize);
            rightBoundZone.SetBoxCollierSize(ySize: verticalSize);
        }
    }
}
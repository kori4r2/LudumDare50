using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50 {
    [System.Serializable]
    public class StarPlacementCalculator {
        [SerializeField] private Camera mainCamera;
        [SerializeField, Range(0f, 50f)] private float spaceBetweenStars = 25f;
        private RuntimeSet<Star> stars;
        private Rect spawnArea = Rect.zero;
        private IStarSpreader starSpreader;
        public int MaxNStars { get; private set; }
        private const int maxLoopSteps = 50;

        public void Setup(int starPoolSize, RuntimeSet<Star> starsRuntimeSet) {
            stars = starsRuntimeSet;
            UpdateSpawnArea();
            CalculateMaxStars(starPoolSize);
            starSpreader = new SpreadFromRandomCircle();
            starSpreader.Setup(spawnArea);
        }

        private void UpdateSpawnArea() {
            Vector2 halfCameraSize = CameraUtils.GetWorldSpaceCameraSize(mainCamera) / 2f;
            spawnArea.min = new Vector2(-halfCameraSize.x, 0f);
            spawnArea.max = new Vector2(halfCameraSize.x, halfCameraSize.y);
        }

        private void CalculateMaxStars(int starPoolSize) {
            float availableArea = spawnArea.width * spawnArea.height;
            float starRadius = spaceBetweenStars / 2f;
            float spacePerStar = Mathf.Pow(starRadius, 2f);
            MaxNStars = Mathf.Min(Mathf.FloorToInt(availableArea / spacePerStar), starPoolSize);
        }

        public Vector3 GetNextStarPosition() {
            Vector3 selectedPosition = Vector3.zero;
            float maxDistance = 0f;
            for (int step = 0; step < maxLoopSteps; step++) {
                Vector3 rolledPosition = starSpreader.GetNewPosition();
                float distanceToClosestStar = CheckDistanceToStars(rolledPosition);
                if (distanceToClosestStar < 0)
                    return rolledPosition;

                if (distanceToClosestStar <= maxDistance)
                    continue;

                selectedPosition = rolledPosition;
                maxDistance = distanceToClosestStar;
            }
            return selectedPosition;
        }

        private float CheckDistanceToStars(Vector3 rolledPosition) {
            float minDistance = float.MaxValue;
            bool hasCloseStar = false;
            foreach (Star star in stars.ToArray()) {
                float distance = Vector2.Distance(rolledPosition, star.transform.position);
                if(distance >= spaceBetweenStars)
                    continue;

				hasCloseStar = true;
                minDistance = Mathf.Min(distance, minDistance);
            }
            return hasCloseStar? minDistance : -1f;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50 {
    [System.Serializable]
    public class StarPlacementCalculator {
        [SerializeField] private Camera mainCamera;
        [SerializeField, Range(0f, 50f)] private float spaceBetweenStars = 25f;
        [SerializeField, Range(1f, 2f)] private float emptySpaceMultiplier = 1f;
        private Rect spawnArea = Rect.zero;
        public int MaxNStars { get; private set; } = 5;

        public void Setup(int starPoolSize) {
            UpdateSpawnArea();
            MaxNStars = Mathf.Min(MaxNStars, starPoolSize);
        }

        private void UpdateSpawnArea() {
            Vector2 halfCameraSize = CameraUtils.GetWorldSpaceCameraSize(mainCamera) / 2f;
            spawnArea.min = new Vector2(-halfCameraSize.x, 0f);
            spawnArea.max = new Vector2(halfCameraSize.x, halfCameraSize.y);
        }

        public Vector3 GetNextStarPosition() {
            return Vector3.zero;
        }
    }
}

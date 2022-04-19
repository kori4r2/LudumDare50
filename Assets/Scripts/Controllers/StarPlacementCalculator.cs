using UnityEngine;
using UnityEngine.Events;

namespace LudumDare50 {
    [System.Serializable]
    public class StarPlacementCalculator {
        [SerializeField] private Camera mainCamera;
        public Camera MainCamera => mainCamera;
        private float spaceBetweenStars;
        private RuntimeSet<Star> stars;
        private Rect spawnArea = Rect.zero;
        public Rect SpawnArea => spawnArea;
        private const float borderSize = 2f;
        private IStarSpreader starSpreader;
        public int MaxNStars { get; private set; }
        private const int maxLoopSteps = 50;
        public UnityEvent OnCameraChange;

        public void Setup(ActiveGameSettingsReference gameSettings, int starPoolSize, RuntimeSet<Star> starsRuntimeSet) {
            stars = starsRuntimeSet;
            spaceBetweenStars = gameSettings.SpaceBetweenStars;
            UpdateSpawnArea();
            CalculateMaxStars(starPoolSize);
            SetupCameraChangeCallbacks(starPoolSize);
            starSpreader = new SpreadFromRandomCircle();
            starSpreader.Setup(spawnArea);
        }

        private void UpdateSpawnArea() {
            Vector2 halfCameraSize = CameraUtils.GetWorldSpaceCameraSize(mainCamera) / 2f;
            spawnArea.min = new Vector2(-halfCameraSize.x, 0f);
            spawnArea.max = new Vector2(halfCameraSize.x, halfCameraSize.y);
            spawnArea.width -= borderSize;
            spawnArea.height -= borderSize;
        }

        private void CalculateMaxStars(int starPoolSize) {
            float availableArea = spawnArea.width * spawnArea.height;
            float starRadius = spaceBetweenStars / 2f;
            float spacePerStar = Mathf.Pow(starRadius, 2f);
            MaxNStars = Mathf.Clamp(Mathf.FloorToInt(availableArea / spacePerStar), 2, starPoolSize);
        }

        private void SetupCameraChangeCallbacks(int starPoolSize) {
            OnCameraChange = new UnityEvent();
            OnCameraChange.AddListener(UpdateSpawnArea);
            OnCameraChange.AddListener(() => CalculateMaxStars(starPoolSize));
            OnCameraChange.AddListener(() => starSpreader.Setup(spawnArea));
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
                if (distance >= spaceBetweenStars)
                    continue;

                hasCloseStar = true;
                minDistance = Mathf.Min(distance, minDistance);
            }
            return hasCloseStar ? minDistance : -1f;
        }
    }
}

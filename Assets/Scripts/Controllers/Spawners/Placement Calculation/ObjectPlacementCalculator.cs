using UnityEngine;
using Toblerone.Toolbox;

namespace LudumDare50 {
    [System.Serializable]
    public abstract class ObjectPlacementCalculator<T> where T : MonoBehaviour {
        private const float borderSize = 2f;
        private const int maxLoopSteps = 50;
        protected virtual int MinNObjects => 2;
        [SerializeField] private Camera mainCamera;
        public Camera MainCamera => mainCamera;
        protected float spaceBetweenObjects;
        private RuntimeSet<T> objects;
        private Rect spawnArea = Rect.zero;
        public Rect SpawnArea => spawnArea;
        private IObjectSpreader objectSpreader;
        public int MaxNObjects { get; private set; }
        private int poolSize;

        public abstract void Setup(ActiveGameSettingsReference gameSettings, int poolSize, RuntimeSet<T> runtimeSet);

        protected void Setup(int poolSize, RuntimeSet<T> runtimeSet) {
            objects = runtimeSet;
            this.poolSize = poolSize;
            UpdateSpawnArea();
            CalculateMaxObjects();
            objectSpreader = new SpreadFromRandomCircle();
            objectSpreader.Setup(spawnArea);
        }

        private void UpdateSpawnArea() {
            Vector2 halfCameraSize = CameraUtils.GetWorldSpaceCameraSize(mainCamera) / 2f;
            spawnArea.min = new Vector2(-halfCameraSize.x, 0f);
            spawnArea.max = new Vector2(halfCameraSize.x, halfCameraSize.y);
            spawnArea.width -= borderSize;
            spawnArea.height -= borderSize;
        }

        private void CalculateMaxObjects() {
            float availableArea = spawnArea.width * spawnArea.height;
            float objectRadius = spaceBetweenObjects / 2f;
            float spacePerObject = Mathf.Pow(objectRadius, 2f);
            MaxNObjects = Mathf.Clamp(Mathf.FloorToInt(availableArea / spacePerObject), MinNObjects, poolSize);
        }

        public void OnCameraChanged() {
            UpdateSpawnArea();
            CalculateMaxObjects();
            objectSpreader.Setup(spawnArea);
        }

        public Vector3 GetNextObjectPosition() {
            Vector3 selectedPosition = Vector3.zero;
            float maxDistance = 0f;
            for (int step = 0; step < maxLoopSteps; step++) {
                Vector3 rolledPosition = objectSpreader.GetNewPosition();
                float distanceToClosestObject = CheckDistanceToObjects(rolledPosition);
                if (distanceToClosestObject < 0)
                    return rolledPosition;

                if (distanceToClosestObject <= maxDistance)
                    continue;

                selectedPosition = rolledPosition;
                maxDistance = distanceToClosestObject;
            }
            return selectedPosition;
        }

        private float CheckDistanceToObjects(Vector3 rolledPosition) {
            float minDistance = float.MaxValue;
            bool hasCloseObject = false;
            foreach (T obj in objects) {
                float distance = Vector2.Distance(rolledPosition, obj.transform.position);
                if (distance >= spaceBetweenObjects)
                    continue;

                hasCloseObject = true;
                minDistance = Mathf.Min(distance, minDistance);
            }
            return hasCloseObject ? minDistance : -1f;
        }
    }
}

using UnityEngine;

namespace LudumDare50 {
    public class StarSpawner : MonoBehaviour {
        [SerializeField] private ActiveGameSettingsReference gameSettings;
        private float RespawnTime => gameSettings.StarsRespawnTime;
        private Timer respawnTimer;
        [SerializeField] private StarPool starPool;
        [SerializeField] private StarPlacementCalculator placementCalculator;
        [SerializeField] private StarEvent starDespawnedEvent;
        private GenericEventListener<Star> starDespawnedEventListener;
        [SerializeField] private BoolVariable isPlaying;
        private VariableObserver<bool> isPlayingObserver;
        [SerializeField] private RuntimeSet<Star> starsSpawned;
        private int StarCount => starsSpawned.Count;
        private bool CanSpawnStar => StarCount < placementCalculator.MaxNStars;
        [SerializeField] private EventSO cameraChangeEvent;
        private EventListener cameraChangeEventListener;

        private void Awake() {
            respawnTimer = new Timer(RespawnTime);
            placementCalculator.Setup(gameSettings, starPool.PoolSize, starsSpawned);
            CreateObserversAndListeners();
        }

        private void CreateObserversAndListeners() {
            isPlayingObserver = new VariableObserver<bool>(isPlaying, OnGameStateChanged);
            starDespawnedEventListener = new GenericEventListener<Star>(starDespawnedEvent, RemoveStarAndCheckRespawn);
            cameraChangeEventListener = new EventListener(cameraChangeEvent, OnCameraChanged);
        }

        private void OnGameStateChanged(bool newIsPlaying) {
            if (newIsPlaying) {
                SpawnAllStars();
            } else {
                ReturnAllStarsToPool();
                respawnTimer.StopTimer();
            }
        }

        private void SpawnAllStars() {
            while (CanSpawnStar) {
                SpawnNewStar();
            }
        }

        private void SpawnNewStar() {
            if (!CanSpawnStar)
                return;

            Vector3 newPosition = placementCalculator.GetNextStarPosition();
            starPool.InstantiateStar(newPosition, Quaternion.identity);
        }

        private void ReturnAllStarsToPool() {
            foreach (Star star in starsSpawned.ToArray()) {
                starPool.ReturnStarToPool(star);
            }
        }

        private void RemoveStarAndCheckRespawn(Star starDespawned) {
            CheckForRespawn();
        }

        private void CheckForRespawn() {
            if (CanSpawnStar && respawnTimer.IsDone) {
                respawnTimer.StartTimer();
            }
        }

        private void OnCameraChanged() {
            placementCalculator.OnCameraChange?.Invoke();
            RemoveStarsOutsideSpawnArea();
            RemoveStarsAboveMaxNumber();
        }

        private void RemoveStarsOutsideSpawnArea() {
            Rect spawnArea = placementCalculator.SpawnArea;
            foreach (Star star in starsSpawned.ToArray()) {
                if (CameraUtils.IsInsideLimits(star.transform.position, spawnArea.min, spawnArea.max))
                    continue;

                starPool.ReturnStarToPool(star);
            }
        }

        private void RemoveStarsAboveMaxNumber() {
            if (StarCount <= placementCalculator.MaxNStars)
                return;

            foreach (Star star in starsSpawned.ToArray()) {
                starPool.ReturnStarToPool(star);
                if (StarCount <= placementCalculator.MaxNStars)
                    return;
            }
        }

        private void Update() {
            if (!isPlaying.Value || !CanSpawnStar)
                return;

            if (!respawnTimer.IsDone) {
                respawnTimer.UpdateTimer(Time.deltaTime);
                return;
            }

            SpawnNewStar();
            CheckForRespawn();
        }

        private void OnEnable() {
            isPlayingObserver.StartWatching();
            starDespawnedEventListener.StartListeningEvent();
            cameraChangeEventListener.StartListeningEvent();
        }

        private void OnDisable() {
            isPlayingObserver.StopWatching();
            starDespawnedEventListener.StopListeningEvent();
            cameraChangeEventListener.StopListeningEvent();
        }
    }
}

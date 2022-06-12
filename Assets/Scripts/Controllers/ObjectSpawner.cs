using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toblerone.Toolbox;

namespace LudumDare50 {
    public abstract class ObjectSpawner<T> : MonoBehaviour where T : MonoBehaviour, IPoolableObject {
        [SerializeField] protected ActiveGameSettingsReference gameSettings;
        protected abstract ObjectPlacementCalculator<T> PlacementCalculator { get; }
        protected abstract float RespawnTime { get; }
        private Timer respawnTimer;
        protected abstract ObjectPool<T> ObjectPool { get; }
        protected abstract RuntimeSet<T> ObjectsSpawned { get; }
        protected abstract EventSO UpdateObjectSpawnerEvent { get; }
        private EventListener updateObjectSpawnerEventListener;
        [SerializeField] private BoolVariable isPlaying;
        private VariableObserver<bool> isPlayingObserver;
        private int ObjectCount => ObjectsSpawned.Count;
        protected bool CanSpawnObject => ObjectCount < PlacementCalculator.MaxNObjects;
        [SerializeField] private EventSO cameraChangeEvent;
        private EventListener cameraChangeEventListener;

        private void Awake() {
            respawnTimer = new Timer(RespawnTime);
            PlacementCalculator.Setup(gameSettings, ObjectPool.PoolSize, ObjectsSpawned);
            CreateObserversAndListeners();
        }

        private void CreateObserversAndListeners() {
            isPlayingObserver = new VariableObserver<bool>(isPlaying, OnGameStateChanged);
            updateObjectSpawnerEventListener = new EventListener(UpdateObjectSpawnerEvent, RemoveObjectAndCheckRespawn);
            cameraChangeEventListener = new EventListener(cameraChangeEvent, OnCameraChanged);
        }

        private void OnGameStateChanged(bool newIsPlaying) {
            if (newIsPlaying) {
                SpawnInitialObjects();
                if (CanSpawnObject)
                    respawnTimer.StartTimer();
            } else {
                ReturnAllObjectsToPool();
                respawnTimer.StopTimer();
            }
        }

        protected abstract void SpawnInitialObjects();

        protected void SpawnNewObject() {
            if (!CanSpawnObject)
                return;

            Vector3 newPosition = PlacementCalculator.GetNextObjectPosition();
            ObjectPool.InstantiateObject(newPosition, Quaternion.identity);
        }

        private void ReturnAllObjectsToPool() {
            foreach (T obj in ObjectsSpawned.ToArray()) {
                ObjectPool.ReturnObjectToPool(obj);
            }
        }

        private void RemoveObjectAndCheckRespawn() {
            CheckForRespawn();
        }

        private void CheckForRespawn() {
            if (CanSpawnObject && respawnTimer.IsDone) {
                respawnTimer.StartTimer();
            }
        }

        private void OnCameraChanged() {
            PlacementCalculator.OnCameraChange?.Invoke();
            RemoveObjectsOutsideSpawnArea();
            RemoveObjectsAboveMaxNumber();
        }

        private void RemoveObjectsOutsideSpawnArea() {
            Rect spawnArea = PlacementCalculator.SpawnArea;
            foreach (T obj in ObjectsSpawned.ToArray()) {
                if (CameraUtils.IsInsideLimits(obj.transform.position, spawnArea.min, spawnArea.max))
                    continue;

                ObjectPool.ReturnObjectToPool(obj);
            }
        }

        private void RemoveObjectsAboveMaxNumber() {
            if (ObjectCount <= PlacementCalculator.MaxNObjects)
                return;

            foreach (T obj in ObjectsSpawned.ToArray()) {
                ObjectPool.ReturnObjectToPool(obj);
                if (ObjectCount <= PlacementCalculator.MaxNObjects)
                    return;
            }
        }

        private void Update() {
            if (!isPlaying.Value || !CanSpawnObject)
                return;

            if (!respawnTimer.IsDone) {
                respawnTimer.UpdateTimer(Time.deltaTime);
                return;
            }

            SpawnNewObject();
            CheckForRespawn();
        }

        private void OnEnable() {
            isPlayingObserver.StartWatching();
            updateObjectSpawnerEventListener.StartListeningEvent();
            cameraChangeEventListener.StartListeningEvent();
        }

        private void OnDisable() {
            isPlayingObserver.StopWatching();
            updateObjectSpawnerEventListener.StopListeningEvent();
            cameraChangeEventListener.StopListeningEvent();
        }
    }
}

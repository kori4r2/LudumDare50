using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50 {
    public class StarSpawner : MonoBehaviour {
        [SerializeField] private float respawnTime;
        private Timer respawnTimer;
        [SerializeField] private StarPool starPool;
        [SerializeField] private StarPlacementCalculator placementCalculator;
        [SerializeField] private StarEvent starDespawnedEvent;
        private GenericEventListener<Star> starDespawnedEventListener;
        [SerializeField] private BoolVariable isPlaying;
        private VariableObserver<bool> isPlayingObserver;
        private HashSet<Star> starsSpawned = new HashSet<Star>();
        private int StarCount => starsSpawned.Count;
        private bool CanSpawnStar => StarCount < placementCalculator.MaxNStars;

        private void Awake() {
            respawnTimer = new Timer(respawnTime);
            placementCalculator.Setup(starPool.PoolSize);
            isPlayingObserver = new VariableObserver<bool>(isPlaying, OnGameStateChanged);
            starDespawnedEventListener = new GenericEventListener<Star>(starDespawnedEvent, RemoveStarAndCheckRespawn);
        }

        private void OnGameStateChanged(bool newIsPlaying) {
            if(newIsPlaying) {
                SpawnAllStars();
            } else {
                ReturnAllStarsToPool();
                respawnTimer.StopTimer();
            }
        }

        private void SpawnAllStars() {
            while(CanSpawnStar) {
                SpawnNewStar();
            }
        }

        private void SpawnNewStar() {
            if(!CanSpawnStar)
                return;

            Vector3 newPosition = placementCalculator.GetNextStarPosition();
            Star newStar = starPool.InstantiateStar(newPosition, Quaternion.identity);
            starsSpawned.Add(newStar);
        }

        private void ReturnAllStarsToPool() {
            foreach(Star star in starsSpawned) {
                starPool.ReturnStarToPool(star);
            }
            starsSpawned.Clear();
        }

        private void RemoveStarAndCheckRespawn(Star starDespawned) {
            starsSpawned.Remove(starDespawned);
            CheckForRespawn();
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

        private void CheckForRespawn() {
            if (CanSpawnStar && respawnTimer.IsDone) {
                respawnTimer.StartTimer();
            }
        }

        private void OnEnable() {
            isPlayingObserver.StartWatching();
            starDespawnedEventListener.StartListeningEvent();
        }

        private void OnDisable() {
            isPlayingObserver.StopWatching();
            starDespawnedEventListener.StopListeningEvent();
        }
    }
}

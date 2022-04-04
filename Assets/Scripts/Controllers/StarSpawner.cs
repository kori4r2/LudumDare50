using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50 {
    public class StarSpawner : MonoBehaviour {
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
            placementCalculator.Setup(starPool.PoolSize);
            isPlayingObserver = new VariableObserver<bool>(isPlaying, OnGameStateChanged);
            starDespawnedEventListener = new GenericEventListener<Star>(starDespawnedEvent, RemoveStarFromHashSet);
        }

        private void OnGameStateChanged(bool newIsPlaying) {
            if(newIsPlaying) {
                SpawnAllStars();
            } else {
                ReturnAllStarsToPool();
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

        private void RemoveStarFromHashSet(Star starDespawned) {
            starsSpawned.Remove(starDespawned);
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

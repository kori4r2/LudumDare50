using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50 {
    public class StarPool : MonoBehaviour {
        [SerializeField] private Star starPrefab;
        [SerializeField] private int poolSize;
        public int PoolSize => poolSize;
        [SerializeField] private StarEvent despawnedStarEvent;
        private GenericEventListener<Star> despawnedStarEventListener;
        private Queue<Star> starQueue = new Queue<Star>();

        private void Awake() {
            BuildPool();
            despawnedStarEventListener = new GenericEventListener<Star>(despawnedStarEvent, ReturnStarToPool);
        }

        private void BuildPool() {
            for (int index = 0; index < poolSize; index++) {
                Star newStar = Instantiate<Star>(starPrefab, Vector3.zero, Quaternion.identity);
                ReturnStarToPool(newStar);
            }
        }

        public void ReturnStarToPool(Star startDespawned) {
            GameObject starObj = startDespawned.gameObject;
            starObj.transform.SetParent(transform);
            starObj.SetActive(false);
            starQueue.Enqueue(startDespawned);
        }

        public Star InstantiateStar(Vector3 position, Quaternion rotation) {
            Star instantiatedStar = starQueue.Dequeue();
            instantiatedStar.Init();
            GameObject starObj = instantiatedStar.gameObject;
            starObj.transform.SetPositionAndRotation(position, rotation);
            starObj.SetActive(true);
            return instantiatedStar;
        }

        private void OnEnable() {
            despawnedStarEventListener.StartListeningEvent();
        }

        private void OnDisable() {
            despawnedStarEventListener.StopListeningEvent();
        }
    }
}

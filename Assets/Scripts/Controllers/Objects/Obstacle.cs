using System.Collections;
using UnityEngine;
using Toblerone.Toolbox;

namespace LudumDare50 {
    public class Obstacle : MonoBehaviour, IPoolableObject {
        [SerializeField] private float lifeTimeMin = 3f;
        [SerializeField] private float lifeTimeMax = 8f;
        [SerializeField] private ObstacleEvent obstacleDespawnedEvent;
        [SerializeField] private EventSO updateObstacleSpawnerEvent;
        [SerializeField] private ObstacleRuntimeSet runtimeSet;

        private WaitForSeconds delayCoroutine;

        private void Awake() {
            delayCoroutine = new WaitForSeconds(Random.Range(lifeTimeMin, lifeTimeMax));
        }

        public virtual void InitObject() {
            StartCoroutine(SelfDestruct());
        }

        private IEnumerator SelfDestruct() {
            yield return delayCoroutine;
            obstacleDespawnedEvent.Raise(this);
            updateObstacleSpawnerEvent.Raise();
        }

        private void OnEnable() {
            runtimeSet.AddElement(this);
        }

        private void OnDisable() {
            runtimeSet.RemoveElement(this);
        }
    }
}

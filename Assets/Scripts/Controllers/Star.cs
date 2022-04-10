using System.Collections;
using UnityEngine;

namespace LudumDare50 {
    public class Star : MonoBehaviour {
        public const string starTag = "Star";
        [SerializeField] private ActiveGameSettingsReference gameSettings;
        private float TimeGain => gameSettings.StarTimeGain;
        [SerializeField] private FloatVariable currentTime;
        [SerializeField] private BoolVariable isPlaying;
        [SerializeField] private StarRuntimeSet runtimeSet;
        [SerializeField] private StarEvent despawnedStarEvent;
        [SerializeField] private Collider2D collisionTrigger;

        private void Awake() {
            tag = starTag;
        }

        public void Init() {
            collisionTrigger.enabled = true;
        }

        private void OnEnable() {
            runtimeSet.AddElement(this);
        }

        private void OnDisable() {
            runtimeSet.RemoveElement(this);
            StopAllCoroutines();
        }

        public void FadeOut() {
            if (!isPlaying.Value)
                return;

            currentTime.Value += TimeGain;
            collisionTrigger.enabled = false;
            StartCoroutine(DebugDespawnAfterTimeCoroutine());
        }

        private IEnumerator DebugDespawnAfterTimeCoroutine() {
            yield return new WaitForSeconds(0.5f);
            Despawn();
        }

        public void Despawn() {
            if (!isPlaying.Value || !despawnedStarEvent)
                return;

            despawnedStarEvent.Raise(this);
        }
    }
}

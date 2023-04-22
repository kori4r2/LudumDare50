using System.Collections;
using UnityEngine;
using Toblerone.Toolbox;

namespace LudumDare50 {
    public class Star : MonoBehaviour, IPoolableObject {
        public const string starTag = "Star";
        [SerializeField] private ActiveGameSettingsReference gameSettings;
        private float TimeGain => gameSettings.StarTimeGain;
        [SerializeField] private FloatVariable currentTime;
        [SerializeField] private BoolVariable isPlaying;
        [SerializeField] private StarRuntimeSet runtimeSet;
        [SerializeField] private StarEvent despawnedStarEvent;
        [SerializeField] private EventSO updateStarSpawnerEvent;
        [SerializeField] private EventSO pulledBackHook;
        private EventListener pulledBackHookListener = null;
        [SerializeField] private Collider2D collisionTrigger;
        [SerializeField] private StarAnimations starAnimations;

        private void Awake() {
            tag = starTag;
            pulledBackHookListener = new EventListener(pulledBackHook, FadeOut);
        }

        public void InitObject() {
            collisionTrigger.enabled = true;
        }

        private void OnEnable() {
            runtimeSet.AddElement(this);
            starAnimations.TriggerFadeIn();
        }

        private void OnDisable() {
            runtimeSet.RemoveElement(this);
            pulledBackHookListener.StopListeningEvent();
            // TO DO Delete
            StopAllCoroutines();
        }

        public void StartStruggleAnimation() {
            starAnimations.TriggerStruggle();
            pulledBackHookListener.StartListeningEvent();
            // TO DO This should be in the Character's animation
            StartCoroutine(DebugAutoReturnCoroutine());
        }

        private IEnumerator DebugAutoReturnCoroutine() {
            yield return new WaitForSeconds(.5f);
            if (pulledBackHook)
                pulledBackHook.Raise();
        }

        public void FadeOut() {
            if (!isPlaying.Value)
                return;

            pulledBackHookListener.StopListeningEvent();
            currentTime.Value += TimeGain;
            collisionTrigger.enabled = false;
            starAnimations.TriggerFadeOut();
            // TO DO This should be in the Star's animation
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
            updateStarSpawnerEvent.Raise();
        }
    }
}

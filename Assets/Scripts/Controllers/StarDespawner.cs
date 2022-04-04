using UnityEngine;

namespace LudumDare50 {
    public class StarDespawner : MonoBehaviour {
        [SerializeField] private StarEvent starHitEvent;
        private GenericEventListener<Star> starHitEventListener;
        [SerializeField] private EventSO pulledHookEvent;
        private EventListener pulledHookEventListener;
        private Star starToDespawn = null;

        private void Awake() {
            starHitEventListener = new GenericEventListener<Star>(starHitEvent, SaveReferenceToStar);
            pulledHookEventListener = new EventListener(pulledHookEvent, DespawnSavedStar);
        }

        private void SaveReferenceToStar(Star star) {
            starToDespawn ??= star;
        }

        private void DespawnSavedStar() {
            starToDespawn = null;
        }

        private void OnEnable() {
            starHitEventListener?.StartListeningEvent();
            pulledHookEventListener?.StartListeningEvent();
        }

        private void OnDisable() {
            starHitEventListener?.StopListeningEvent();
            pulledHookEventListener?.StopListeningEvent();
        }
    }
}

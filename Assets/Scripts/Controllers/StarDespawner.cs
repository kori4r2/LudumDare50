using UnityEngine;

namespace LudumDare50 {
    public class StarDespawner : MonoBehaviour {
        [SerializeField] private StarEvent starHitEvent;
        private GenericEventListener<Star> starHitEventListener;
        [SerializeField] private EventSO pulledHookEvent;
        private EventListener pulledHookEventListener;
        [SerializeField] private BoolVariable isPlaying;
        private VariableObserver<bool> isPlayingObserver;
        private Star starToDespawn = null;

        private void Awake() {
            starHitEventListener = new GenericEventListener<Star>(starHitEvent, SaveReferenceToStar);
            pulledHookEventListener = new EventListener(pulledHookEvent, FadeoutSavedStar);
            isPlayingObserver = new VariableObserver<bool>(isPlaying, OnGameStateChanged);
        }

        private void SaveReferenceToStar(Star star) {
            if (starToDespawn) {
                return;
            }
            starToDespawn = star;
            starToDespawn.StartStruggleAnimation();
        }

        private void FadeoutSavedStar() {
            if (!isPlaying.Value || !starToDespawn)
                return;

            starToDespawn.FadeOut();
            starToDespawn = null;
        }

        private void OnGameStateChanged(bool newIsPlaying) {
            starToDespawn = null;
        }

        private void OnEnable() {
            starHitEventListener?.StartListeningEvent();
            pulledHookEventListener?.StartListeningEvent();
            isPlayingObserver?.StartWatching();
        }

        private void OnDisable() {
            starHitEventListener?.StopListeningEvent();
            pulledHookEventListener?.StopListeningEvent();
            isPlayingObserver?.StopWatching();
        }
    }
}

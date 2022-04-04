using UnityEngine;

namespace LudumDare50 {
    public class Star : MonoBehaviour {
        public const string starTag = "Star";
        [SerializeField] private float timeGain;
        [SerializeField] private FloatVariable currentTime;
        [SerializeField] private BoolVariable isPlaying;
        [SerializeField] private StarRuntimeSet runtimeSet;
        [SerializeField] private StarEvent despawnedStarEvent;

        private void Awake() {
            tag = starTag;
        }

        private void OnEnable() {
            runtimeSet.AddElement(this);
        }

        private void OnDisable() {
            runtimeSet.RemoveElement(this);
        }

        public void FadeOut() {
            if (!isPlaying.Value) {
                return;
            }
            currentTime.Value += timeGain;
        }

        public void Despawn() {
            despawnedStarEvent?.Raise(this);
        }
    }
}

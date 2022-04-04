using UnityEngine;

namespace LudumDare50 {
    public class Star : MonoBehaviour {
        public const string starTag = "Star";
        [SerializeField] private StarRuntimeSet runtimeSet;

        private void Awake() {
            tag = starTag;
        }

        private void OnEnable() {
            runtimeSet.AddElement(this);
        }

        private void OnSpawn() {
            runtimeSet.RemoveElement(this);
        }
    }
}

using UnityEngine;

namespace LudumDare50 {
    public class CanvasController : MonoBehaviour {
        [SerializeField] private VariableObserver<bool> isPlayingObserver;
        [SerializeField] private GameObject overlayObject;
        [SerializeField] private GameObject buttonObject;

        private void OnEnable() {
            isPlayingObserver.StartWatching();
        }

        private void OnDisable() {
            isPlayingObserver.StopWatching();
        }

        public void EnableOverlayAndButton(bool isPlaying) {
            overlayObject.SetActive(!isPlaying);
            buttonObject.SetActive(!isPlaying);
        }
    }
}

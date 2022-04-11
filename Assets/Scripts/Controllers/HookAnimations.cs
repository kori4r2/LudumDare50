using UnityEngine;

namespace LudumDare50 {
    [System.Serializable]
    public class HookAnimations {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Vector3 hookAnchorPointOffset;
        [SerializeField] private Animator hookAnimator;
        [SerializeField] private string shakeHookBoolParameter;
        [Header("Events")]
        [SerializeField] private StarEvent hitStarEvent;
        private GenericEventListener<Star> hitStarEventListener;
        [SerializeField] private EventSO pulledBackHookEvent;
        private EventListener pulledBackHookEventListener;
        private Vector3 defaultLocalPosition = Vector3.zero;
        private Vector3 hookAnchorPoint = Vector3.zero;

        public void ShowHook() {
            spriteRenderer.enabled = true;
        }

        public void ShowLine(Transform parentTransform) {
            hookAnchorPoint = parentTransform.position + defaultLocalPosition + hookAnchorPointOffset;
            if (!lineRenderer) {
                return;
            }
            lineRenderer.enabled = true;
            UpdateLinePoints();
        }

        private void UpdateLinePoints() {
            lineRenderer.SetPositions(new Vector3[] { spriteRenderer.transform.position, hookAnchorPoint });
        }

        public void HideHook() {
            spriteRenderer.enabled = false;
            if (lineRenderer)
                lineRenderer.enabled = false;
        }

        public void Update() {
            if (lineRenderer && lineRenderer.enabled)
                UpdateLinePoints();
        }

        public void Setup() {
            hitStarEventListener = new GenericEventListener<Star>(hitStarEvent, RepositionAndShakeHook);
            pulledBackHookEventListener = new EventListener(pulledBackHookEvent, ResetAndStopShakingHook);
            defaultLocalPosition = spriteRenderer.transform.localPosition;
            hookAnchorPoint = spriteRenderer.transform.position + hookAnchorPointOffset;
        }

        private void RepositionAndShakeHook(Star starHit) {
            spriteRenderer.transform.position = starHit.transform.position;
            if (hookAnimator)
                hookAnimator.SetBool(shakeHookBoolParameter, true);
        }

        private void ResetAndStopShakingHook() {
            spriteRenderer.transform.localPosition = defaultLocalPosition;
            if (hookAnimator)
                hookAnimator.SetBool(shakeHookBoolParameter, false);
        }

        public void Enable() {
            hitStarEventListener.StartListeningEvent();
            pulledBackHookEventListener.StartListeningEvent();
        }

        public void Disable() {
            hitStarEventListener.StopListeningEvent();
            pulledBackHookEventListener.StopListeningEvent();
        }
    }
}

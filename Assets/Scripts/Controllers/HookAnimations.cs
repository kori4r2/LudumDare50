using UnityEngine;

namespace LudumDare50 {
    [System.Serializable]
    public class HookAnimations {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite indicatorSprite;
        [SerializeField] private Sprite hookSprite;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Vector3 hookAnchorPointOffset;
        [SerializeField] private Animator hookAnimator;
        [SerializeField] private string shakeHookBoolParameter;
        private EventListener threwHookEventListener;
        private GenericEventListener<Star> hitStarEventListener;
        private EventListener pulledBackHookEventListener;
        private Vector3 defaultLocalPosition = Vector3.zero;
        private Vector3 hookAnchorPoint = Vector3.zero;

        public void ShowHookIndicator() {
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = indicatorSprite;
        }

        private void ShowHook() {
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = hookSprite;
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
            spriteRenderer.sprite = indicatorSprite;
            if (lineRenderer)
                lineRenderer.enabled = false;
        }

        public void Update() {
            if (lineRenderer && lineRenderer.enabled)
                UpdateLinePoints();
        }

        public void Setup(EventSO threwHookEvent, StarEvent hitStarEvent, EventSO pulledBackHookEvent) {
            threwHookEventListener = new EventListener(threwHookEvent, ShowHook);
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
            threwHookEventListener.StartListeningEvent();
            hitStarEventListener.StartListeningEvent();
            pulledBackHookEventListener.StartListeningEvent();
        }

        public void Disable() {
            threwHookEventListener.StopListeningEvent();
            hitStarEventListener.StopListeningEvent();
            pulledBackHookEventListener.StopListeningEvent();
        }
    }
}

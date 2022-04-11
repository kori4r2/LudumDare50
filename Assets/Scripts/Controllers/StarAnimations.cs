using UnityEngine;

namespace LudumDare50 {
    [System.Serializable]
    public class StarAnimations {
        [SerializeField] private Animator starAnimator;
        [SerializeField] private string fadeInTriggerParameter;
        [SerializeField] private string fadeOutTriggerParameter;
        [SerializeField] private string struggleTriggerParameter;

        public void TriggerFadeIn() {
            if (starAnimator)
                starAnimator.SetTrigger(fadeInTriggerParameter);
        }

        public void TriggerStruggle() {
            if (starAnimator)
                starAnimator.SetTrigger(struggleTriggerParameter);
        }

        public void TriggerFadeOut() {
            if (starAnimator)
                starAnimator.SetTrigger(fadeOutTriggerParameter);
        }
    }
}

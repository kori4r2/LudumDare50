using System;
using UnityEngine;
using Toblerone.Toolbox;

namespace LudumDare50 {
    [System.Serializable]
    public class CharacterAnimations {
        [SerializeField] private Animator characterAnimator;
        [Header("Animation parameters")]
        [SerializeField] private string movementBoolParameter;
        [SerializeField] private string swingingBoolParameter;
        [SerializeField] private string throwHookTriggerParameter;
        [SerializeField] private string struggleStarTriggerParameter;
        [SerializeField] private string pullHookTriggerParameter;
        [Header("Events")]
        [SerializeField] private EventSO threwHookEvent;
        private EventListener threwHookEventListener;
        [SerializeField] private StarEvent hitStarEvent;
        private GenericEventListener<Star> hitStarEventListener;
        [SerializeField] private EventSO pulledHookEvent;
        private EventListener pulledHookEventListener;

        public void Setup() {
            if (!characterAnimator)
                return;

            threwHookEventListener = new EventListener(threwHookEvent, TriggerThrowHookAnimation);
            hitStarEventListener = new GenericEventListener<Star>(hitStarEvent, TriggerStruggleAnimation);
            pulledHookEventListener = new EventListener(pulledHookEvent, TriggerPullHookAnimation);
        }

        private void TriggerThrowHookAnimation() {
            characterAnimator.SetTrigger(throwHookTriggerParameter);
        }

        private void TriggerStruggleAnimation(Star starHit) {
            characterAnimator.SetTrigger(struggleStarTriggerParameter);
        }

        private void TriggerPullHookAnimation() {
            characterAnimator.SetTrigger(pullHookTriggerParameter);
        }

        public void Update(bool isMoving, bool isSwinging) {
            if (!characterAnimator)
                return;

            characterAnimator.SetBool(movementBoolParameter, isMoving);
            characterAnimator.SetBool(swingingBoolParameter, isSwinging);
        }

        public void Enable() {
            if (!characterAnimator)
                return;

            threwHookEventListener.StartListeningEvent();
            hitStarEventListener.StartListeningEvent();
            pulledHookEventListener.StartListeningEvent();
        }

        public void Disable() {
            if (!characterAnimator)
                return;

            threwHookEventListener.StopListeningEvent();
            hitStarEventListener.StopListeningEvent();
            pulledHookEventListener.StopListeningEvent();
        }
    }
}

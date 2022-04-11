using System.Collections;
using UnityEngine;

namespace LudumDare50 {
    [RequireComponent(typeof(Collider2D))]
    public class Hook : MonoBehaviour {
        [Header("Settings")]
        [SerializeField] private ActiveGameSettingsReference gameSettings;
        private AimCalculator aimCalculator;
        [Header("Movement")]
        [SerializeField] private Movable2D hookMovable;
        private float ThrowSpeed => gameSettings.HookThrowSpeed;
        private float ReturnSpeed => gameSettings.HookReturnSpeed;
        [Header("Event References")]
        [SerializeField] private StarEvent hitStarEvent;
        [SerializeField] private EventSO pulledBackHook;
        [SerializeField] private EventSO threwHook;
        private EventListener hookPullBackListener;
        [Header("Animation")]
        [SerializeField] private HookAnimations hookAnimation;
        [Header("Other References")]
        [SerializeField] private BoolVariable canCharacterMove;
        [SerializeField] private StarRuntimeSet starsRuntimeSet;
        private Vector3 startingPosition;
        public bool IsAiming { get; private set; } = false;
        private bool ignoreTriggers = true;

        private void Awake() {
            aimCalculator = new AimCalculator(gameSettings);
            hookPullBackListener = new EventListener(pulledBackHook, ReturnHook);
            hookAnimation.Setup(threwHook, hitStarEvent, pulledBackHook);
            HideHook();
        }

        private void ReturnHook() {
            ignoreTriggers = true;
            hookMovable.AllowDynamicMovement();
            hookMovable.SetVelocity(-aimCalculator.ThrowDirection * ReturnSpeed);
        }

        private void StopHookMovement() {
            hookMovable.AllowKinematicMovement();
            hookMovable.SetVelocity(Vector2.zero);
        }

        private void OnEnable() {
            hookPullBackListener?.StartListeningEvent();
            hookAnimation.Enable();
        }

        private void OnDisable() {
            hookPullBackListener?.StopListeningEvent();
            hookAnimation.Disable();
        }

        private void Update() {
            hookAnimation.Update();
            if (!IsAiming)
                return;

            UpdateRotation();
            aimCalculator.UpdateAim(Time.deltaTime);
        }

        private void UpdateRotation() {
            transform.rotation = Quaternion.FromToRotation(Vector2.up, aimCalculator.ThrowDirection);
        }

        public void StartAiming() {
            transform.localPosition = startingPosition;
            aimCalculator.StartSwinging();
            UpdateRotation();
            hookAnimation.ShowHookIndicator();
            IsAiming = true;
        }

        public void ThrowHook() {
            StartHookMovement();
            IsAiming = false;
            ignoreTriggers = false;
            hookAnimation.ShowLine(transform);
            if (threwHook)
                threwHook.Raise();
        }

        private void StartHookMovement() {
            hookMovable.AllowDynamicMovement();
            hookMovable.SetVelocity(aimCalculator.ThrowDirection * ThrowSpeed);
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            GameObject otherObject = collision.gameObject;
            if (otherObject.CompareTag(Character.characterTag)) {
                HideHook();
                canCharacterMove.Value = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D collider) {
            if (ignoreTriggers)
                return;

            GameObject otherObject = collider.gameObject;
            if (otherObject.CompareTag(Star.starTag)) {
                OnStarCollision(otherObject);
            } else if (otherObject.CompareTag(Bounds.boundsTag)) {
                OnBoundsCollision();
            }
        }

        private void OnStarCollision(GameObject otherObject) {
            Star star = starsRuntimeSet.GetActiveElement(otherObject);
            if (star == null)
                return;

            ignoreTriggers = true;
            StopHookMovement();
            if (hitStarEvent)
                hitStarEvent.Raise(star);
            StartCoroutine(DebugAutoReturnCoroutine());
        }

        private IEnumerator DebugAutoReturnCoroutine() {
            yield return new WaitForSeconds(.5f);
            if (pulledBackHook)
                pulledBackHook.Raise();
        }

        private void OnBoundsCollision() {
            StopHookMovement();
            if (pulledBackHook)
                pulledBackHook.Raise();
        }

        private void HideHook() {
            transform.localPosition = startingPosition;
            StopHookMovement();
            hookAnimation.HideHook();
            IsAiming = false;
        }

        private void FixedUpdate() {
            hookMovable.UpdateMovable();
        }
    }
}

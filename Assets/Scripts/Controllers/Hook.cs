using System.Collections;
using UnityEngine;

namespace LudumDare50 {
    [RequireComponent(typeof(Collider2D))]
    public class Hook : MonoBehaviour {
        [SerializeField] private AimCalculator aimCalculator;
        [Header("Movement")]
        [SerializeField] private Movable2D hookMovable;
        [SerializeField] private float throwSpeed;
        [SerializeField] private float returnSpeed;
        [Header("Event References")]
        [SerializeField] private StarEvent hitStarEvent;
        [SerializeField] private EventSO pulledBackHook;
        private EventListener hookPullBackListener;
        [Header("Other References")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BoolVariable canCharacterMove;
        [SerializeField] private StarRuntimeSet starsRuntimeSet;
        private Vector3 startingPosition;
        public bool IsAiming { get; private set; } = false;
        private bool ignoreTriggers = true;

        private void Awake() {
            hookPullBackListener = new EventListener(pulledBackHook, ReturnHook);
            HideHook();
        }

        private void ReturnHook() {
            ignoreTriggers = true;
            hookMovable.AllowDynamicMovement();
            hookMovable.SetVelocity(-aimCalculator.ThrowDirection * returnSpeed);
        }

        private void StopHookMovement() {
            hookMovable.AllowKinematicMovement();
            hookMovable.SetVelocity(Vector2.zero);
        }

        private void OnEnable() {
            hookPullBackListener?.StartListeningEvent();
        }

        private void OnDisable() {
            hookPullBackListener?.StopListeningEvent();
        }

        private void Update() {
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
            spriteRenderer.enabled = true;
            IsAiming = true;
        }

        public void ThrowHook() {
            StartHookMovement();
            IsAiming = false;
            ignoreTriggers = false;
        }

        private void StartHookMovement() {
            hookMovable.AllowDynamicMovement();
            hookMovable.SetVelocity(aimCalculator.ThrowDirection * throwSpeed);
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            GameObject otherObject = collision.gameObject;
            if(otherObject.CompareTag(Character.characterTag)) {
                HideHook();
                canCharacterMove.Value = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D collider) {
            if(ignoreTriggers)
                return;

            GameObject otherObject = collider.gameObject;
            if(otherObject.CompareTag(Star.starTag)) {
                OnStarCollision(otherObject);
            } else if(otherObject.CompareTag(Bounds.boundsTag)) {
                OnBoundsCollision();
            }
        }

        private void OnStarCollision(GameObject otherObject) {
            Star star = starsRuntimeSet.GetActiveElement(otherObject);
            if (star == null)
                return;

            ignoreTriggers = true;
            StopHookMovement();
            hitStarEvent?.Raise(star);
            StartCoroutine(DebugAutoReturnCoroutine());
        }

        private IEnumerator DebugAutoReturnCoroutine() {
            yield return new WaitForSeconds(.5f);
            pulledBackHook?.Raise();
        }

        private void OnBoundsCollision() {
            StopHookMovement();
            pulledBackHook?.Raise();
        }

        private void HideHook() {
            transform.localPosition = startingPosition;
            StopHookMovement();
            spriteRenderer.enabled = false;
            IsAiming = false;
        }

        private void FixedUpdate() {
            hookMovable.UpdateMovable();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50 {
    [RequireComponent(typeof(Collider2D))]
    public class Hook : MonoBehaviour {
        [SerializeField] private AimCalculator aimCalculator;
        [Header("Movement")]
        [SerializeField] private Movable2D hookMovable;
        [SerializeField] private float throwSpeed;
        [SerializeField] private float returnSpeed;
        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BoolVariable canCharacterMove;
        private Vector3 startingPosition;
        public bool IsAiming { get; private set; } = false;

        private void Awake() {
            StopHookMovement();
            startingPosition = transform.localPosition;
        }

        private void StopHookMovement() {
            hookMovable.AllowKinematicMovement();
            hookMovable.SetVelocity(Vector2.zero);
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
            StartCoroutine(DebugAutoReturnCoroutine());
        }

        private void StartHookMovement() {
            hookMovable.AllowDynamicMovement();
            hookMovable.SetVelocity(aimCalculator.ThrowDirection * throwSpeed);
        }

        private IEnumerator DebugAutoReturnCoroutine() {
            yield return new WaitForSeconds(1f);
            ReturnHook();
        }

        private void ReturnHook() {
            hookMovable.SetVelocity(-aimCalculator.ThrowDirection * returnSpeed);
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            if(collision.gameObject.CompareTag(Character.characterTag)) {
                HideHook();
                canCharacterMove.Value = true;
            }
        }

        private void HideHook() {
            StopHookMovement();
            spriteRenderer.enabled = false;
            IsAiming = false;
        }

        private void FixedUpdate() {
            hookMovable.UpdateMovable();
        }
    }
}

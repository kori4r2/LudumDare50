using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LudumDare50 {
    public class Character : MonoBehaviour {
        [Header("Input")]
        [SerializeField] private InputActionReference pointerPressAction;
        [SerializeField] private InputActionReference pointerPositionAction;
        [Header("Movement")]
        [SerializeField, Range(0f, 20f)] private float moveSpeed;
        [SerializeField] private Rigidbody2D characterRigidBody;
        [SerializeField] private Collider2D characterCollider;

        private PointerInputProcessor pointerInputProcessor;
        private Movable2D movable2D;
        private bool isMoving = false;

        private void Awake() {
			pointerInputProcessor = new PointerInputProcessor(pointerPressAction, pointerPositionAction);
			SetInputProcessorCallbacks();
			movable2D = new Movable2D(characterRigidBody);
		}

		private void SetInputProcessorCallbacks() {
			pointerInputProcessor.OnPress.AddListener(OnPointerPress);
			pointerInputProcessor.OnRelease.AddListener(OnPointerRelease);
		}

		private void OnPointerPress() {
			if (IsPointerOverCharacter()) {
                movable2D.BlockMovement();
			} else {
				movable2D.AllowDynamicMovement();
                isMoving = true;
				MoveCharacterTowardsPointer();
			}
		}

		private bool IsPointerOverCharacter() {
            Vector2 pointerPosition = pointerInputProcessor.PointerPosition;
			return characterCollider.bounds.Contains(pointerPosition);
		}

		private void MoveCharacterTowardsPointer() {
            Vector2 pointerPosition = pointerInputProcessor.PointerPosition;
			Vector2 direction = new Vector2(pointerPosition.x - transform.position.x, 0).normalized;
			movable2D.SetVelocity(direction * moveSpeed);
		}

		private void OnPointerRelease() {
            movable2D.BlockMovement();
            isMoving = false;
        }

        private void Update() {
            if(!isMoving)
                return;

            MoveCharacterTowardsPointer();
        }

        private void OnEnable() {
            pointerInputProcessor.Enable();
        }

        private void OnDisable() {
            pointerInputProcessor.Disable();
        }

        private void FixedUpdate() {
            movable2D.UpdateMovable();
        }
    }
}

using UnityEngine;

namespace LudumDare50 {
    public class Character : MonoBehaviour {
        [Header("Input")]
        [SerializeField] private PointerInputProcessor pointerInputProcessor;
        [Header("Movement")]
        [SerializeField] private Movable2D movable2D;
        [SerializeField, Range(0f, 20f)] private float moveSpeed;
        [SerializeField, Range(0f, 1f)] private float deadzoneSize;
        [SerializeField] private BoolVariable canMove;
        private VariableObserver<bool> canMoveObserver;
        [SerializeField] private Collider2D characterCollider;
        [Header("Hook")]
        [SerializeField] private Hook hook;
        public const string characterTag = "Character";

        private bool isMoving = false;

        private void Awake() {
            tag = characterTag;
            canMove.Value = true;
			SetInputProcessorCallbacks();
            canMoveObserver = new VariableObserver<bool>(canMove, OnCanMoveChanged);
		}

		private void SetInputProcessorCallbacks() {
			pointerInputProcessor.OnPress.AddListener(OnPointerPress);
			pointerInputProcessor.OnRelease.AddListener(OnPointerRelease);
		}

        private void OnCanMoveChanged(bool newCanMoveValue) {
            if (!newCanMoveValue) {
                movable2D.BlockMovement();
            }
        }

		private void OnPointerPress() {
            if(!canMove.Value)
                return;

			if (IsPointerOverCharacter()) {
                canMove.Value = false;
                hook.StartAiming();
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
			Vector2 direction = GetDirectionToPointer(pointerPosition);
			movable2D.SetVelocity(direction * moveSpeed);
		}

		private Vector2 GetDirectionToPointer(Vector2 pointerPosition) {
            if(IsPointerInsideDeadzone(pointerPosition))
                return Vector2.zero;

			return new Vector2(pointerPosition.x - transform.position.x, 0f).normalized;
		}

		private bool IsPointerInsideDeadzone(Vector2 pointerPosition) {
			return Mathf.Abs(pointerPosition.x - transform.position.x) <= deadzoneSize / 2.0f;
		}

		private void OnPointerRelease() {
            if(hook.IsAiming){
                hook.ThrowHook();
                return;
            }

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
            canMoveObserver.StartWatching();
        }

        private void OnDisable() {
            pointerInputProcessor.Disable();
            canMoveObserver.StopWatching();
        }

        private void FixedUpdate() {
            movable2D.UpdateMovable();
        }
    }
}

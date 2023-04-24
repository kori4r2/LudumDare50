using Toblerone.Toolbox;
using UnityEngine;

namespace LudumDare50 {
    [System.Serializable]
    public class CharacterMovement {
        [SerializeField] private Movable2D movable2D;
        public bool IsMoving => movable2D.IsMoving;
        [SerializeField] private BoolVariable canMove;
        private VariableObserver<bool> canMoveObserver;
        public bool CanMove => canMove != null && canMove.Value;
        private ActiveGameSettingsReference gameSettings;
        private float MoveSpeed => gameSettings.CharacterMoveSpeed;
        private float DeadzoneSize => gameSettings.MovementDeadzoneSize;
        private Transform transform;
        private bool isMoving = false;

        public void Setup(ActiveGameSettingsReference gameSettings, Transform transform) {
            this.gameSettings = gameSettings;
            this.transform = transform;
            canMove.Value = true;
            canMoveObserver = new VariableObserver<bool>(canMove, OnCanMoveChanged);
        }

        private void OnCanMoveChanged(bool newCanMoveValue) {
            if (!newCanMoveValue) {
                StopMoving();
            }
        }

        public void StopMoving() {
            movable2D.BlockMovement();
            isMoving = false;
        }

        public void BlockMovement() {
            canMove.Value = false;
        }

        public void MoveCharacterTowardsPointer(PointerInputProcessor pointerInputProcessor) {
            movable2D.AllowDynamicMovement();
            isMoving = true;
            Vector2 pointerPosition = pointerInputProcessor.PointerPosition;
            Vector2 direction = GetDirectionToPointer(pointerPosition);
            movable2D.SetVelocity(direction * MoveSpeed);
        }

        private Vector2 GetDirectionToPointer(Vector2 pointerPosition) {
            if (IsPointerInsideDeadzone(pointerPosition))
                return Vector2.zero;

            return new Vector2(pointerPosition.x - transform.position.x, 0f).normalized;
        }

        private bool IsPointerInsideDeadzone(Vector2 pointerPosition) {
            return Mathf.Abs(pointerPosition.x - transform.position.x) <= DeadzoneSize / 2.0f;
        }

        public void ProcessInput(PointerInputProcessor pointerInputProcessor) {
            if (!isMoving)
                return;
            MoveCharacterTowardsPointer(pointerInputProcessor);
        }

        public void FixedUpdate() {
            movable2D.UpdateMovable();
        }

        public void EnableObservers() {
            canMoveObserver.StartWatching();
        }

        public void DisableObservers() {
            canMoveObserver.StopWatching();
        }
    }
}

using UnityEngine;
using Toblerone.Toolbox;

namespace LudumDare50 {
    public class Character : MonoBehaviour {
        public const string characterTag = "Character";
        [Header("Settings")]
        [SerializeField] private ActiveGameSettingsReference gameSettings;
        [Header("Input")]
        [SerializeField] private PointerInputProcessor pointerInputProcessor;
        [Header("Movement")]
        [SerializeField] private Movable2D movable2D;
        private float MoveSpeed => gameSettings.CharacterMoveSpeed;
        private float DeadzoneSize => gameSettings.MovementDeadzoneSize;
        [SerializeField] private Collider2D characterCollider;
        [SerializeField] private BoolVariable canMove;
        [SerializeField] private BoolVariable isPlaying;
        [SerializeField] private EventSO cameraChangeEvent;
        private EventListener camerachangeEventListener;
        private VariableObserver<bool> canMoveObserver;
        private bool isMoving = false;
        [Header("Animation")]
        [SerializeField] private CharacterAnimations characterAnimations;
        [Header("Hook")]
        [SerializeField] private Hook hook;

        private void Awake() {
            tag = characterTag;
            canMove.Value = true;
            SetInputProcessorCallbacks();
            canMoveObserver = new VariableObserver<bool>(canMove, OnCanMoveChanged);
            camerachangeEventListener = new EventListener(cameraChangeEvent, StayWithinCameraLimits);
            characterAnimations.Setup();
        }

        private void SetInputProcessorCallbacks() {
            pointerInputProcessor.OnPress.AddListener(OnPointerPress);
            pointerInputProcessor.OnRelease.AddListener(OnPointerRelease);
        }

        private void OnPointerPress() {
            if (!isPlaying.Value || !canMove.Value)
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

        private void OnPointerRelease() {
            if (hook.IsAiming) {
                hook.ThrowHook();
                return;
            }

            movable2D.BlockMovement();
            isMoving = false;
        }

        private void OnCanMoveChanged(bool newCanMoveValue) {
            if (!newCanMoveValue) {
                movable2D.BlockMovement();
            }
        }

        private void StayWithinCameraLimits() {
            Vector2 currentPosition = transform.position;
            Vector2 minPosition = CalculateMinCharacterPosition();
            Vector2 maxPosition = CalculateMaxCharacterPosition();
            if (CameraUtils.IsInsideLimits(currentPosition, minPosition, maxPosition))
                return;

            ClampCurrentPosition(currentPosition, minPosition, maxPosition);
        }

        private Vector2 CalculateMinCharacterPosition() {
            Vector2 minPosition = CameraUtils.GetWorldSpaceCameraMinPosition(pointerInputProcessor.MainCamera);
            minPosition.x += DeadzoneSize / 2f;
            return minPosition;
        }

        private Vector2 CalculateMaxCharacterPosition() {
            Vector2 maxPosition = CameraUtils.GetWorldSpaceCameraMaxPosition(pointerInputProcessor.MainCamera);
            maxPosition.x -= DeadzoneSize / 2f;
            return maxPosition;
        }

        private void ClampCurrentPosition(Vector2 currentPosition, Vector2 minPosition, Vector2 maxPosition) {
            float newPositionX = Mathf.Clamp(currentPosition.x, minPosition.x, maxPosition.x);
            float newPositionY = Mathf.Clamp(currentPosition.y, minPosition.y, maxPosition.y);
            transform.position = new Vector3(newPositionX, newPositionY, transform.position.z);
        }

        private void Update() {
            characterAnimations.Update(movable2D.IsMoving, hook.IsAiming);
            if (!isMoving)
                return;

            MoveCharacterTowardsPointer();
        }

        private void OnEnable() {
            pointerInputProcessor.Enable();
            characterAnimations.Enable();
            canMoveObserver.StartWatching();
            camerachangeEventListener.StartListeningEvent();
        }

        private void OnDisable() {
            pointerInputProcessor.Disable();
            characterAnimations.Disable();
            canMoveObserver.StopWatching();
            camerachangeEventListener.StopListeningEvent();
        }

        private void FixedUpdate() {
            movable2D.UpdateMovable();
        }
    }
}

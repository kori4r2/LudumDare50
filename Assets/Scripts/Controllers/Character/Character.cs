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
        [SerializeField] private CharacterMovement characterMovement;
        [SerializeField] private Collider2D characterCollider;
        [SerializeField] private BoolVariable isPlaying;
        [SerializeField] private EventSO cameraChangeEvent;
        private EventListener camerachangeEventListener;
        [Header("Animation")]
        [SerializeField] private CharacterAnimations characterAnimations;
        [Header("Hook")]
        [SerializeField] private Hook hook;

        private void Awake() {
            tag = characterTag;
            characterMovement.Setup(gameSettings, transform);
            SetInputProcessorCallbacks();
            camerachangeEventListener = new EventListener(cameraChangeEvent, StayWithinCameraLimits);
            characterAnimations.Setup();
        }

        private void SetInputProcessorCallbacks() {
            pointerInputProcessor.OnPress.AddListener(OnPointerPress);
            pointerInputProcessor.OnRelease.AddListener(OnPointerRelease);
        }

        private void OnPointerPress() {
            if (!isPlaying.Value || !characterMovement.CanMove)
                return;

            if (IsPointerOverCharacter()) {
                characterMovement.BlockMovement();
                hook.StartAiming();
            } else {
                characterMovement.MoveCharacterTowardsPointer(pointerInputProcessor);
            }
        }

        private bool IsPointerOverCharacter() {
            Vector2 pointerPosition = pointerInputProcessor.PointerPosition;
            return characterCollider.bounds.Contains(pointerPosition);
        }

        private void OnPointerRelease() {
            if (hook.IsAiming) {
                hook.ThrowHook();
                return;
            }

            characterMovement.StopMoving();
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
            minPosition.x += gameSettings.MovementDeadzoneSize / 2f;
            return minPosition;
        }

        private Vector2 CalculateMaxCharacterPosition() {
            Vector2 maxPosition = CameraUtils.GetWorldSpaceCameraMaxPosition(pointerInputProcessor.MainCamera);
            maxPosition.x -= gameSettings.MovementDeadzoneSize / 2f;
            return maxPosition;
        }

        private void ClampCurrentPosition(Vector2 currentPosition, Vector2 minPosition, Vector2 maxPosition) {
            float newPositionX = Mathf.Clamp(currentPosition.x, minPosition.x, maxPosition.x);
            float newPositionY = Mathf.Clamp(currentPosition.y, minPosition.y, maxPosition.y);
            transform.position = new Vector3(newPositionX, newPositionY, transform.position.z);
        }

        private void Update() {
            characterAnimations.Update(characterMovement.IsMoving, hook.IsAiming);
            characterMovement.ProcessInput(pointerInputProcessor);
        }

        private void OnEnable() {
            pointerInputProcessor.Enable();
            characterAnimations.Enable();
            characterMovement.EnableObservers();
            camerachangeEventListener.StartListeningEvent();
        }

        private void OnDisable() {
            pointerInputProcessor.Disable();
            characterAnimations.Disable();
            characterMovement.DisableObservers();
            camerachangeEventListener.StopListeningEvent();
        }

        private void FixedUpdate() {
            characterMovement.FixedUpdate();
        }
    }
}

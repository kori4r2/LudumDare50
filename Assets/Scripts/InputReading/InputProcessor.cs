using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace LudumDare50 {
    [System.Serializable]
    public class PointerInputProcessor {
        [SerializeField] private InputActionReference pointerPressAction;
        [SerializeField] private InputActionReference pointerPositionAction;
        private Vector2 pointerPosition = Vector2.zero;
        public Vector2 PointerPosition => pointerPosition;

        public UnityEvent OnPress { get; private set; } = new UnityEvent();
        public UnityEvent OnRelease { get; private set; } = new UnityEvent();
        private Camera mainCamera = null;
        private Camera MainCamera {
            get{
                mainCamera ??= Camera.main;
                return mainCamera;
            }
        }

        public PointerInputProcessor(InputActionReference pressAction, InputActionReference positionAction) {
            pointerPressAction = pressAction;
            pointerPositionAction = positionAction;
        }

        public void Enable() {
            pointerPressAction.action.started += OnPressPointer;
            pointerPressAction.action.canceled += OnReleasePointer;
            pointerPositionAction.action.performed += OnMovePointer;
        }

        public void Disable() {
            pointerPressAction.action.started -= OnPressPointer;
            pointerPressAction.action.canceled -= OnReleasePointer;
            pointerPositionAction.action.performed -= OnMovePointer;
        }

        private void OnPressPointer(InputAction.CallbackContext context) {
            OnPress?.Invoke();
        }

        private void OnReleasePointer(InputAction.CallbackContext context) {
            OnRelease?.Invoke();
        }

        private void OnMovePointer(InputAction.CallbackContext context) {
            Vector2 actionValue = context.ReadValue<Vector2>();
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);
            Vector2 screenPosition = ClampVector2(actionValue, Vector2.zero, screenSize);
            pointerPosition = MainCamera.ScreenToWorldPoint(screenPosition);
        }

        private static Vector2 ClampVector2(Vector2 value, Vector2 minValue, Vector2 maxValue) {
            return new Vector2(Mathf.Clamp(value.x, minValue.x, maxValue.x), Mathf.Clamp(value.y, minValue.y, maxValue.y));
        }
    }
}

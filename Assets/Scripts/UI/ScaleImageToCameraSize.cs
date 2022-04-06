using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50 {
    public class ScaleImageToCameraSize : MonoBehaviour {
        [SerializeField] private Camera scalingCamera;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private EventSO canvasChangeEvent;
        private EventListener canvasChangeEventListener;

        private void Awake() {
            UpdateImageSize();
            canvasChangeEventListener = new EventListener(canvasChangeEvent, UpdateImageSize);
        }

        private void UpdateImageSize() {
            Vector2 cameraSize = CameraUtils.GetWorldSpaceCameraSize(scalingCamera);
            spriteRenderer.transform.localScale = new Vector3(cameraSize.x, cameraSize.y, 1.0f);
        }

        private void OnEnable() {
            canvasChangeEventListener.StartListeningEvent();
        }

        private void OnDisable() {
            canvasChangeEventListener.StopListeningEvent();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50 {
    public class ScaleImageToCameraSize : MonoBehaviour {
        [SerializeField] private Camera scalingCamera;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private void Awake() {
            Vector2 cameraSize = CameraUtils.GetWorldSpaceCameraSize(scalingCamera);
            spriteRenderer.transform.localScale = new Vector3(cameraSize.x, cameraSize.y, 1.0f);
        }
    }
}

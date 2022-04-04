using UnityEngine;

namespace LudumDare50 {
    public static class CameraUtils {
        public static Vector2 GetWorldSpaceCameraSize(Camera camera) {
            Vector3 screenSpaceSize = new Vector3(Screen.width, Screen.height, 0f);
            Vector2 worldSpaceSize = 2 * camera.ScreenToWorldPoint(screenSpaceSize);
            return worldSpaceSize;
        }
    }
}

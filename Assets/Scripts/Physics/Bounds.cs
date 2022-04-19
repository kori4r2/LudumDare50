using System;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50 {
    public class Bounds : MonoBehaviour {
        #region Static Variables and Properties
        private static Vector2 FirstCorner => new Vector2(-1, 1);
        private static Vector2 SecondCorner => new Vector2(-1, -1);
        private static Vector2 ThirdCorner => new Vector2(1, -1);
        private static Vector2 FourthCorner => new Vector2(1, 1);
        private static Vector2[] defaultColliderPoints = null;
        private static Vector2[] DefaultColliderPoints {
            get {
                defaultColliderPoints ??= new Vector2[] { FirstCorner, SecondCorner, ThirdCorner, FourthCorner, FirstCorner };
                return defaultColliderPoints;
            }
        }
        #endregion
        private const float edgeRadius = 5f;
        public const string boundsTag = "Bounds";
        [SerializeField] private Camera boundsCamera;
        [SerializeField] private EdgeCollider2D boundsCollider;
        [SerializeField] private EventSO cameraChangeEvent;
        private EventListener cameraChangeEventListener;

        private void Awake() {
            cameraChangeEventListener = new EventListener(cameraChangeEvent, OnCameraChange);
        }

        private void OnCameraChange() {
            UpdateBounds();
        }

        private void OnEnable() {
            cameraChangeEventListener?.StartListeningEvent();
        }

        private void OnDisable() {
            cameraChangeEventListener?.StopListeningEvent();
        }

        private void Start() {
            UpdateBounds();
        }

        private void UpdateBounds() {
            Vector2 worldSpaceLimits = GetWorldSpaceLimits();
            UpdateEdgeCollider(worldSpaceLimits);
        }

        private Vector2 GetWorldSpaceLimits() {
            Vector2 worldSpaceLimits = CameraUtils.GetWorldSpaceCameraSize(boundsCamera);
            worldSpaceLimits += new Vector2(2 * edgeRadius, 2 * edgeRadius);
            return worldSpaceLimits;
        }

        private void UpdateEdgeCollider(Vector2 worldSpaceLimits) {
            boundsCollider.transform.position = boundsCamera.transform.position;
            boundsCollider.edgeRadius = edgeRadius;
            List<Vector2> newPoints = GetNewColliderPoints(worldSpaceLimits);
            boundsCollider.SetPoints(newPoints);
        }

        private static List<Vector2> GetNewColliderPoints(Vector2 worldSpaceLimits) {
            List<Vector2> newPoints = new List<Vector2>(DefaultColliderPoints);
            for (int index = 0; index < DefaultColliderPoints.Length; index++) {
                newPoints[index] *= worldSpaceLimits / 2f;
            }
            return newPoints;
        }
    }
}

using UnityEngine;

namespace LudumDare50 {
    [System.Serializable]
    public class AimCalculator  {
        [SerializeField, Range(0, 180)] private float swingAngle;
        [SerializeField, Tooltip("Angles per second")] private float swingingSpeed;
        private float aimTime;
        private float QuarterPeriod => (swingAngle / 2.0f) / swingingSpeed;
        private int tIndex => Mathf.FloorToInt(aimTime / QuarterPeriod) % 4;
        public Vector2 ThrowDirection { get; private set; } = Vector2.up;
        private Vector2 swingStartingPoint {
            get {
                return tIndex switch {
                    0 => Quaternion.AngleAxis(QuarterPeriod * swingingSpeed, Vector3.forward) * Vector2.up,
                    1 => Vector2.up,
                    2 => Quaternion.AngleAxis(QuarterPeriod * swingingSpeed, Vector3.back) * Vector2.up,
                    3 => Vector2.up,
                    _ => Vector2.zero
                };
            }
        }

        public void StartSwinging() {
            aimTime = 0;
            ThrowDirection = swingStartingPoint;
        }

        public void UpdateAim(float deltaTime) {
            Vector3 rotationAxis = (tIndex > 1)? Vector3.forward : Vector3.back;
            int nQuarterPeriodsPassed = Mathf.FloorToInt(aimTime / QuarterPeriod);
            float swingOffsetTime = aimTime - nQuarterPeriodsPassed * QuarterPeriod;
            ThrowDirection = Quaternion.AngleAxis(swingingSpeed * swingOffsetTime, rotationAxis) * swingStartingPoint;
            aimTime += deltaTime;
        }
    }
}

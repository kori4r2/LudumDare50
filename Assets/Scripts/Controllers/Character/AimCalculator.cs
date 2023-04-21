using UnityEngine;

namespace LudumDare50 {
    [System.Serializable]
    public class AimCalculator {
        private float swingAngle;
        private float swingingSpeed;
        private float aimTime;
        private float QuarterPeriod => (swingAngle / 2.0f) / swingingSpeed;
        private int TimeIndex => Mathf.FloorToInt(aimTime / QuarterPeriod) % 4;
        public Vector2 ThrowDirection { get; private set; } = Vector2.up;
        private Vector2 SwingStartingPoint {
            get => TimeIndex switch {
                0 => Quaternion.AngleAxis(QuarterPeriod * swingingSpeed, Vector3.forward) * Vector2.up,
                1 => Vector2.up,
                2 => Quaternion.AngleAxis(QuarterPeriod * swingingSpeed, Vector3.back) * Vector2.up,
                3 => Vector2.up,
                _ => Vector2.zero
            };
        }

        public AimCalculator(ActiveGameSettingsReference gameSettings) {
            swingAngle = gameSettings.HookAimAngle;
            swingingSpeed = gameSettings.HookSwingingSpeed;
        }

        public void StartSwinging() {
            aimTime = 0;
            ThrowDirection = SwingStartingPoint;
        }

        public void UpdateAim(float deltaTime) {
            Vector3 rotationAxis = (TimeIndex > 1) ? Vector3.forward : Vector3.back;
            int nQuarterPeriodsPassed = Mathf.FloorToInt(aimTime / QuarterPeriod);
            float swingOffsetTime = aimTime - (nQuarterPeriodsPassed * QuarterPeriod);
            ThrowDirection = Quaternion.AngleAxis(swingingSpeed * swingOffsetTime, rotationAxis) * SwingStartingPoint;
            aimTime += deltaTime;
        }
    }
}

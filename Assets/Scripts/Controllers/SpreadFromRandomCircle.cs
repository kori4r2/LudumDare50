using UnityEngine;

namespace LudumDare50 {
    public class SpreadFromRandomCircle : IStarSpreader {
        private Rect rectangularArea;
        public Vector3 GetNewPosition() {
            Vector2 newPoint = MapCircleToSquare(Random.insideUnitCircle);
            newPoint.x *= rectangularArea.width / 2.0f;
            newPoint.y *= rectangularArea.height / 2.0f;
            newPoint += rectangularArea.center;
            return new Vector3(newPoint.x, newPoint.y, 0f);
        }

        public void Setup(Rect area) {
            rectangularArea = area;
        }

        private static Vector2 MapCircleToSquare(Vector2 circlePosition) {
            // Bless you Chamberlain Fong
            float u = circlePosition.x;
            float v = circlePosition.y;
            float u2 = u * u;
            float v2 = v * v;
            float twosqrt2 = 2.0f * Mathf.Sqrt(2.0f);
            float subtermx = 2.0f + u2 - v2;
            float subtermy = 2.0f - u2 + v2;
            float termx1 = subtermx + u * twosqrt2;
            float termx2 = subtermx - u * twosqrt2;
            float termy1 = subtermy + v * twosqrt2;
            float termy2 = subtermy - v * twosqrt2;
            float x = 0.5f * Mathf.Sqrt(termx1) - 0.5f * Mathf.Sqrt(termx2);
            float y = 0.5f * Mathf.Sqrt(termy1) - 0.5f * Mathf.Sqrt(termy2);
            return new Vector2(x, y);
        }
    }
}

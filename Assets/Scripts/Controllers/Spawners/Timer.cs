namespace LudumDare50 {
    public class Timer {
        private float duration;
        public float TimeLeft { get; private set; }
        public bool IsDone => TimeLeft <= 0f;

        public Timer(float duration) {
            this.duration = duration;
            TimeLeft = 0f;
        }

        public void StartTimer() {
            TimeLeft = duration;
        }

        public void StopTimer() {
            TimeLeft = 0f;
        }

        public void UpdateTimer(float deltaTime) {
            if (IsDone)
                return;

            TimeLeft -= deltaTime;
            if (TimeLeft <= 0f) {
                TimeLeft = 0f;
            }
        }
    }
}

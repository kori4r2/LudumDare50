namespace LudumDare50 {
    public class Timer {
        private float duration;
        private float timeLeft;
        public float TimeLeft => timeLeft;
        public bool IsDone => timeLeft <= 0f;

        public Timer(float duration) {
            this.duration = duration;
            timeLeft = 0f;
        }

        public void StartTimer() {
            timeLeft = duration;
        }

        public void StopTimer() {
            timeLeft = 0f;
        }

        public void UpdateTimer(float deltaTime) {
            if (IsDone)
                return;

            timeLeft -= deltaTime;
            if (timeLeft <= 0f) {
                timeLeft = 0f;
            }
        }
    }
}

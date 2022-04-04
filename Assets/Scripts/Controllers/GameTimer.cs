using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50 {
    [System.Serializable]
    public class GameTimer {
        [SerializeField] private float maxTime;
        private BoolVariable isPlaying;
        private VariableObserver<bool> isPlayingObserver;
        [SerializeField] private FloatVariable timeVariable;
        public float CurrentTime => timeVariable.Value;

        public void Setup(BoolVariable isPlayingReference) {
            isPlaying = isPlayingReference;
            isPlayingObserver = new VariableObserver<bool>(isPlaying, OnGameStateChanged);
        }

        private void OnGameStateChanged(bool newIsPlaying) {
            if(newIsPlaying)
                timeVariable.Value = maxTime;
        }

        public void UpdateTimer(float deltaTime) {
            if(!isPlaying.Value)
                return;

            timeVariable.Value -= deltaTime;
            CheckGameEnd();
        }

        private void CheckGameEnd() {
            if (timeVariable.Value > float.Epsilon)
                return;

            timeVariable.Value = 0f;
            isPlaying.Value = false;
        }

        public void OnEnable() {
            isPlayingObserver?.StartWatching();
        }

        public void OnDisable() {
            isPlayingObserver?.StopWatching();
        }
    }
}

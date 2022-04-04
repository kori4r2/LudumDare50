using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50 {
    public class Background : MonoBehaviour {
        [SerializeField] private BoolVariable isPlaying;
        [SerializeField] private GameTimer timer;

        private void Awake() {
            timer.Setup(isPlaying);
        }

        private void Update() {
            timer.UpdateTimer(Time.deltaTime);
            if(isPlaying.Value) {
                UpdateBackgroundSmooth(timer.CurrentTime);
            } else {
                UpdateBackgroundFast(timer.CurrentTime);
            }
        }

        private void UpdateBackgroundSmooth(float currentTime) {
        }

        private void UpdateBackgroundFast(float currentTime) {
        }

        private void OnEnable() {
            timer.OnEnable();
        }

        private void OnDisable() {
            timer.OnDisable();
        }
    }
}

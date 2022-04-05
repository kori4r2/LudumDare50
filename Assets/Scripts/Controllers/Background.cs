using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50 {
    public class Background : MonoBehaviour {
        [SerializeField] private BoolVariable isPlaying;
        [SerializeField] private GameTimer timer;
        private float maxMaterialOffset => 0.8f;
        private float minMaterialOffset => -1f;
        private float currentMaterialOffset => minMaterialOffset + (maxMaterialOffset-minMaterialOffset) * timer.CurrentProgress;
        private float lastUsedOffset;
        [SerializeField] private Material backgroundShaderMaterial;
        [SerializeField, Range(0f, 0.95f)] private float gameplaySmoothing = 0.5f;
        [SerializeField, Range(0f, 0.95f)] private float cutsceneSmoothing = 0.1f;
        private const string shaderPropertyName = "SkyOffset";

        private void Awake() {
            timer.Setup(isPlaying);
            lastUsedOffset = 0f;
        }

        private void Update() {
            timer.UpdateTimer(Time.deltaTime);
            if(isPlaying.Value) {
                UpdateBackgroundWithSmoothing(gameplaySmoothing);
            } else {
                UpdateBackgroundWithSmoothing(cutsceneSmoothing);
            }
        }

        private void UpdateBackgroundWithSmoothing(float smoothing) {
            lastUsedOffset = Mathf.Lerp(lastUsedOffset, currentMaterialOffset, (1.0f - smoothing));
            backgroundShaderMaterial.SetFloat(shaderPropertyName, lastUsedOffset);
        }

        private void OnEnable() {
            timer.OnEnable();
        }

        private void OnDisable() {
            timer.OnDisable();
        }
    }
}

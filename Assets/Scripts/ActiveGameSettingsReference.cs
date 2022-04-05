using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50 {
    public class ActiveGameSettingsReference : ScriptableObject {
        [SerializeField] private GameSettings activeGameSettings;
        public float CharacterMoveSpeed { get => activeGameSettings.CharacterMoveSpeed; }
        public float MovementDeadzoneSize { get => activeGameSettings.MovementDeadzoneSize; }
        public float MaxTime { get => activeGameSettings.MaxTime; }
        public float StarTimeGain { get => activeGameSettings.StarTimeGain; }
        public float SpaceBetweenStars { get => activeGameSettings.SpaceBetweenStars; }
        public float StarsRespawnTime { get => activeGameSettings.StarsRespawnTime; }
        public float HookAimAngle { get => activeGameSettings.HookAimAngle; }
        public float HookSwingingSpeed { get => activeGameSettings.HookSwingingSpeed; }
        public float HookThrowSpeed { get => activeGameSettings.HookThrowSpeed; }
        public float HookReturnSpeed { get => activeGameSettings.HookReturnSpeed; }
    }
}

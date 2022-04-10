using UnityEngine;

namespace LudumDare50 {
    public class ActiveGameSettingsReference : ScriptableObject {
        [SerializeField] private GameSettings activeGameSettings;
        public float CharacterMoveSpeed => activeGameSettings.CharacterMoveSpeed;
        public float MovementDeadzoneSize => activeGameSettings.MovementDeadzoneSize;
        public float MaxTime => activeGameSettings.MaxTime;
        public float StarTimeGain => activeGameSettings.StarTimeGain;
        public float SpaceBetweenStars => activeGameSettings.SpaceBetweenStars;
        public float StarsRespawnTime => activeGameSettings.StarsRespawnTime;
        public float HookAimAngle => activeGameSettings.HookAimAngle;
        public float HookSwingingSpeed => activeGameSettings.HookSwingingSpeed;
        public float HookThrowSpeed => activeGameSettings.HookThrowSpeed;
        public float HookReturnSpeed => activeGameSettings.HookReturnSpeed;
    }
}

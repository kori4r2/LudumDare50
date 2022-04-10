using UnityEngine;

namespace LudumDare50 {
    [CreateAssetMenu(menuName = "GameSettings")]
    public class GameSettings : ScriptableObject {
        [SerializeField, Range(0f, 20f)] private float characterMoveSpeed;
        public float CharacterMoveSpeed => characterMoveSpeed;
        [SerializeField, Range(0f, 1f)] private float movementDeadzoneSize;
        public float MovementDeadzoneSize => movementDeadzoneSize;
        [SerializeField] private float maxTime;
        public float MaxTime => maxTime;
        [SerializeField] private float starTimeGain;
        public float StarTimeGain => starTimeGain;
        [SerializeField, Range(0f, 50f)] private float spaceBetweenStars;
        public float SpaceBetweenStars => spaceBetweenStars;
        [SerializeField] private float starsRespawnTime;
        public float StarsRespawnTime => starsRespawnTime;
        [SerializeField, Range(0f, 180f)] private float hookAimAngle;
        public float HookAimAngle => hookAimAngle;
        [SerializeField, Tooltip("Angles per second")] private float hookSwingingSpeed;
        public float HookSwingingSpeed => hookSwingingSpeed;
        [SerializeField] private float hookThrowSpeed;
        public float HookThrowSpeed => hookThrowSpeed;
        [SerializeField] private float hookReturnSpeed;
        public float HookReturnSpeed => hookReturnSpeed;
    }
}

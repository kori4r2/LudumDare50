using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50 {
    [CreateAssetMenu(menuName="GameSettings")]
    public class GameSettings : ScriptableObject {
        [SerializeField, Range(0f, 20f)] private float characterMoveSpeed;
        public float CharacterMoveSpeed { get => characterMoveSpeed; }
        [SerializeField, Range(0f, 1f)] private float movementDeadzoneSize;
        public float MovementDeadzoneSize { get => movementDeadzoneSize; }
        [SerializeField] private float maxTime;
        public float MaxTime { get => maxTime; }
        [SerializeField] private float starTimeGain;
        public float StarTimeGain { get => starTimeGain; }
        [SerializeField, Range(0f, 50f)] private float spaceBetweenStars;
        public float SpaceBetweenStars { get => spaceBetweenStars; }
        [SerializeField] private float starsRespawnTime;
        public float StarsRespawnTime { get => starsRespawnTime; }
        [SerializeField, Range(0f, 180f)] private float hookAimAngle;
        public float HookAimAngle { get => hookAimAngle; }
        [SerializeField, Tooltip("Angles per second")] private float hookSwingingSpeed;
        public float HookSwingingSpeed { get => hookSwingingSpeed; }
        [SerializeField] private float hookThrowSpeed;
        public float HookThrowSpeed { get => hookThrowSpeed; }
        [SerializeField] private float hookReturnSpeed;
        public float HookReturnSpeed { get => hookReturnSpeed; }
    }
}

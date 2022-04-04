using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50 {
    [System.Serializable]
    public class CharacterAnimatorInterface {
        [SerializeField] private RuntimeAnimatorController animatorController;

        private void Awake() {
        }
    }
}

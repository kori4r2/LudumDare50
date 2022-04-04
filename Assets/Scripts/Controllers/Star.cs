using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50 {
    public class Star : MonoBehaviour {
        public const string starTag = "Star";

        private void Awake() {
            tag = starTag;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50 {
    public class AnimationEventRaiser : MonoBehaviour {
        public void RaiseEvent(EventSO eventToRaise) {
            eventToRaise?.Raise();
        }
    }
}

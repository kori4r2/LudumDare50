using UnityEngine;

namespace LudumDare50 {
    public class AnimationEventRaiser : MonoBehaviour {
        public void RaiseEvent(EventSO eventToRaise) {
            if (eventToRaise)
                eventToRaise.Raise();
        }
    }
}

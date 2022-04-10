using UnityEngine;

namespace LudumDare50 {
    public class AnimationEventRaiser : MonoBehaviour {
        public void RaiseEvent(EventSO eventToRaise) {
            eventToRaise?.Raise();
        }
    }
}

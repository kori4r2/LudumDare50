using UnityEngine;
using UnityEngine.EventSystems;

namespace LudumDare50 {
    public class RaiseEventOnRectChange : UIBehaviour {
        [SerializeField] private EventSO eventToRaise;
        protected override void OnRectTransformDimensionsChange() {
            if (Application.isPlaying && eventToRaise)
                eventToRaise.Raise();
        }
    }
}

using UnityEngine;

namespace LudumDare50 {
    public interface IObjectSpreader {
        void Setup(Rect area);
        Vector3 GetNewPosition();
    }
}

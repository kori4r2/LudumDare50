using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50 {
    public interface IStarSpreader {
        void Setup(Rect area);
        Vector3 GetNewPosition();
    }
}
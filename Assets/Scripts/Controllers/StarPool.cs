using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50 {
    public class StarPool : ObjectPool<Star> {
        [SerializeField] private Star starPrefab;
        [SerializeField] private StarEvent despawnedStarEvent;

        protected override GenericEvent<Star> DespawnedObjectEvent => despawnedStarEvent;

        protected override Star ObjectPrefab => starPrefab;
    }
}

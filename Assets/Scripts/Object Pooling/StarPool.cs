using UnityEngine;
using Toblerone.Toolbox;

namespace LudumDare50 {
    public class StarPool : ObjectPool<Star> {
        [SerializeField] private Star starPrefab;
        protected override Star ObjectPrefab => starPrefab;
        [SerializeField] private StarEvent despawnedStarEvent;
        protected override GenericEvent<Star> DespawnedObjectEvent => despawnedStarEvent;

    }
}

using UnityEngine;
using Toblerone.Toolbox;

namespace LudumDare50 {
    public class ObstaclePool : ObjectPool<Obstacle> {
        [SerializeField] private Obstacle obstaclePrefab;
        protected override Obstacle ObjectPrefab => obstaclePrefab;
        [SerializeField] private ObstacleEvent despawnedObstacleEvent;

        protected override GenericEvent<Obstacle> DespawnedObjectEvent => despawnedObstacleEvent;
    }
}

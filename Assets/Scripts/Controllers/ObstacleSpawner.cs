using UnityEngine;

namespace LudumDare50 {
    public class ObstacleSpawner : ObjectSpawner<Obstacle> {
        protected override float RespawnTime => gameSettings.ObstaclesRespawnTime;
        [SerializeField] private ObstaclePlacementCalculator placementCalculator;
        protected override ObjectPlacementCalculator<Obstacle> PlacementCalculator => placementCalculator;
        [SerializeField] private ObstaclePool obstaclePool;
        protected override ObjectPool<Obstacle> ObjectPool => obstaclePool;
        [SerializeField] private EventSO updateObectSpawnerEvent;
        protected override EventSO UpdateObjectSpawnerEvent => updateObectSpawnerEvent;
        [SerializeField] private RuntimeSet<Obstacle> obstaclesSpawned;
        protected override RuntimeSet<Obstacle> ObjectsSpawned => obstaclesSpawned;
        protected override void SpawnInitialObjects() { }
    }
}

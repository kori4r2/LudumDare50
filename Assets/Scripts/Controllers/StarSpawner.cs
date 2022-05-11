using UnityEngine;

namespace LudumDare50 {
    public class StarSpawner : ObjectSpawner<Star> {
        protected override float RespawnTime => gameSettings.StarsRespawnTime;
        [SerializeField] private StarPlacementCalculator placementCalculator;
        protected override ObjectPlacementCalculator<Star> PlacementCalculator => placementCalculator;
        [SerializeField] private StarPool starPool;
        protected override ObjectPool<Star> ObjectPool => starPool;
        [SerializeField] private EventSO updateStarSpawnerEvent;
        protected override EventSO UpdateObjectSpawnerEvent => updateStarSpawnerEvent;
        [SerializeField] private RuntimeSet<Star> starsSpawned;
        protected override RuntimeSet<Star> ObjectsSpawned => starsSpawned;

        protected override void SpawnInitialObjects() {
            while (CanSpawnObject) {
                SpawnNewObject();
            }
        }
    }
}

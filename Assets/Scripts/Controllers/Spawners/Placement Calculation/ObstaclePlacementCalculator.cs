using Toblerone.Toolbox;

namespace LudumDare50 {
    [System.Serializable]
    public class ObstaclePlacementCalculator : ObjectPlacementCalculator<Obstacle> {
        protected override int MinNObjects => 1;
        public override void Setup(ActiveGameSettingsReference gameSettings, int poolSize, RuntimeSet<Obstacle> runtimeSet) {
            spaceBetweenObjects = gameSettings.SpaceBetweenObstacles;
            Setup(poolSize, runtimeSet);
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

namespace LudumDare50 {
    [System.Serializable]
    public class StarPlacementCalculator : ObjectPlacementCalculator<Star> {
        public override void Setup(ActiveGameSettingsReference gameSettings, int poolSize, RuntimeSet<Star> runtimeSet) {
            spaceBetweenObjects = gameSettings.SpaceBetweenStars;
            Setup(poolSize, runtimeSet);
        }
    }
}

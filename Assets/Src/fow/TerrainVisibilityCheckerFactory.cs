using UnityEngine;

namespace fow
{
    static class TerrainVisibilityCheckerFactory
    {
        public static IVisibilityChecker Create()
        {
            return Terrain.activeTerrain.GetComponent<TerrainVisibilityComponent>();
        }
    }
}

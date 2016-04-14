using UnityEngine;

namespace vfx
{
    class UnitSelectionChecker : MonoBehaviour
    {
        public void Start()
        {
            var terrain = Terrain.activeTerrain;
            if (terrain != null && terrain.gameObject.layer != LayersInt.Terrain)
            {
                Debug.LogWarning("Terrain doesn't have terrain layer set. UnitSelection effect won't work");
            }
        }
    }
}

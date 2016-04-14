using UnityEngine;

namespace fow
{

    public class FOWVisor : MonoBehaviour
    {
        [SerializeField]
        private ViewRange viewRange = null;

        public float Range
        {
            get
            {
                return viewRange.range;
            }
        }

        public Vector3 Position()
        {
            return transform.position;
        }

        private void Update()
        {
            var terrainVisibility = Terrain.activeTerrain.GetComponent<TerrainVisibilityComponent>();
            if (terrainVisibility != null) {
                terrainVisibility.markAreaVisible(Position(), Range);
            }
        }

        private void OnDrawGizmosSelected()
        {
            dbg.GizmoUtils.DrawCircleXZ(transform.position, Range, Color.grey);
        }
    }

}

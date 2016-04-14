using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace ui.input.gesture
{
    class Path
    {
        public IList<Vector3> Points { get; private set; }

        static public Path Detect(IList<Vector2> points)
        {
            if (points.Count >= 2 && select.Selection.instance.Objects.Any()) {
                IList<Vector3> path = (from p in points
                                        let worldPos = ScreenPointToTerrain(p)
                                        where worldPos.HasValue
                                        select worldPos.Value).ToList();
                if (path.Count >= 2) {
                    return new Path(path);
                }
            }
            return null;
        }

        static Vector3? ScreenPointToTerrain(Vector2 p)
        {
            Ray r = Camera.main.ScreenPointToRay(p);
            TerrainCollider terrainCollider = Terrain.activeTerrain.GetComponent<TerrainCollider>();
            RaycastHit hitInfo;
            if (terrainCollider.Raycast(r, out hitInfo, float.MaxValue)) {  //TODO: float.MaxValue is ba-a-a-d idea....
                return hitInfo.point;
            }
            return null;
        }

        private Path(IList<Vector3> points)
        {
            Points = points;
        }
    }
}

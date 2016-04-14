using UnityEngine;

namespace dbg
{
    static class GizmoUtils
    {
        public static void DrawCircleXZ(Vector3 center, float radius, Color color, int segments = 30)
        {
            Gizmos.color = color;
            for (int i = 0; i <= segments; i++) {
                Gizmos.DrawLine(
                    center + radius * new Vector3(Mathf.Sin(360.0f * i / segments), 0, Mathf.Cos(360.0f * i / segments)),
                    center + radius * new Vector3(Mathf.Sin(360.0f * (i + 1) / segments), 0, Mathf.Cos(360.0f * (i + 1) / segments))
                    );
            }
        }
    }
}

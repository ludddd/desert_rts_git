using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace ui.minimap
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(UILineRenderer))]
    class DrawCameraArea : MonoBehaviour
    {
        [SerializeField]
        private Camera sourceCamera = null;
        [SerializeField]
        private Camera targetCamera = null;
        private UILineRenderer lineRenderer;

        public void Start()
        {
            lineRenderer = GetComponent<UILineRenderer>();
            lineRenderer.Points = new Vector2[ViewportPoints.Length];
            lineRenderer.LineThickness = 1.0f / lineRenderer.canvas.scaleFactor;
        }

        private static readonly Vector3[] ViewportPoints = {
            new Vector3(0, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 0, 0),
            new Vector3(0, 0, 0),
        };

        public void OnRenderObject()
        {
            if (targetCamera == null)
            {
                return;
            }
            Draw(GetCameraArea());
        }

        private IEnumerable<Vector3> GetCameraArea()
        {
            var collider = Terrain.activeTerrain.GetComponent<TerrainCollider>();
            return ViewportPoints
                .Select(t => utils.Projection.ProjectToTerrain(sourceCamera, collider, t))
                .Select(t => targetCamera.WorldToScreenPoint(t));
        }

        private void Draw(IEnumerable<Vector3> points)
        {
            var array = points.ToArray();
            Debug.Assert(array.Length == lineRenderer.Points.Length);
            for (int i = 0; i < array.Length; i++) {
                lineRenderer.Points[i] = array[i];
            }
            lineRenderer.SetVerticesDirty();
        }
    }
}

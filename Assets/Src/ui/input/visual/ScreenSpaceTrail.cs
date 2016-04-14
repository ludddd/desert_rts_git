using System.Collections.Generic;
using UnityEngine;

namespace ui.input.visual
{

    public class ScreenSpaceTrail : MonoBehaviour
    {
        [SerializeField]
        private List<Vector2> points;
        [SerializeField]
        private Material material;
        [SerializeField]
        private Color color;
        [SerializeField]
        private float halfWidth;
        [SerializeField]
        private AnimationCurve textureFunc = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField]
        private float textureScale = 0.05f;

        [Header("Debug settings")]
        [SerializeField]
        private bool showTrailMesh = false;
        [SerializeField]
        private Color wireFrameColor;
        [SerializeField]
        private Color lineColor;

        private Vector3[] verticies;
        private Vector2[] uv;

        private void Start()
        {
            BuildMesh();
        }

        public void BuildMesh()
        {
            BuildGeom();
            BuildUV();
        }

        private void BuildGeom()
        {
            if (points.Count <= 1) {
                verticies = new Vector3[0];
                return;
            }

            verticies = new Vector3[points.Count * 2];

            InitPoint(0, halfWidth * NormalXYTo(EdgeDir(0)));
            for (int i = 1; i < points.Count - 1; i++) {
                InitPointWithBissect(i);
            }
            InitPoint(points.Count - 1, halfWidth * NormalXYTo(EdgeDir(points.Count - 2)));
        }

        public void BuildUV()
        {
            if (points.Count <= 1) {
                uv = new Vector2[0];
                return;
            }
            uv = new Vector2[points.Count * 2];
            float length = 0;
            for (int i = points.Count - 1; i >= 0; i--) {
                if (i < points.Count - 1) {
                    length += Vector2.Distance(points[i + 1], points[i]);
                }
                float v = textureFunc.Evaluate(textureScale * length / halfWidth);
                uv[2 * i] = new Vector2(0, v);
                uv[2 * i + 1] = new Vector2(1, v);
            }
        }

        private void InitPointWithBissect(int i)
        {
            Vector3 dirPrev = -EdgeDir(i-1).normalized;
            Vector3 dirNext = EdgeDir(i).normalized;
            Vector3 v = (dirPrev + dirNext).normalized;   //bissect
            if (Mathf.Approximately(v.sqrMagnitude, 0)) {   //segments are parallel
                v = halfWidth * NormalXYTo(dirNext);
            } else {
                float sin = Vector3.Cross(dirPrev, v).z;
                v *= halfWidth / sin;
            }
            InitPoint(i, v);
        }

        private void InitPoint(int i, Vector3 offset)
        {
            verticies[2 * i] = (Vector3)points[i] - offset;
            verticies[2 * i + 1] = (Vector3)points[i] + offset;
        }

        private Vector3 EdgeDir(int i)
        {
            return (points[i + 1] - points[i]).normalized;
        }

        private static Vector3 NormalXYTo(Vector3 v)
        {
            return Vector3.Cross(v, Vector3.forward).normalized;
        }

        private void OnGUI()
        {
            GL.PushMatrix();
            LoadGLProjMatrixForScreenSpace();
            DrawMesh();
            if (showTrailMesh) {
                DrawWireframe();
                DrawLines();
            }
            GL.PopMatrix();
        }

        private void DrawMesh()
        {
            material.SetPass(0);
            GL.Color(Color.white);
            GL.Begin(GL.TRIANGLE_STRIP);
            for (int i = 0; i < verticies.Length; i++) {
                GL.TexCoord(uv[i]);
                GL.Vertex(verticies[i]);
            }
            GL.End();
        }

        private void DrawWireframe()
        {
            UnityEngine.UI.Graphic.defaultGraphicMaterial.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(wireFrameColor);
            for (int i = 0; i < verticies.Length - 2; i++) {
                GL.Vertex(verticies[i]);
                GL.Vertex(verticies[i + 1]);
                GL.Vertex(verticies[i + 1]);
                GL.Vertex(verticies[i + 2]);
                GL.Vertex(verticies[i + 2]);
                GL.Vertex(verticies[i]);
            }
            GL.End();
        }

        private static void LoadGLProjMatrixForScreenSpace()
        {
            Matrix4x4 projMatr = Matrix4x4.Ortho(0, Screen.width, 0, Screen.height, -1, 100);
            GL.LoadProjectionMatrix(projMatr);
        }

        private void DrawLines()
        {
            UnityEngine.UI.Graphic.defaultGraphicMaterial.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(lineColor);
            for (int i = 1; i < points.Count; i++) {
                GL.Vertex(points[i - 1]);
                GL.Vertex(points[i]);
            }
            GL.End();
        }

        public void SetPoints(IEnumerable<Vector2> data)
        {
            points.Clear();
            points.AddRange(data);
            BuildMesh();
        }
    }
}


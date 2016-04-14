using System.Collections.Generic;
using grid.render;
using UnityEngine;

namespace fow
{
    [RequireComponent(typeof(Camera))]
    public class CameraFOW : MonoBehaviour
    {
        [SerializeField]
        private FOWRenderer fowRenderer = new FOWRenderer();
        [SerializeField]
        private Type type = Type.RectAsCircle;
        [SerializeField]
        private Quality quality = Quality.High;

        [Range(0, 1)]
        [SerializeField] private float exploredIntensity = 0.5f;

        enum Quality
        {
            High,
            Medium,
            Low,
            Lowest
        }

        public void Start()
        {
            var visComp = Terrain.activeTerrain.GetComponent<TerrainVisibilityComponent>();
            if (visComp == null) {  //TODO: this causes crash cause fowRenderer:Init is not called by acmera tryies to render fow
                return;
            }
            var cam = GetComponent<Camera>();
            float resMult = GetResolutionMultiplier(quality);
            var fowFabric = FOWFabric.Get(type);
            fowRenderer.Init((int)(resMult * cam.pixelWidth), (int)(resMult * cam.pixelHeight),
                new grid.BoolGridToTexture(visComp.ExploredArea, CreateFieldRenderer(visComp)),
                new grid.BoolGridToTexture(visComp.VisibleArea, CreateFieldRenderer(visComp)),
                fowFabric.CreateBlitMaterial(exploredIntensity)
                );
        }

        private IGridRenderer CreateFieldRenderer(TerrainVisibilityComponent visComp)
        {
            var fowFabric = FOWFabric.Get(type);
            return fowFabric.CreateRenderer(visComp.Grid, visComp.CellSize);
        }

        public void OnPreRender()
        {
            fowRenderer.Render(Camera.current);
            fowRenderer.ApplyFOW(Camera.current);
        }

        public void OnPostRender()
        {
            fowRenderer.ApplyNoFOW();
        }

        private static float GetResolutionMultiplier(Quality quality)
        {
            var map = new Dictionary<Quality, float>
            {
                { Quality.High, 1.0f },
                { Quality.Medium, 0.5f },
                { Quality.Low, 0.25f },
                { Quality.Lowest, 0.125f }
            };
            return map[quality];
        }
    }
}

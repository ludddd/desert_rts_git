using faction;
using fow;
using unit;
using UnityEngine;

namespace ui.minimap
{
    class DrawUnits: MonoBehaviour
    {
        [SerializeField]
        private Camera minimapCamera;
        [SerializeField]
        private Texture texture;
        [SerializeField]
        private Material material;
        [SerializeField]
        private int sizeInPixel;

        private void OnGUI()
        {
            if (Event.current.type != EventType.Repaint) return;
            var visibilityChecker = Terrain.activeTerrain.GetComponent<TerrainVisibilityComponent>();
            foreach (var unit in UnitUtils.GetAllUnits()) {
                var pos = unit.transform.position;
                if (visibilityChecker != null && !visibilityChecker.IsVisible(pos)) continue;
                DrawPoint(pos, GetUnitColor(unit));
            }
        }

        private static Color GetUnitColor(GameObject unit)
        {
            var factionData = Faction.GetFactionDataIfAny(unit);
            var color = factionData.HasValue ? factionData.Value.Color : Color.white;
            return color;
        }

        private void DrawPoint(Vector3 pos, Color color)
        {
            var screenPos = ToScreenSpace(pos);
            material.color = color;
            Graphics.DrawTexture(GetDrawRect(screenPos), texture, material);
        }

        private Vector3 ToScreenSpace(Vector3 worldPos)
        {
            var viewPos = minimapCamera.WorldToViewportPoint(worldPos);
            var localRect = ((RectTransform)transform).rect;
            var rectPos = new Vector3(localRect.xMin + viewPos.x * localRect.width, localRect.yMin + viewPos.y * localRect.height);
            var screenPos = transform.TransformPoint(rectPos);
            return InvertScreenY(screenPos);
        }

        private static Vector3 InvertScreenY(Vector3 screenPos)
        {
            return new Vector3(screenPos.x, Screen.height - screenPos.y);
        }

        private Rect GetDrawRect(Vector3 screenPos)
        {
            var size = sizeInPixel*Vector2.one;
            return new Rect(new Vector2(screenPos.x, screenPos.y) - 0.5f * size, size);
        }
    }
}

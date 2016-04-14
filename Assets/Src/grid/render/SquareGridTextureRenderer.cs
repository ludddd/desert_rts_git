using UnityEngine;

namespace grid.render
{
    class SquareGridTextureRenderer: IGridRenderer
    {
        private readonly Texture2D texture;
        private readonly Color32[] colorArray;

        public SquareGridTextureRenderer(Texture2D texture, int sizeX, int sizeY)
        {
            this.texture = texture;
            texture.Resize(sizeX, sizeY);
            colorArray = new Color32[sizeX * sizeY];
            Clear();
        }

        public void DrawCell(int cellIdx)
        {
            int y = cellIdx / texture.width;
            int x = cellIdx % texture.width;
            texture.SetPixel(x, y, Color.white);
        }

        public void Render()
        {
            texture.Apply();
        }

        public void Clear()
        {
            texture.SetPixels32(colorArray);
            texture.Apply();
        }
    }
}

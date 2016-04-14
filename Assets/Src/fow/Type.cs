using grid;
using grid.mesh;
using grid.render;
using UnityEngine;

namespace fow
{
    public enum Type
    {
        RectAsCircle,
        RectMesh,
        HexAsCircle,
        HexMesh
    }

    public abstract class FOWFabric
    {

        public static FOWFabric Get(Type type)
        {
            switch (type) {
                case Type.RectAsCircle:
                    return new CellAsCircleFabric();
                case Type.RectMesh:
                    return new NinePointFabric();
                case Type.HexAsCircle:
                    return new HexAsCircleFabric();
                case Type.HexMesh:
                    return new HexAsMeshFactory();
            }
            Debug.Assert(false, "Type is not supported");
            return null;
        }

        public abstract IGridRenderer CreateRenderer(IGrid grid, float cellSize);

        public Material CreateBlitMaterial(float blitCoeff)
        {
            var rez = new Material(Shader.Find("Custom/FOW/Blend"));
            rez.SetFloat("coeff", blitCoeff);
            return rez;
        }
    };

    public class HexAsCircleFabric : FOWFabric
    {
        public override IGridRenderer CreateRenderer(IGrid grid, float cellSize)
        {
            float overlappedCellSize = 1.2f * 2.0f * cellSize;
            return MeshGridRenderer.CreateSingleMeshGrid(grid, SquareXZMeshGenerator.Build(overlappedCellSize),
                CellAsCircleFabric.CreateVisibilityRangeMaterial());
        }
    }

    class CellAsCircleFabric: FOWFabric
    {
        public override IGridRenderer CreateRenderer(IGrid grid, float cellSize)
        {
            float overlappedCellSize = cellSize * Mathf.Sqrt(2) * 1.2f; //overlap cell circles
            return MeshGridRenderer.CreateSingleMeshGrid(grid,
                SquareXZMeshGenerator.Build(overlappedCellSize), CreateVisibilityRangeMaterial());
        }

        public static Material CreateVisibilityRangeMaterial()
        {
            var rez = new Material(Shader.Find("Custom/FOW/CircleFront"));
            rez.mainTexture = Resources.Load<Texture>("FOW/fow_gradient");
            return rez;
        }
    }

    class NinePointFabric: FOWFabric
    {
        public override IGridRenderer CreateRenderer(IGrid grid, float cellSize)
        {
            return MeshGridRenderer.CreatePoint9FowMesh(grid, cellSize, CreateVisibilityRangeMaterial());
        }

        public static Material CreateVisibilityRangeMaterial()
        {
            var rez = new Material(Shader.Find("Custom/FOW/Intensity"));
            return rez;
        }
    }

    public class HexAsMeshFactory : FOWFabric
    {
        public override IGridRenderer CreateRenderer(IGrid grid, float cellSize)
        {
            return MeshGridRenderer.CreateHexFowMesh(grid, NinePointFabric.CreateVisibilityRangeMaterial());
        }
    }
}

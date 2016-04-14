using System;
using System.Collections.Generic;
using UnityEngine;

namespace grid
{
    public class SquareGrid : IGrid
    {
        private Bounds bounds;
        public float CellSize {get; private set;}
        public int SizeX {get; private set;}
        public int SizeZ {get; private set;}

        private SquareGrid(Bounds area, float cellSize)
        {
            bounds = area;
            CellSize = cellSize;
            SizeX = Mathf.FloorToInt(bounds.size.x / cellSize);
            SizeZ = Mathf.FloorToInt(bounds.size.z / cellSize);
        }

        public static SquareGrid CreateForArea(Bounds area, float cellSize)
        {
            return new SquareGrid(MakeBoundsMultiplyOfSize(area, cellSize), cellSize);
        }

        public static Bounds MakeBoundsMultiplyOfSize(Bounds bounds, float size)
        {
            return new Bounds(bounds.center, new Vector3(size * Mathf.Ceil(bounds.size.x / size),
                                                         bounds.size.y,
                                                         size * Mathf.Ceil(bounds.size.z / size)));
        }

        public int CellCount
        {
            get
            {
                return SizeX * SizeZ;
            }
        }

        public Vector3 CellCenter(int cellId)
        {
            if (cellId < 0 || cellId >= CellCount) {
                throw new IndexOutOfRangeException(string.Format("cellId {0} is out of range [{1}, {2})", cellId, 0, CellCount));
            }
            int z = cellId / SizeZ;
            int x = cellId % SizeZ;
            return bounds.min + new Vector3((0.5f + x) * CellSize, 0, (0.5f + z) * CellSize);
        }

        public int PosToCellId(Vector3 pos)
        {
            if (!bounds.Contains(new Vector3(pos.x, bounds.center.y, pos.z))) {
                return CellIdx.Wrong;
            }
            int x = GetXIdx(pos.x);
            int z = GetZIdx(pos.z);
            return z * SizeX + x;
        }

        private int GetXIdx(float x)
        {
            return Mathf.Clamp(Mathf.FloorToInt((x - bounds.min.x) / CellSize), 0, SizeX - 1);
        }

        private int GetZIdx(float z)
        {
            return Mathf.Clamp(Mathf.FloorToInt((z - bounds.min.z) / CellSize), 0, SizeZ - 1);
        }

        public IEnumerable<int> CellIdxInCircle(Vector3 pos, float radius)
        {
            int xMin = GetXIdx(Mathf.Max(bounds.min.x, pos.x - radius));
            int xMax = GetXIdx(Mathf.Min(bounds.max.x, pos.x + radius));
            int zMin = GetZIdx(Mathf.Max(bounds.min.z, pos.z - radius));
            int zMax = GetZIdx(Mathf.Min(bounds.max.z, pos.z + radius));
            xMin = Mathf.Max(0, xMin);
            zMin = Mathf.Max(0, zMin);
            xMax = Mathf.Min(xMax, SizeX - 1);
            zMax = Mathf.Min(zMax, SizeZ - 1);
            int returnCount = 0;
            for (int z = zMin; z <= zMax; z++) {
                for (int x = xMin; x <= xMax; x++) {
                    var cellIdx = z * SizeX + x;
                    if (Vector3.Distance(CellCenter(cellIdx), pos) > radius) continue;
                    returnCount++;
                    yield return cellIdx;
                }
            }
            if (returnCount == 0)
            {
                //means radius is too small to capture any cell
                var centerCell = PosToCellId(pos);
                if (centerCell != CellIdx.Wrong)
                {
                    yield return centerCell;    //return at least cell pos points to
                }
            }
        }

        public bool IsIndexValid(int cellIdx)
        {
            return 0 <= cellIdx && cellIdx < SizeX * SizeZ;
        }

    }
}

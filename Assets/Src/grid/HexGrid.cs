using System;
using System.Collections.Generic;
using UnityEngine;

namespace grid
{
    public class HexGrid: IGrid
    {
        public enum Dir
        {
            TopRight,
            Right,
            BottomRight,
            BottomLeft,
            Left,
            TopLeft
        }

        private readonly Hex hex;
        private readonly int nColumn;
        private readonly int nRow;
        private readonly Vector3 org;

        public HexGrid(int nColumn, int nRow, float size):
            this(Vector3.zero, nColumn, nRow, size)
        {
        }

        public HexGrid(Vector3 org, int nColumn, int nRow, float size)
        {
            this.org = org;
            this.nColumn = nColumn;
            this.nRow = nRow;
            hex = new Hex(size);
        }

        public static HexGrid CreateForArea(Bounds bounds, float cellSize)
        {
            var hex = new Hex(cellSize);
            int nColumns = Mathf.CeilToInt((bounds.size.x + hex.HalfWidth)/hex.Width);
            int nRows = Mathf.CeilToInt((bounds.size.z + 0.5f * hex.HalfHeight)/(0.75f * hex.Height));
            //make symmetric
            float horzExtent = (0.5f + nColumns)*hex.Width - bounds.size.x;
            Debug.Assert(horzExtent >= 0.0f);
            float vertExtent = 0.75f*hex.Height*nRows - 0.25f * hex.Height - bounds.size.z;
            Debug.Assert(vertExtent >= 0.0f);
            var org = new Vector3(  bounds.min.x + 0.5f * (hex.Width - horzExtent), 0,
                                    bounds.min.z + 0.5f * (hex.HalfHeight - vertExtent));
            return new HexGrid(org, nColumns, nRows, cellSize);
        }

        public int PosToCellId(Vector3 pos)
        {
            int row = GetRow(pos);
            int column = GetColumn(pos, row);
            int cellId = GetCellIdx(column, row);
            //TODO: this cycle is unobvious (throw fixes duplication...)
            for (int i = 0; i < 2; i++) //check two cells at most
            {
                if (cellId == CellIdx.Wrong) return CellIdx.Wrong;
                var cellCenter = CellCenter(cellId);
                var cellHex = new Hex(hex.HalfHeight, cellCenter);
                if (cellHex.Contains(pos)) {
                    return cellId;
                }
                cellId = Go(cellId, pos.x > cellCenter.x ? Dir.BottomRight : Dir.BottomLeft);
            }            
            return CellIdx.Wrong;
        } 

        private int GetRow(Vector3 pos)
        {
            var posLocal = pos - org;
            int row = Mathf.FloorToInt((posLocal.z + hex.HalfHeight) / (0.75f * hex.Height));
            if (row == nRow && posLocal.z - 0.75f * hex.Height * (row - 1) < hex.Height) {
                row -= 1; //special case for last row
            }

            return row;
        }

        private int GetColumn(Vector3 pos, int row)
        {
            var posLocal = pos - org;
            return Mathf.FloorToInt((posLocal.x + (row % 2 == 0 ? hex.HalfWidth : 0)) / hex.Width);
        }

        public Vector3 CellCenter(int cellId)
        {
            if (!IsIndexValid(cellId)) {
                throw new ArgumentOutOfRangeException(string.Format("cellId {0} is out of range [0,{1})", cellId, CellCount));
            }
            int column = GetColumn(cellId);
            int row = GetRow(cellId);
            return org + new Vector3(hex.Width * column + (row % 2 == 0 ? 0 : hex.HalfWidth), 0, 0.75f * hex.Height * row);
        }

        private int GetRow(int cellId)
        {
            return Mathf.FloorToInt(cellId / nColumn);
        }

        private int GetColumn(int cellId)
        {
            return cellId % nColumn;
        }

        public int CellCount {
            get { return nColumn*nRow; }
        }

        public int NColumn {
            get { return nColumn;}
        }
        public int NRow {
            get { return nRow;}
        }

        public float CellSize
        {
            get { return hex.HalfHeight; }
        }

        public float CellWidth
        {
            get { return hex.Width; }
        }

        public IEnumerable<int> CellIdxInCircle(Vector3 pos, float radius)
        {
            int count = 0;
            int minRow = Math.Max(0, GetRow(pos - radius*Vector3.forward));
            int maxRow = Math.Min(GetRow(pos + radius*Vector3.forward), NRow - 1);
            for (int row = minRow; row <= maxRow; row++)
            {
                int minColumn = Math.Max(0, GetColumn(pos - radius*Vector3.right, row));
                int maxColumn = Math.Min(GetColumn(pos + radius*Vector3.right, row), NColumn - 1);
                for (int column = minColumn; column <= maxColumn; column++)
                {
                    var cellIdx = GetCellIdx(column, row);
                    if (!IsIndexValid(cellIdx)) continue;
                    var cellCenter = CellCenter(cellIdx);
                    if (Vector3.Distance(pos, cellCenter) <= radius)
                    {
                        count++;
                        yield return cellIdx;
                    }
                }
            }
            if (count == 0) //radius is too small to get any cell center
            {
                int cellIdx = PosToCellId(pos); //so lets check if pos belongs to any cell
                if (cellIdx != CellIdx.Wrong)
                {
                    yield return cellIdx;
                }
            }
        }

        public bool IsIndexValid(int cellIdx)
        {
            return 0 <= cellIdx && cellIdx < CellCount;
        }

        public int GetCellIdx(int column, int row)
        {
            if (row < 0 || nRow <= row) return CellIdx.Wrong;
            if (column < 0 || nColumn <= column) return CellIdx.Wrong;
            return row*NColumn + column;
        }

        public int Go(int fromCell, Dir dir)
        {
            int column = GetColumn(fromCell);
            int row = GetRow(fromCell);
            bool evenRow = row%2 == 0;
            switch (dir) {
                case Dir.BottomLeft:
                    return GetCellIdx(evenRow ? column - 1 : column, row - 1);
                case Dir.BottomRight:
                    return GetCellIdx(evenRow ? column : column + 1, row - 1);
                case Dir.Left:
                    return GetCellIdx(column - 1, row);
                case Dir.Right:
                    return GetCellIdx(column + 1, row);
                case Dir.TopLeft:
                    return GetCellIdx(evenRow ?  column - 1 : column, row + 1);
                case Dir.TopRight:
                    return GetCellIdx(evenRow ? column : column + 1, row + 1);
            }
            Debug.Assert(false, "unsupported direction");
            return CellIdx.Wrong;
        }

        public static Hex.Vertex[] VertexInDir(Dir dir)
        {
            return new [] {Hex.Vertex.Top + (int)dir, Hex.Vertex.Top + ((int)dir + 1) % 6};
        }
    }
}

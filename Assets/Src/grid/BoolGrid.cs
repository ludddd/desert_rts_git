using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace grid
{
    //unobvious class - can it be replaced with ICollection<int>?
    class BoolGrid: IGrid
    {
        private readonly IGrid impl;
        private readonly BitArray bitArray;

        public BoolGrid(IGrid gridImpl)
        {
            impl = gridImpl;
            bitArray = new BitArray(gridImpl.CellCount);
        }

        public bool Is(Vector3 pos)
        {
            int cellId = impl.PosToCellId(pos);
            return impl.IsIndexValid(cellId) && bitArray[cellId];
        }

        public void MarkArea(Vector3 center, float radius)
        {
            foreach (var idx in impl.CellIdxInCircle(center, radius)) {
                bitArray[idx] = true;
            }
        }

        public void Clear()
        {
            bitArray.SetAll(false);
        }

        public int CellCount
        {
            get
            {
                return impl.CellCount;
            }
        }

        public IEnumerable<int> GetVisibleCells()
        {
            for (int i = 0; i < bitArray.Length; i++) {
                if (bitArray[i]) {
                    yield return i;
                }
            }
        }

        public void Or(BoolGrid other)
        {
            bitArray.Or(other.bitArray);
        }

        public int PosToCellId(Vector3 pos)
        {
            return impl.PosToCellId(pos);
        }

        public Vector3 CellCenter(int cellId)
        {
            return impl.CellCenter(cellId);
        }

        public IEnumerable<int> CellIdxInCircle(Vector3 pos, float radius)
        {
            return impl.CellIdxInCircle(pos, radius);
        }

        public bool IsIndexValid(int cellIdx)
        {
            return impl.IsIndexValid(cellIdx);
        }
    }
}

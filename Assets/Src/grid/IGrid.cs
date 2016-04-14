using UnityEngine;
using System.Collections.Generic;

namespace grid
{
    public interface IGrid
    {        
        int PosToCellId(Vector3 pos);
        Vector3 CellCenter(int cellId);
        int CellCount { get; }
        IEnumerable<int> CellIdxInCircle(Vector3 pos, float radius);
        bool IsIndexValid(int cellIdx);
    }

    //Don't want to make it to full CellIdx class yet... 
    public class CellIdx
    {
        public const int Wrong = -1;
    }
}

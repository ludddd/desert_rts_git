using System;
using System.Collections.Generic;
using UnityEngine;

namespace grid
{
    public class Traveler
    {
        public enum Dir
        {
            NW, N, NE, W, E, SW, S, SE
        };

        private int sizeX;
        private int sizeY;

        public Traveler(int sizeX, int sizeY)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
        }

        public bool IsIndexValid(int idx)
        {
            return 0 <= idx && idx <= sizeX * sizeY;
        }

        public int N(int cellIdx)
        {
            if (!IsIndexValid(cellIdx)) {
                return CellIdx.Wrong;
            }
            return cellIdx >= sizeX ? cellIdx - sizeX : CellIdx.Wrong;
        }

        public int S(int cellIdx)
        {
            if (!IsIndexValid(cellIdx)) {
                return CellIdx.Wrong;
            }
            return cellIdx < sizeX * (sizeY - 1) ? cellIdx + sizeX : CellIdx.Wrong;
        }

        public int W(int cellIdx)
        {
            if (!IsIndexValid(cellIdx)) {
                return CellIdx.Wrong;
            }
            return cellIdx % sizeX > 0 ? cellIdx - 1 : CellIdx.Wrong;
        }

        public int E(int cellIdx)
        {
            if (!IsIndexValid(cellIdx)) {
                return CellIdx.Wrong;
            }
            return cellIdx % sizeX < sizeX - 1 ? cellIdx + 1 : CellIdx.Wrong;
        }

        public int NE(int cellIdx)
        {
            return N(E(cellIdx));
        }

        public int NW(int cellIdx)
        {
            return N(W(cellIdx));
        }

        public int SE(int cellIdx)
        {
            return S(E(cellIdx));
        }

        public int SW(int cellIdx)
        {
            return S(W(cellIdx));
        }

        public int Go(int from, Dir dir)
        {
            //What's up with performance?...
            IDictionary<Dir, Func<int, int>> map = new Dictionary<Dir, Func<int, int>>
            {
                {Dir.N, N},
                {Dir.NW, NW},
                {Dir.NE, NE},
                {Dir.W, W},
                {Dir.E, E},
                {Dir.S, S},
                {Dir.SW, SW},
                {Dir.SE, SE},
            };
            Debug.Assert(map.ContainsKey(dir));
            return map[dir](from);
        }        
    }
}

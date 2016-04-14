
using System;
using UnityEngine;

namespace grid
{
    public static class GridFactory
    {
        public enum Type
        {
            Square,
            Hex
        }

        public static IGrid CreateForArea(Type type, Bounds bounds, float cellSize)
        {
            switch (type)
            {
                    case Type.Square:
                        return SquareGrid.CreateForArea(bounds, cellSize);
                    case Type.Hex:
                        return HexGrid.CreateForArea(bounds, cellSize);
            }
            throw new ArgumentOutOfRangeException("type", string.Format("unsupported grid type {0}", Enum.GetName(typeof(Type), type)));
        }
    }
}

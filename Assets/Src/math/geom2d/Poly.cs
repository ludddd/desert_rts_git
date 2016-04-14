using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace math.geom2d
{
    public class Poly
    {
        private Vector2[] vertices;

        public Poly(IEnumerable<Vector2> points)
        {
            Debug.Assert(points.Count() > 2);
            vertices = points.ToArray();
        }

        public bool IsInside(Vector2 point)
        {
            //winding number test for a point in a polygon
            //code is taken from http://geomalgorithms.com/a03-_inclusion.html
            int windingNumber = 0;

            // loop through all edges of the polygon
            for (int i = 0; i < vertices.Length; i++) {                         // edge from V[i] to  V[i+1]
                int nextI = (i + 1) % vertices.Length;
                if (vertices[i].y <= point.y) {                                 // start y <= P.y
                    if (vertices[nextI].y > point.y) {                          // an upward crossing
                        if (IsLeft(vertices[i], vertices[nextI], point) > 0) {  // P left of  edge
                            ++windingNumber;                                    // have  a valid up intersect
                        }
                    }
                } else {                                                        // start y > P.y (no test needed)
                    if (vertices[nextI].y <= point.y) {                         // a downward crossing
                        if (IsLeft(vertices[i], vertices[nextI], point) < 0) {  // P right of  edge
                            --windingNumber;                                    // have  a valid down intersect
                        }
                    }
                }
            }
            return windingNumber != 0;  //windingNumber == 0 only when point is outside
        }

        private static int IsLeft(Vector2 P0, Vector2 P1, Vector2 P2)
        {
            float dotProduct = (P1.x - P0.x) * (P2.y - P0.y)
                                   - (P1.y - P0.y) * (P2.x - P0.x);
            return System.Math.Sign(dotProduct);
        }

    }
}

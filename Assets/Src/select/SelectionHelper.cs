using math.geom2d;
using System;
using System.Collections.Generic;
using System.Linq;
using unit;
using UnityEngine;

namespace select
{
    public static class SelectionHelper
    {
        private static readonly Plane GroundPlane = new Plane(Vector3.up, Vector3.zero);        

        public static void SelectUnitsInRect(this Selection selObj, Vector3 from, Vector3 to)
        {
            var rect = new Rect(Mathf.Min(from.x, to.x), Mathf.Min(from.z, to.z),
                                 Mathf.Abs(to.x - from.x), Mathf.Abs(to.z - from.z));
            var units = from unit in UnitUtils.GetAllUnits()
                        let pos = unit.transform.position
                        where rect.Contains(new Vector2(pos.x, pos.z))
                        select unit;
            var unitsAsSelectable = GameobjectToSelectables(units);
            selObj.AddMultiple(unitsAsSelectable);
        }

        public static void SelectUnitsInArea(this Selection selObj, Poly poly)
        {
            var units = from unit in UnitUtils.GetAllUnits()
                        let pos = unit.transform.position
                        where poly.IsInside(new Vector2(pos.x, pos.z))
                        select unit;
            var unitsAsSelectable = GameobjectToSelectables(units);
            selObj.AddMultiple(unitsAsSelectable);
        }

        public static void SelectByScreenSpaceArea(this Selection selObj, IEnumerable<Vector2> closedArea)
        {
            var poly = new Poly(closedArea.ToArray());
            var units = from unit in UnitUtils.GetAllUnits()
                let pos = Camera.main.WorldToScreenPoint(unit.transform.position)
                where poly.IsInside(new Vector2(pos.x, pos.y))
                select unit;
            var unitsAsSelectable = GameobjectToSelectables(units);
            selObj.AddMultiple(unitsAsSelectable);
        }

        public static void SelectOnScreenByPredicate(this Selection selObj, Func<GameObject, bool> predicate)
        {
            var poly = ScreenAreaInWorldSpace();
            var units = from unit in UnitUtils.GetAllUnits()
                        let pos = unit.transform.position
                        where poly.IsInside(new Vector2(pos.x, pos.z))
                        where predicate(unit)
                        select unit;
            var unitsAsSelectable = GameobjectToSelectables(units);
            selObj.AddMultiple(unitsAsSelectable);
        }

        private static Poly ScreenAreaInWorldSpace()
        {
            var screenArea = new[]
                                    {
                            new Vector2(0, 0),
                            new Vector2(Camera.main.pixelWidth, 0),
                            new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight),
                            new Vector2(0, Camera.main.pixelHeight),
                                    };
            var pointsInWorldSpace = ScreenToWorldPlane(screenArea, GroundPlane);
            return new Poly(pointsInWorldSpace.ToArray());
        }

        public static IEnumerable<GameObject> AsGameObjects(this Selection selObj)
        {
            return SelectablesToGameobjects(selObj.Objects);
        }

        public static GameObject FirstSelectedUnit(this Selection selObj)
        {
            return selObj.AsGameObjects().First();
        }

        private static IEnumerable<Vector2> ScreenToWorldPlane(IEnumerable<Vector2> arr, Plane plane)
        {
            return (from p in arr let v = ScreenPointToPlane(p, plane) select new Vector2(v.x, v.z));
        }

        private static Vector3 ScreenPointToPlane(Vector2 p, Plane plane)
        {
            var r = Camera.main.ScreenPointToRay(p);
            float dist;
            plane.Raycast(r, out dist);
            return r.GetPoint(dist);
        }

        public static IEnumerable<ISelectable> GameobjectToSelectables(IEnumerable<GameObject> items)
        {
            return from item in items
                   let component = item.GetComponent<SelectableUnit>()
                   where component != null
                   select (ISelectable)component;
        }

        public static IEnumerable<GameObject> SelectablesToGameobjects(IEnumerable<ISelectable> items)
        {
            return from item in items
                   let component = item as MonoBehaviour
                   where component != null
                   select component.gameObject;
        }
    }
}

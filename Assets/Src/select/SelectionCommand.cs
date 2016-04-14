using System.Linq;
using faction;
using unit;
using utils;
using UnityEngine;

namespace select
{
    class SelectionCommand: MonoBehaviour
    {
        public void SelectUnitsOnScreen()
        {
            var cam = Camera.main;
            var viewBounds = ViewSpaceBounds(cam);
            var units = from unit in UnitUtils.GetAllUnits()
                        let pos = cam.WorldToViewportPoint(unit.transform.position)
                        where viewBounds.Contains(pos)
                        where PlayerFaction.IsPlayerFaction(unit)
                        select unit;
            var unitsAsSelectable = SelectionHelper.GameobjectToSelectables(units);
            Selection.instance.AddMultiple(unitsAsSelectable);
        }

        private static Bounds ViewSpaceBounds(Camera cam)
        {
            return new Bounds
            {
                min = new Vector3(0, 0, cam.nearClipPlane),
                max = new Vector3(1, 1, cam.farClipPlane)
            };
        }

        public void SelectOneGameObject(GameObject obj)
        {
            var selectable = obj.GetComponentWithInterface<ISelectable>();
            if (selectable != null)
            {
                Selection.instance.Clear();
                Selection.instance.Add(selectable);
            }
        }
    }
}

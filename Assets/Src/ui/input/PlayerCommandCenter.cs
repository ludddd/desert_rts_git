using select;
using System.Collections.Generic;
using System.Linq;
using ai.behavior.movement;
using ai.command;
using UnityEngine;
using faction;
using wpn;

namespace ui.input
{
    public class PlayerCommandCenter: MonoBehaviour
    {
        public void SelectUnit(GameObject unit)
        {
            Selection.instance.Clear();
            var component = unit.GetComponent<SelectableUnit>();
            if (component) {
                Selection.instance.Add(component);
            }
        }

        public void SelectArea(IEnumerable<Vector2> area)
        {
            Selection.instance.Clear();
            Selection.instance.SelectByScreenSpaceArea(area);
        }

        public void SelectAllUnitsOfTypeAndFactionOnScreen(GameObject unit)
        {
            var faction = unit.GetComponent<Faction>();
            if (faction == null) {
                return;
            }
            var unitType = unit.GetComponent<unit.UnitType>();
            if (unitType == null) {
                return;
            }
            Selection.instance.Clear();
            Selection.instance.SelectOnScreenByPredicate(obj => unitType.SameType(obj));
        }

        public void SelectAllUnitsOfFactionOnScreen(GameObject unit)
        {
            var faction = unit.GetComponent<Faction>();
            if (faction == null) {
                return;
            }
            var factionId = faction.FactionId;
            Selection.instance.Clear();
            Selection.instance.SelectOnScreenByPredicate(obj => factionId.IsSameFaction(obj));
        }

        public void ClearSelection(Vector3 pos)
        {
            Selection.instance.Clear();
        }

        public void GoTo(Vector3 pos)
        {
            ai.command.GroupGoTo.GoTo(Selection.instance.AsGameObjects().ToList(), pos);
        }

        public void FollowPath(IList<Vector3> path)
        {
            ai.command.GroupGoTo.FollowPath(Selection.instance.AsGameObjects().ToList(), path);
        }

        public void Attack(GameObject target)
        {
            foreach (var unit in Selection.instance.AsGameObjects()) {
                var cmdExec = unit.GetComponent<ai.CommandExecutor>();
                if (!cmdExec) {
                    continue;
                }
                SingleUnit.Attack(cmdExec, GameObjectTarget.FromGameObject(target));
            }
        }

        public void StopSelectedUnits()
        {
            foreach (var obj in Selection.instance.AsGameObjects()) {
                var cmdExec = obj.GetComponent<ai.CommandExecutor>();
                if (!cmdExec) {
                    continue;
                }
                cmdExec.SetMovement(new Base(cmdExec.NavMeshAgent));
            }
        }
    }
}

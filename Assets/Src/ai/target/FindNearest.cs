using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using faction;
using System;
using unit;
using wpn;

namespace ai.target
{
    public class FindNearest
    {
        public static ITarget Get(Vector3 selfPos, FactionData.FactionId factionId, float range, Func<ITarget, bool> predicate = null)
        {
            var units = UnitUtils.GetAllUnits();
            units = units.Where(factionId.IsFoe);
            units = units.Where(unit => Vector3.Distance(selfPos, unit.transform.position) <= range);
            if (predicate != null) {
                units = units.Where(unit => predicate(GameObjectTarget.FromGameObject(unit)));  //TODO: this can be sloooow...
            }
            var rez = units.OrderBy(unit => (unit.transform.position - selfPos).sqrMagnitude).FirstOrDefault();
            return rez == null ? InvalidTarget.Invalid : GameObjectTarget.FromGameObject(rez);
        }

        public static IEnumerable<ITarget> GetTargetsInRange(GameObject self, float range)
        {
            var faction = self.GetComponent<Faction>();
            if (faction == null) {
                return Enumerable.Empty<ITarget>();
            }
            var factionId = faction.FactionId;
            var selfPos = self.transform.position;
            var units = UnitUtils.GetAllUnits();
            units = units.Where(unit => factionId.IsFoe(unit));
            var targets = units.Select<GameObject, ITarget>(GameObjectTarget.FromGameObject);
            return targets.Where(target => Vector3.Distance(target.Pos, selfPos) <= range);
        }
    }
}


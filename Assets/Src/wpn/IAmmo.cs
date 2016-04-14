using UnityEngine;

namespace wpn
{
    interface IAmmo
    {
        void Setup(Vector3 from, Vector3 dir, Weapon weapon, faction.FactionData.FactionId factionId, ITarget target);
    }
}

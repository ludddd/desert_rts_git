using UnityEngine;

namespace faction
{
    static class FriendOrFoe
    {
        public static bool IsFoe(this FactionData.FactionId factionId, GameObject other)
        {
            return !IsSameFaction(factionId, other);
        }

        public static bool IsFriend(this FactionData.FactionId factionId, GameObject other)
        {
            return IsSameFaction(factionId, other);
        }

        public static bool IsSameFaction(this FactionData.FactionId factionId, GameObject other)
        {
            var otherFaction = other.GetComponent<Faction>();
            if (!otherFaction) {
                return false;
            }
            return factionId.IsSameFaction(otherFaction.FactionId);
        }

    }
}

using UnityEngine;

namespace faction
{
    static class PlayerFaction
    {
        private static FactionData.FactionId cachedPlayerFactionId;

        public static FactionData.FactionId FactionId
        {
            get
            {
                if (cachedPlayerFactionId == null)
                {
                    cachedPlayerFactionId = GetPlayerId();
                }
                return cachedPlayerFactionId;
            }
        }

        private static FactionData.FactionId GetPlayerId()
        {
            var player = GameObject.FindGameObjectWithTag(Tags.Player);
            if (player == null) { return FactionData.FactionId.NoFaction;}
            var faction = player.GetComponent<Faction>();
            return faction.FactionId;
        }

        public static bool IsPlayerFaction(this FactionData.FactionId id)
        {
            return id.Equals(FactionId);
        }

        public static bool IsPlayerFaction(GameObject obj)
        {
            return IsPlayerFaction(Faction.GetFactionId(obj));
        }
    }
}

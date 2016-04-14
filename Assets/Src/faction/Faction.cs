using UnityEngine;

namespace faction
{
    public class Faction : MonoBehaviour
    {
        [SerializeField]
        protected FactionData.FactionId factionId;

        virtual public FactionData.FactionId FactionId
        {
            get
            {
                return factionId;
            }
        }

        public static FactionData.Data? GetFactionDataIfAny(GameObject gameObject)
        {
            var factionId = GetFactionId(gameObject);
            if (factionId.Equals(FactionData.FactionId.NoFaction) || FactionData.Instance == null)
            {
                return null;
            }
            return factionId.GetFactionData();
        }

        //TODO: replace all self.GetComponent<Faction> with this
        public static FactionData.FactionId GetFactionId(GameObject gameObject)
        {
            var faction = gameObject.GetComponent<Faction>();
            return faction == null ? FactionData.FactionId.NoFaction : faction.FactionId;
        }
    }
}
namespace faction
{
    public class FactionFromParent : Faction
    {
        override public FactionData.FactionId FactionId
        {
            get
            {
#if UNITY_EDITOR
                return GetFactionIdFromParent();
#else
                return base.FactionId;
#endif
            }
        }

        private FactionData.FactionId GetFactionIdFromParent()
        {
            if (transform.parent != null) {
                var parentFaction = transform.parent.gameObject.GetComponentInParent<Faction>();
                if (parentFaction != null) {
                    return parentFaction.FactionId;
                }
            }            
            return base.FactionId;
        }

        public void Awake()
        {
            factionId = GetFactionIdFromParent();
        }
    }
}

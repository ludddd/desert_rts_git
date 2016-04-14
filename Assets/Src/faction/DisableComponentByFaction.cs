using UnityEngine;

namespace faction
{
    [RequireComponent(typeof(Faction))]
    class DisableComponentByFaction : MonoBehaviour
    {
        private static readonly System.Type[] componentsToDisable = { typeof(fow.FOWVisor) };

        public void Start()
        {
            var faction = GetComponent<Faction>().FactionId;
            if (faction.IsPlayerFaction()) {
                return;
            }
            foreach (var type in componentsToDisable) {
                if (!type.IsSubclassOf(typeof(MonoBehaviour))) {
                    continue;
                }
                foreach (var component in gameObject.GetComponents(type)) {
                    ((MonoBehaviour)component).enabled = false;
                }
            }
        }
    }
}

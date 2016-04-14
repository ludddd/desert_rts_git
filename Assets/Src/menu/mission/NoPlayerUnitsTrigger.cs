using faction;
using unit;
using UnityEngine;
using UnityEngine.Events;

namespace menu.mission
{
    class NoPlayerUnitsTrigger: MonoBehaviourEx
    {
        [SerializeField]
        private UnityEvent triggerEvent;
        [SerializeField]
        private FactionData.FactionId faction;

        private void Update()
        {
            if (AreTherePlayerUnits()) return;
            Signal();
        }

        private void Signal()
        {
            triggerEvent.Invoke();
            enabled = false;
        }

        private bool AreTherePlayerUnits()
        {
            foreach (var unit in UnitUtils.GetAllUnits())
            {
                var unitFaction = unit.GetComponent<Faction>();
                if (unitFaction != null && unitFaction.FactionId.IsPlayerFaction())
                {
                    return true;
                }
            }
            return false;
        }

        [editor.attr.Button]
        public void Test()
        {
            Signal();
        }
    }
}

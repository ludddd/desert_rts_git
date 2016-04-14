using faction;
using unit;
using utils;
using UnityEngine;

namespace trigger
{
    [RequireComponent(typeof(Collider))]
    class UnitEnterVolume : MonoBehaviour
    {
        [SerializeField]
        private UnityEventWithGameObject OnEnter;
        [SerializeField]
        private bool signalOnce = false;

        private void Start()
        {
            var colliderComp = GetComponent<Collider>();
            Debug.Assert(colliderComp.isTrigger, "Collider should be set in triiger mode");
        }

        private void OnTriggerEnter(Collider other)
        {
            var obj = other.gameObject;
            if (IsUnit(obj) && IsPlayerFaction(obj))
            {
                SignalEvent(obj);
            }
        }

        private void SignalEvent(GameObject obj)
        {
            OnEnter.Invoke(obj);
            if (signalOnce)
            {
                gameObject.SetActive(false);
            }
        }

        private static bool IsUnit(GameObject obj)
        {
            return obj.IsUnit();
        }

        private static bool IsPlayerFaction(GameObject obj)
        {
            var faction = obj.GetComponent<Faction>();
            return faction != null && faction.FactionId.IsPlayerFaction();
        }
    }
}

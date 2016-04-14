using ai;
using UnityEngine;

namespace unit
{
    class UnitType: MonoBehaviour
    {
        [SerializeField]
        private string typeName;

        public string TypeName
        {
            get
            {
                return typeName;
            }
        }

        public bool SameType(GameObject other)
        {
            var comp = other.GetComponent<UnitType>();
            return comp != null && typeName.Equals(comp.typeName);
        }
    }
}
